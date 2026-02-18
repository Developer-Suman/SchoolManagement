using AutoMapper;
using ES.Academics.Application.Academics.Command.ClosedAcademicYear;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.ServiceInterface;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.IRepository;
using ZXing;
using static System.Formats.Asn1.AsnWriter;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class StudentsPromotionServices : IStudentsPromotion
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;

        public StudentsPromotionServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _helperMethodServices = helperMethodServices;
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<ClosedAcademicYearResponse>> CloseAcademicYear(ClosedAcademicYearCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                try
                {
                    var schoolId = _tokenService.SchoolId().FirstOrDefault();

                    if (string.IsNullOrEmpty(schoolId))
                        throw new Exception("SchoolId not found in token.");

                    #region Clone School Settings for New Academic Year

                    var existingSettings = await _unitOfWork
                        .BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId);

                    if (existingSettings == null)
                        throw new Exception("School settings not found.");

                    var currentSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                .GetSingleAsync(x => x.SchoolId == schoolId && x.IsActive == true);

                    if (currentSettings == null)
                        throw new Exception("Active school settings not found.");

                    var nextYearSettings = currentSettings.CreateForNewYear(command.closedAcademicId);

                    currentSettings.Deactivate();


                    _unitOfWork.BaseRepository<SchoolSettings>().Update(currentSettings);
                    await _unitOfWork.BaseRepository<SchoolSettings>().AddAsync(nextYearSettings);

                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();


                    var resultDisplay = new ClosedAcademicYearResponse(nextYearSettings.Id);
                    return Result<ClosedAcademicYearResponse>.Success(resultDisplay);


                    //var newSchoolSettings = new SchoolSettings(
                    //    Guid.NewGuid().ToString(),
                    //    existingSettings.ShowTaxInSales,
                    //    existingSettings.ShowTaxInPurchase,
                    //    existingSettings.PurchaseReferences,
                    //    existingSettings.ShowReferenceNumberForSales,
                    //    existingSettings.ShowExpiredDateInItem,
                    //    existingSettings.ShowSerialNumberInItem,
                    //    existingSettings.ShowSerialNumberForPurchase,
                    //    existingSettings.ShowSerialNumberForSales,
                    //    existingSettings.JournalReference,
                    //    existingSettings.InventoryMethod,
                    //    existingSettings.SchoolId,
                    //    existingSettings.CurrentFiscalYearId,
                    //    existingSettings.AllowBackDatedEntry,
                    //    existingSettings.ReceiptTransactionNumberType,
                    //    existingSettings.PaymentTransactionNumberType,
                    //    existingSettings.IncomeTransactionNumberType,
                    //    existingSettings.ExpensesTransactionNumberType,
                    //    existingSettings.ShowReturnNumberForPurchase,
                    //    existingSettings.ShowReturnNumberForSales,
                    //    existingSettings.ShowQuotationNumberForPurchase,
                    //    existingSettings.ShowQuotationNumberForSales,
                    //    existingSettings.PurchaseReturnNumberType,
                    //    existingSettings.SalesReturnNumberType,
                    //    existingSettings.PurchaseQuotationNumberType,
                    //    existingSettings.SalesQuotationNumberType,
                    //    existingSettings.UserId,
                    //    command.closedAcademicId // New Academic Year Id
                    //);



                    //await _unitOfWork
                    //    .BaseRepository<SchoolSettings>()
                    //    .AddAsync(newSchoolSettings);

   

                    #endregion
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while cloning school settings for the new academic year.", ex);
                }


            }


        }

        public async Task<string?> GetCurrentAcademicYear()
        {
            try
            {
                var schoolId = _tokenService.SchoolId().FirstOrDefault();

                var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x=>x.SchoolId == schoolId 
                && x.IsActive == true);
                return schoolSettings.AcademicYearId;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting CurrentAcademicYear.", ex);
            }
        }
    }
}
