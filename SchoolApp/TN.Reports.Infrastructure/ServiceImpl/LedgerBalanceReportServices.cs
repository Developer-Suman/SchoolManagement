using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Reports.Application.Annex13.Queries;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Reports.Application.LedgerBalance.Queries.LedgerSummary;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class LedgerBalanceReportServices : ILedgerBalanceServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IGetUserScopedData _getUserScopedData;


        public LedgerBalanceReportServices(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IDateConvertHelper dateConvertHelper, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _dateConvertHelper = dateConvertHelper;
            _getUserScopedData = getUserScopedData;
        }



        public async Task<Result<PagedResult<LedgerBalanceReportQueryResponse>>> GetLedgerBalanceReportByLedger(PaginationRequest paginationRequest, LedgerBalanceDTOs ledgerBalanceDTOs)
        {
            try
            {
                var (journalEntry, currentSchoolId, institutionId, userRole, isSuperAdmin) =
    await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                DateTime startEnglishDate = ledgerBalanceDTOs.startDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(ledgerBalanceDTOs.startDate);

                DateTime endEnglishDate = ledgerBalanceDTOs.endDate == default
                    ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(ledgerBalanceDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);

                // Get list of SchoolsId for institution scope (used later if needed)
                var institutionSchoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                // Filter journal entries by company scope
                IQueryable<JournalEntryDetails> filterJournalEntry;

                if (!string.IsNullOrEmpty(ledgerBalanceDTOs.requestedSchoolId))
                {
                    filterJournalEntry = journalEntry.Where(x => x.SchoolId == ledgerBalanceDTOs.requestedSchoolId);
                }
                else if (!string.IsNullOrEmpty(currentSchoolId) && !isSuperAdmin)
                {
                    filterJournalEntry = journalEntry.Where(x => x.SchoolId == currentSchoolId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    filterJournalEntry = journalEntry.Where(x => institutionSchoolIds.Contains(x.SchoolId));
                }
                else
                {
                    filterJournalEntry = journalEntry;
                }

                // Now apply the main filter for date range and ledgerId
                var filteredLedgerData = await filterJournalEntry
                    .Where(sd =>
                        (string.IsNullOrEmpty(ledgerBalanceDTOs.ledgerId) || sd.LedgerId == ledgerBalanceDTOs.ledgerId) &&
                        sd.TransactionDate >= startEnglishDate &&
                        sd.TransactionDate <= endEnglishDate
                    )
                    .GroupBy(c => new { c.SchoolId, c.LedgerId })
                    .Select(g => new LedgerBalanceReportQueryResponse(
                        g.Key.SchoolId,
                        g.Key.LedgerId,
                        g.Sum(x => x.CreditAmount - x.DebitAmount)
                    ))
                    .ToListAsync();

                PagedResult<LedgerBalanceReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = filteredLedgerData.Count();

                    var pagedItems = filteredLedgerData
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<LedgerBalanceReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<LedgerBalanceReportQueryResponse>
                    {
                        Items = filteredLedgerData.ToList(),
                        TotalItems = filteredLedgerData.Count(),
                        PageIndex = 1,
                        pageSize = filteredLedgerData.Count()
                    };
                }


                return Result<PagedResult<LedgerBalanceReportQueryResponse>>.Success(finalResponseList);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching all the report using {ledgerBalanceDTOs.ledgerId}");
            }
        }

        public async Task<Result<PagedResult<LedgerSummaryResponse>>> GetLedgerSummaryByLedger(PaginationRequest paginationRequest, string ledgerId)
        {
            try
            {

                #region Logic that we should try
                //var journalEntryIds = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                //    .GetConditionalFilterType(sd =>
                //        sd.LedgerId == ledgerId && sd.DebitAmount != 0,
                //        query => query.Select(c => c.JournalEntryId)
                //    );

                //var journalEntryDetails = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                //.GetConditionalAsync(sd =>
                //    sd.LedgerId == ledgerId && journalEntryIds.Contains(sd.JournalEntryId)
                //);


                //var mappedLedgerSummaryDetails = _mapper.Map<PagedResult<LedgerSummaryResponse>>(LedgerSummaryDetails.ToList());
                // var journalEntryDetails = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                //.GetConditionalFilterType(
                //    c => journalEntryIds.Contains(c.JournalEntryId),
                //    query => query.AsNoTracking()
                //);

                // var debitList = journalEntryDetails
                //     .Where(c => c.DebitAmount > 0)
                //     .GroupBy(c => new { c.JournalEntryId, c.LedgerId })
                //     .Select(g => new LedgerSummaryResponse(
                //         g.Key.JournalEntryId,
                //         g.Key.LedgerId,
                //         g.Sum(c => c.DebitAmount).ToString("0.00"),
                //         "0.00"  // Only debit amount, credit is zero
                //     ))
                //     .ToList();

                // var creditList = journalEntryDetails
                //     .Where(c => c.CreditAmount > 0)
                //     .GroupBy(c => new { c.JournalEntryId, c.LedgerId })
                //     .Select(g => new LedgerSummaryResponse(
                //         g.Key.JournalEntryId,
                //         g.Key.LedgerId,
                //         "0.00",  // Only credit amount, debit is zero
                //         g.Sum(c => c.CreditAmount).ToString("0.00")
                //     ))
                //     .ToList();




                #endregion


                var (journalEntry, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                var filterJournalEntry = isSuperAdmin ? journalEntry : journalEntry.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");


                var schoolIds = await _unitOfWork.BaseRepository<School>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );


                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(schoolId))
                {
                    filterJournalEntry = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                        .FindBy(x => schoolIds.Contains(x.SchoolId));
                }


                var scopedJournalEntryIds = filterJournalEntry
                    .Where(x => x.LedgerId == ledgerId)
                    .Select(x => x.JournalEntryId)
                    .Distinct()
                    .ToList();


                var ledgerSummaryDetails = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetConditionalFilterType(
                        c => scopedJournalEntryIds.Contains(c.JournalEntryId),
                        query => query.AsNoTracking()
                            .GroupBy(c => new { c.JournalEntryId, c.LedgerId })
                            .Select(g => new LedgerSummaryResponse(
                                g.Key.JournalEntryId,
                                g.Key.LedgerId,
                                g.Select(c => c.DebitAmount == 0 ? (decimal?)null : c.DebitAmount).Sum() == 0
                                    ? (decimal?)null
                                    : g.Select(c => c.DebitAmount == 0 ? (decimal?)null : c.DebitAmount).Sum(), // Debit Amount as null if 0
                                g.Select(c => c.CreditAmount == 0 ? (decimal?)null : c.CreditAmount).Sum() == 0
                                    ? (decimal?)null
                                    : g.Select(c => c.CreditAmount == 0 ? (decimal?)null : c.CreditAmount).Sum()  // Credit Amount as null if 0
                            ))
                    );


                var totalItems = ledgerSummaryDetails.Count();

                var paginatedLedgerSummary = paginationRequest != null && paginationRequest.IsPagination
                    ? ledgerSummaryDetails
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : ledgerSummaryDetails.ToList();

                var pagedResult = new PagedResult<LedgerSummaryResponse>
                {
                    Items = paginatedLedgerSummary,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };


                return Result<PagedResult<LedgerSummaryResponse>>.Success(pagedResult);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching summary of ledger using {ledgerId}", ex.InnerException);
            }
        }
       
    }
}
