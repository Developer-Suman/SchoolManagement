using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Queries.FiscalYearStartDate;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.CustomMiddleware.CustomException;
using TN.Shared.Infrastructure.Data;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class FiscalYearServices : IFiscalYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly FiscalContext _fiscalContext;
        private readonly IMapper _mapper;
        private readonly ISettingServices _settingServices;


        public FiscalYearServices(IUnitOfWork unitOfWork, ITokenService tokenService, FiscalContext fiscalContext,IMapper mapper, ISettingServices settingServices)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _fiscalContext = fiscalContext;
            _settingServices = settingServices;
        }

        public async Task<Result<CloseFiscalYearResponse>> CloseFiscalYear(CloseFiscalYearCommand request)
        {

           using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
        

                    var schoolId = _tokenService.SchoolId().FirstOrDefault();

                    #region Running CompanyDate and doesnot give access when closed fyId beyond the currentDate
                    var currentFiscalYearsToBeClosed = (await _unitOfWork.BaseRepository<FiscalYears>()
                   .GetConditionalAsync(
                       predicate: fy => fy.Id == request.closedFiscalId,
                       includes: fy => fy.SchoolSettingsFiscalYears
                   )).FirstOrDefault();

                    var schoolSettingFiscalYear = currentFiscalYearsToBeClosed.SchoolSettingsFiscalYears?
                        .FirstOrDefault(x => x.SchoolId == schoolId)
                        ?? throw new Exception("No SchoolSettingsFiscalYear linked to this fiscal year.");

                    var runningSchoolSettingFiscalYear = (await _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>()
                        .FindBy(fy => fy.SchoolSettingsId == schoolSettingFiscalYear.SchoolSettingsId && !fy.IsClosed))
                        .FirstOrDefault()
                        ?? throw new Exception("No current SchoolSettingsFiscalYear found.");

                    var runningFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                        .GetSingleAsync(fy => fy.Id == runningSchoolSettingFiscalYear.FiscalYearId)
                        ?? throw new Exception("Running FiscalYear not found.");

                    var runningFiscalYearStartDate = runningFiscalYear.StartDate.Date;
                    var currentDate = DateTime.Now.Date;
                    
                    if (runningFiscalYearStartDate < currentDate)
                    {
                        throw new NotFoundExceptions("You won't go beyond the current date.");
                        //return Result<List<GetSelectedFiscalYearQueryResponse>>.Failure("");
                    }

                    #endregion

                    var currentFiscalYears = (await _unitOfWork.BaseRepository<FiscalYears>()
                       .GetConditionalAsync(
                           predicate: fy => fy.Id == _fiscalContext.CurrentFiscalYearId,
                           includes: fy => fy.SchoolSettingsFiscalYears
                       )).FirstOrDefault();

                    if (currentFiscalYears == null)
                        throw new Exception("Fiscal year not found.");

                    if (currentFiscalYears.SchoolSettingsFiscalYears == null || !currentFiscalYears.SchoolSettingsFiscalYears.Any())
                        throw new Exception("No CompanySettingsFiscalYear linked to this fiscal year.");
              

                    var schoolSettingsId = currentFiscalYears.SchoolSettingsFiscalYears
                        .Where(x=>x.SchoolId == schoolId)
                        .Select(x => x.SchoolSettingsId)
                        .FirstOrDefault();
     
                    var fiscalYear = await _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>()
                    .FirstOrDefaultAsync(x => x.FiscalYearId == request.closedFiscalId && x.SchoolSettingsId == schoolSettingsId && x.SchoolId == schoolId);

                    if (fiscalYear == null || fiscalYear.IsClosed)
                        throw new Exception("Fiscal year not found or already closed.");


                    var ledgerBalances = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                          .GetConditionalFilterType(
                              x => x.FiscalId == request.closedFiscalId,
                              query => query
                                  .GroupBy(g => g.LedgerId)
                                  .Select(g => new
                                  {
                                      LedgerId = g.Key,
                                      Balance = g.Sum(x => x.CreditAmount - x.DebitAmount)
                                  })
                          );

                    var closingBalances = ledgerBalances.Select(b =>
                        new ClosingBalance(
                            Guid.NewGuid().ToString(),
                            b.LedgerId,
                            request.closedFiscalId,
                            b.Balance,
                            _tokenService.GetUserId(),
                            DateTime.UtcNow
                        )
                    ).ToList();



        



                    await _unitOfWork.BaseRepository<ClosingBalance>().AddRange(closingBalances);

                    fiscalYear.IsClosed = true;
                    fiscalYear.UserId = _tokenService.GetUserId();
                    fiscalYear.ClosedAt = DateTime.UtcNow;

                    _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>().Update(fiscalYear);

                    var currentFiscalYear = (await _unitOfWork.BaseRepository<FiscalYears>()
                          .GetConditionalAsync(fy => fy.Id == _fiscalContext.CurrentFiscalYearId))
                          .SingleOrDefault();


                    var nextFiscalYear = (await _unitOfWork.BaseRepository<FiscalYears>()
                       .GetConditionalAsync(
                           fy => fy.StartDate > currentFiscalYear.StartDate,
                           q => q.OrderBy(fy => fy.StartDate)
                       ))
                       .FirstOrDefault();



                    var schoolSettingsFiscalYear = new SchoolSettingsFiscalYear
                        (
                        Guid.NewGuid().ToString(),
                        fiscalYear.SchoolSettingsId,
                        nextFiscalYear.Id,
                        false,
                        "",
                        DateTime.UtcNow,
                        true,
                        nextFiscalYear.FyName,
                        schoolId,
                        false
                        );
                    await _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>().AddAsync(schoolSettingsFiscalYear);


                    #region Update IsUpToCurrentFiscalYear
                    fiscalYear.IsUpToCurrentFiscalYear = false;
                    _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>().Update(fiscalYear);

                    #endregion


                    #region Update CurrentFyId
                    var updateFiscalYear = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x => x.SchoolId == schoolId);
                    updateFiscalYear.CurrentFiscalYearId = nextFiscalYear.Id;

                    _unitOfWork.BaseRepository<SchoolSettings>().Update(updateFiscalYear);
                    #endregion

                    var openingBalances = closingBalances.Select(cb => new OpeningBalance
                    (
                        Guid.NewGuid().ToString(),
                        cb.LedgerId,
                        nextFiscalYear.Id,
                        cb.Balance
                    )).ToList();

                    await _unitOfWork.BaseRepository<OpeningBalance>().AddRange(openingBalances);


                    //if (request.autoOpenNext)
                    //{


                    //}
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    var response = new CloseFiscalYearResponse
                    (fiscalYear.FiscalYearId,
                        "Fiscal year closed and balances carried forward"
                    );

                    return Result<CloseFiscalYearResponse>.Success(response);

                }
       

            
        }

        public async Task<AcademicYear?> GetAcademicYearFromSettingsAsync()
        {
            var schoolId = _tokenService.SchoolId().FirstOrDefault();

            var settingsList = await _unitOfWork.BaseRepository<SchoolSettings>()
                .GetConditionalAsync(
                    x => x.SchoolId == schoolId && x.IsActive == true,
                    null,
                    x => x.AcademicYear
                );

            var settings = settingsList.FirstOrDefault();
            return settings?.AcademicYear;
        }

        public async Task<FiscalYears?> GetCurrentFiscalYearFromSettingsAsync()
        {
            var schoolId = _tokenService.SchoolId().FirstOrDefault();

            var settingsList = await _unitOfWork.BaseRepository<SchoolSettings>()
                .GetConditionalAsync(
                    x => x.SchoolId == schoolId,
                    null,
                    x => x.CurrentFiscalYear
                );

            var settings = settingsList.FirstOrDefault();
            return settings?.CurrentFiscalYear;

        }

        public async Task<(string Id, string FyName)> GetFiscalYearIdForDateAsync(DateTime date)
        {
            var fiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
            .GetSingleAsync(fy => date >= fy.StartDate && date <= fy.EndDate);

            if (fiscalYear == null)
                throw new InvalidOperationException("No fiscal year found for the given date.");

            return (fiscalYear.Id, fiscalYear.FyName);
        }

        public async Task<Result<FiscalYearStartDateResponse>> GetFiscalYearStartDate()
        {

            var currentFiscalYears = (await _unitOfWork.BaseRepository<FiscalYears>()
                      .GetConditionalAsync(
                          predicate: fy => fy.Id == _fiscalContext.CurrentFiscalYearId,
                          includes: fy => fy.SchoolSettingsFiscalYears
                      )).FirstOrDefault();

            if (currentFiscalYears == null)
                throw new Exception("Fiscal year not found.");

            if (currentFiscalYears.SchoolSettingsFiscalYears == null || !currentFiscalYears.SchoolSettingsFiscalYears.Any())
                throw new Exception("No SchoolSettingsFiscalYear linked to this fiscal year.");
            var schoolId = _tokenService.SchoolId().FirstOrDefault();
            // Now safe to extract
            var schoolSettingsId = currentFiscalYears.SchoolSettingsFiscalYears
                .Where(x => x.SchoolId == schoolId)
                .Select(x => x.SchoolSettingsId)
                .FirstOrDefault();

            var runningFiscalYearInSchoolSettingFy = (await _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>()
                .FindBy(fy => fy.SchoolSettingsId == schoolSettingsId && !fy.IsClosed))
                .FirstOrDefault();

            if (runningFiscalYearInSchoolSettingFy == null)
                throw new Exception("No Current SchoolSettingsFiscalYear found.");


            var runningFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                    .GetSingleAsync(fy => fy.Id == runningFiscalYearInSchoolSettingFy.FiscalYearId);

            var runningFiscalYearStartDate = runningFiscalYear?.StartDate.ToString("yyyy-MM-dd");

            var openingCompanyFiscalYearInCompanySettingsFy = (await _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>()
                .FindBy(fy => fy.SchoolSettingsId == schoolSettingsId && fy.IsFiscalYearStarted))
                .FirstOrDefault(fy=>fy.IsFiscalYearStarted);

            if (openingCompanyFiscalYearInCompanySettingsFy == null)
                throw new Exception("No open SchoolSettingsFiscalYear found.");


            var openingSchoolFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>()
                .GetSingleAsync(fy => fy.Id == openingCompanyFiscalYearInCompanySettingsFy.FiscalYearId);


            var openingSchoolFiscalYearStartDate = openingSchoolFiscalYear?.StartDate.ToString("yyyy-MM-dd");
            return Result<FiscalYearStartDateResponse>.Success(
                new FiscalYearStartDateResponse
                (
                    runningFiscalYearStartDate: runningFiscalYearStartDate,
                    openingCompanyFiscalYearStartDate: openingSchoolFiscalYearStartDate
                )
            );
   
        }

        public async Task<Result<List<GetSelectedFiscalYearQueryResponse>>> GetSelectedFiscalYear(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
         
                var currentFiscalYears = (await _unitOfWork.BaseRepository<FiscalYears>()
                      .GetConditionalAsync(
                          predicate: fy => fy.Id == _fiscalContext.CurrentFiscalYearId,
                          includes: fy => fy.SchoolSettingsFiscalYears
                      )).FirstOrDefault();

                if (currentFiscalYears == null)
                    throw new Exception("Fiscal year not found.");

                if (currentFiscalYears.SchoolSettingsFiscalYears == null || !currentFiscalYears.SchoolSettingsFiscalYears.Any())
                    throw new Exception("No SchoolSettingsFiscalYear linked to this fiscal year.");
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                // Now safe to extract
                var schoolSettingsId = currentFiscalYears.SchoolSettingsFiscalYears
                    .Where(x => x.SchoolId == schoolId)
                    .Select(x => x.SchoolSettingsId)
                    .FirstOrDefault();




         




                var selectedFiscalYear = await _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>()
                    .FindBy(fy =>
                        fy.SchoolSettingsId == schoolSettingsId);

                var selectedFiscalYearList = await selectedFiscalYear
                    .Select(x => new GetSelectedFiscalYearQueryResponse(x.FiscalYearId, x.FyName))
                    .ToListAsync();


                var response = _mapper.Map<List<GetSelectedFiscalYearQueryResponse>>(selectedFiscalYearList);

                return Result<List<GetSelectedFiscalYearQueryResponse>>.Success(response);

  

        }

        public bool IsDateInFiscalYear(DateTime date, FiscalYears fiscalYear)
        {
            return date >= fiscalYear.StartDate && date <= fiscalYear.EndDate;
        }
    }
}
