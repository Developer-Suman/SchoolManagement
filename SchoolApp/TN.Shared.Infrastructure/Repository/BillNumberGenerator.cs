
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Purchase;
using TN.Shared.Domain.Entities.Sales;
using TN.Shared.Domain.Entities.Transactions;
using TN.Shared.Domain.IRepository;
using static TN.Authentication.Domain.Entities.School;
using static TN.Authentication.Domain.Entities.SchoolSettings;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Shared.Infrastructure.Repository
{
    public class BillNumberGenerator : IBillNumberGenerator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateConvertHelper _dateConvertHelper;
        //private readonly ISettingServices _settingServices;

        public BillNumberGenerator(IUnitOfWork unitOfWork, IDateConvertHelper dateConvertHelper)
        {

            _dateConvertHelper = dateConvertHelper;

            _unitOfWork = unitOfWork;
            
        }
        public async Task<string> GenerateBillNumberAsync( string schoolId, string billType, string fyName)
        {
            try
            {
                var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
                string schoolShortName = school.Name.Substring(0, 3).ToUpper();

                var nepaliDate = await _dateConvertHelper.ConvertToNepali(DateTime.Now);
                //string[] nepaliNumbers = { "०", "१", "२", "३", "४", "५", "६", "७", "८", "९" };
                //for (int i = 0; i < nepaliNumbers.Length; i++)
                //{
                //    nepaliDate = nepaliDate.Replace(nepaliNumbers[i], i.ToString());
                //}

                var parts = nepaliDate.Split('-');
                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                // Map month number to Roman Nepali month
                string[] nepaliMonthsRoman =
                                {
                    "Baisakh", "Jestha", "Ashadh", "Shrawan", "Bhadra", "Ashwin",
                    "Kartik", "Mangsir", "Poush", "Magh", "Falgun", "Chaitra"
                };

                string nepaliMonthRoman = nepaliMonthsRoman[month - 1];




                string prefix = $"{schoolShortName}-{billType.Substring(0, 3).ToUpper()}-{nepaliMonthRoman}/{fyName}";

                // 1. Check Bill Number Type
                if (billType.Equals("purchase", StringComparison.OrdinalIgnoreCase))
                {
                    if (school.BillNumberGenerationTypeForPurchase == BillNumberGenerationType.Manual)
                        throw new Exception("Bill number should be entered manually.");
                }
                else
                {
                    if (school.BillNumberGenerationTypeForSales == BillNumberGenerationType.Manual)
                        throw new Exception("Bill number should be entered manually.");
                }

                // 2. Start from the last count in BOTH tables combined
                int purchaseCount = await _unitOfWork.BaseRepository<PurchaseDetails>()
                    .CountAsync(p => p.SchoolId == schoolId);
                int salesCount = await _unitOfWork.BaseRepository<SalesDetails>()
                    .CountAsync(s => s.SchoolId == schoolId);

                int nextNumber = Math.Max(purchaseCount, salesCount) + 1;

                // 3. Generate and ensure uniqueness across both tables
                string billNumber;
                bool exists;

                do
                {
                    billNumber = $"{prefix}-{nextNumber:D4}";

                    bool existsInPurchase = await _unitOfWork.BaseRepository<PurchaseDetails>()
                        .AnyAsync(p => p.BillNumber == billNumber && p.SchoolId == schoolId);

                    bool existsInSales = await _unitOfWork.BaseRepository<SalesDetails>()
                        .AnyAsync(s => s.BillNumber == billNumber && s.SchoolId == schoolId);

                    exists = existsInPurchase || existsInSales;

                    if (exists)
                        nextNumber++;

                } while (exists);

                return billNumber;
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while creating Bill Number for companyId {schoolId}");
            }
        }

        public async Task<string> GenerateJournalReference(string schoolId)
        {
            try
            {
                var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().FirstOrDefaultAsync(x=>x.SchoolId == schoolId);

                var company = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);

                string schoolShortName = company.Name.Substring(0, 3).ToUpper();
                string yearMonth = DateTime.UtcNow.ToString("yyyyMM");
                string prefix = $"{schoolShortName}/{yearMonth}";

                if (schoolSettings.JournalReference == SchoolSettings.JournalReferencesType.Manual)
                {
                    throw new Exception("Journal references should be entered manually.");
                }

                if (schoolSettings.JournalReference == SchoolSettings.JournalReferencesType.Automatic)
                {
                    int lastBillCount = await _unitOfWork.BaseRepository<JournalEntry>().CountAsync(p => p.SchoolId == schoolId);
                    return $"{prefix}-{(lastBillCount + 1):D4}";
                }

                return string.Empty;

            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while Generating Journal References", ex);
            }
        }

        public async Task<string> GenerateReferenceNumber(string schoolId,  ReferenceType type)
        {
            try
            {
                var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().FirstOrDefaultAsync(x => x.SchoolId == schoolId);

                var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);

                string schoolShortName = school.Name.Substring(0, 3).ToUpper();
                string yearMonth = DateTime.UtcNow.ToString("yyyyMM");

                string typePrefix = type switch
                {
                    ReferenceType.Purchase => "PURCHASE",
                    ReferenceType.Sales => "SALES",
                    _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid reference type.")
                };

                string prefix = $"{typePrefix}/{schoolShortName}/{yearMonth}";


                int lastBillCount = type switch
                {
                    ReferenceType.Purchase => await _unitOfWork.BaseRepository<PurchaseDetails>()
                        .CountAsync(p => p.SchoolId == schoolId && type == ReferenceType.Purchase),

                    ReferenceType.Sales => await _unitOfWork.BaseRepository<SalesDetails>()
                        .CountAsync(p => p.SchoolId == schoolId && type == ReferenceType.Sales),

                    _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid reference type.")
                };

                return $"{prefix}-{(lastBillCount + 1):D4}";

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Generating Journal References", ex);
            }
        }

    

        public async Task<string> GenerateTransactionNumber(string schoolId, string transactionNumberType, string fyName)
        {

            try
            {
                var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().FirstOrDefaultAsync(x => x.SchoolId == schoolId);
                var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
                string schoolShortName = school.Name.Substring(0, 3).ToUpper();
                string transactionNumberTypes = transactionNumberType.ToUpper();

                var nepaliDate = await _dateConvertHelper.ConvertToNepali(DateTime.Now);
                //string[] nepaliNumbers = { "०", "१", "२", "३", "४", "५", "६", "७", "८", "९" };
                //for (int i = 0; i < nepaliNumbers.Length; i++)
                //{
                //    nepaliDate = nepaliDate.Replace(nepaliNumbers[i], i.ToString());
                //}

                var parts = nepaliDate.Split('-');
                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                // Map month number to Roman Nepali month
                string[] nepaliMonthsRoman =
                                {
                    "Baisakh", "Jestha", "Ashadh", "Shrawan", "Bhadra", "Ashwin",
                    "Kartik", "Mangsir", "Poush", "Magh", "Falgun", "Chaitra"
                };

                string nepaliMonthRoman = nepaliMonthsRoman[month - 1];


                string prefix = $"{schoolShortName}-{transactionNumberTypes}-{(nepaliMonthRoman).Substring(0, 5).ToUpper()}/{fyName}";

                switch (transactionNumberType.ToLower())
                {

                    case "income":
                        if (schoolSettings.IncomeTransactionNumberType == TransactionNumberType.Manual)
                        {
                            throw new Exception("Income number should be entered manually.");
                        }

                        if (schoolSettings.IncomeTransactionNumberType == TransactionNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<TransactionDetail>()
                                .CountAsync(p => p.SchoolId == schoolId && p.TransactionMode == TransactionType.Income && p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;

                    case "receipt":
                        if (schoolSettings.ReceiptTransactionNumberType == TransactionNumberType.Manual)
                        {
                            throw new Exception("Receipt number should be entered manually.");
                        }

                        if (schoolSettings.ReceiptTransactionNumberType == TransactionNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<TransactionDetail>()
                                .CountAsync(p => p.SchoolId == schoolId && p.TransactionMode == TransactionType.Receipts && p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;

                    case "payment":
                        if (schoolSettings.PaymentTransactionNumberType == TransactionNumberType.Manual)
                        {
                            throw new Exception("Payment number should be entered manually.");
                        }

                        if (schoolSettings.PaymentTransactionNumberType == TransactionNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<TransactionDetail>()
                                .CountAsync(p => p.SchoolId == schoolId && p.TransactionMode == TransactionType.Payment && p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;

                    case "expenses":
                        if (schoolSettings.ExpensesTransactionNumberType == TransactionNumberType.Manual)
                        {
                            throw new Exception("Expenses number should be entered manually.");
                        }

                        if (schoolSettings.ExpensesTransactionNumberType == TransactionNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<TransactionDetail>()
                                .CountAsync(p => p.SchoolId == schoolId && p.TransactionMode == TransactionType.Expense && p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;

                    case "purchasereturn":
                        if (schoolSettings.PurchaseReturnNumberType == PurchaseSalesReturnNumberType.Manual)
                        {
                            throw new Exception("PurchaseReturn number should be entered manually.");
                        }

                        if (schoolSettings.PurchaseReturnNumberType == PurchaseSalesReturnNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<PurchaseReturnDetails>()
                                .CountAsync(p => p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;


                    case "salesreturn":
                        if (schoolSettings.SalesReturnNumberType == PurchaseSalesReturnNumberType.Manual)
                        {
                            throw new Exception("SalesReturn number should be entered manually.");
                        }

                        if (schoolSettings.SalesReturnNumberType == PurchaseSalesReturnNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<SalesReturnDetails>()
                                .CountAsync(p => p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;

                    case "purchasequotation":
                        if (schoolSettings.PurchaseQuotationNumberType == PurchaseSalesQuotationNumberType.Manual)
                        {
                            throw new Exception("purchasequotation number should be entered manually.");
                        }

                        if (schoolSettings.PurchaseQuotationNumberType == PurchaseSalesQuotationNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<PurchaseQuotationDetails>()
                                .CountAsync(p => p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;

                    case "salesquotation":
                        if (schoolSettings.SalesQuotationNumberType == PurchaseSalesQuotationNumberType.Manual)
                        {
                            throw new Exception("salesQuotation number should be entered manually.");
                        }

                        if (schoolSettings.SalesQuotationNumberType == PurchaseSalesQuotationNumberType.Automatic)
                        {
                            int lastBillCount = await _unitOfWork.BaseRepository<SalesQuotationDetails>()
                                .CountAsync(p => p.SchoolId == schoolId);

                            return $"{prefix}-{(lastBillCount + 1):D4}";
                        }
                        break;



                    default:
                        throw new Exception("Invalid transaction type.");
                }


                return string.Empty;

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while creating Bill Number by schoolId {schoolId}");
            }



        }
    }
}
