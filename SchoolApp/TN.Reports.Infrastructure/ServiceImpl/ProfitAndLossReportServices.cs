using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.Profit_LossReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class ProfitAndLossReportServices : IProfitAndLossServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGetUserScopedData _getUserScopedData;

        public ProfitAndLossReportServices(IUnitOfWork unitOfWork,IMapper mapper,IGetUserScopedData getUserScopedData)
        {
            _unitOfWork=unitOfWork;
            _mapper = mapper;
            _getUserScopedData = getUserScopedData;
        }

        public async Task<Result<PagedResult<ProfitAndLossFinalResponse>>> GetProfitLossReport(PaginationRequest PaginationRequest, string? SchoolId,CancellationToken cancellationToken=default)
        {
            try
            {
                // 1. Get user scoped data
                var (journalEntry, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<JournalEntryDetails>();

                // 2. Filter by company
                IQueryable<JournalEntryDetails> filterJournalEntry;

                if (!string.IsNullOrEmpty(SchoolId))
                    filterJournalEntry = journalEntry.Where(x => x.SchoolId == SchoolId);
                else if (!string.IsNullOrEmpty(currentSchoolId) && !isSuperAdmin)
                    filterJournalEntry = journalEntry.Where(x => x.SchoolId == currentSchoolId);
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var schoolIds = await _unitOfWork.BaseRepository<School>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filterJournalEntry = journalEntry.Where(x => schoolIds.Contains(x.SchoolId));
                }
                else
                    filterJournalEntry = journalEntry;

                // 3. Project only necessary fields for P&L
                var entries = await filterJournalEntry
                    .Where(e => e.Ledger != null &&
                                (e.Ledger.SubLedgerGroup.LedgerGroup.MasterId == MasterConstraints.Income ||
                                 e.Ledger.SubLedgerGroup.LedgerGroup.MasterId == MasterConstraints.Expenses ||
                                 e.Ledger.SubLedgerGroup.Id == LedgerGroupConstrants.DutiesAndTaxesLedgerId))

                    .Select(e => new
                    {
                        LedgerId = e.Ledger.Id,
                        LedgerName = e.Ledger.Name,
                        SubLedgerGroupId = e.Ledger.SubLedgerGroup.Id,
                        SubLedgerGroupName = e.Ledger.SubLedgerGroup.Name,
                        LedgerGroupId = e.Ledger.SubLedgerGroup.LedgerGroup.Id,
                        LedgerGroupName = e.Ledger.SubLedgerGroup.LedgerGroup.Name,
                        SectionName = e.Ledger.SubLedgerGroup.LedgerGroup.MasterId == MasterConstraints.Income ? "Income" : "Expenses",
                        Balance = e.DebitAmount - e.CreditAmount
                    })
                    .ToListAsync(cancellationToken);

                // 4. Map to DTOs
                var profitLossDetails = entries.Select(e =>
                {
                    decimal balance = Math.Abs(e.Balance);
                    string balanceType = e.Balance >= 0 ? "Dr" : "Cr";

                    return new ProfitAndLossDetails(
                        e.SectionName,
                        e.LedgerGroupId,
                        e.LedgerGroupName,
                        e.SubLedgerGroupId,
                        e.SubLedgerGroupName,
                        e.LedgerId,
                        e.LedgerName,
                        balance,
                        balanceType
                    );
                }).ToList();

                // 5. Group hierarchy
                var masterGroups = profitLossDetails
                    .GroupBy(x => new { x.SectionName, x.LedgerGroupId, x.LedgerGroupName })
                    .Select(master => new MasterLevelProfitAndLoss(
                        master.Key.SectionName,
                        master.Key.LedgerGroupId,
                        master.Key.LedgerGroupName,
                        master.Sum(x => x.Balance),
                        master.GroupBy(x => new { x.SubLedgerGroupId, x.SubLedgerGroupName })
                            .Select(subGroup => new LedgerGroupLevelProfitAndLoss(
                                master.Key.LedgerGroupId,
                                master.Key.LedgerGroupName,
                                subGroup.Sum(x => x.Balance),
                                subGroup.Select(ledger => new SubLedgerGroupLevelProfitAndLoss(
                                    ledger.SubLedgerGroupId,
                                    ledger.SubLedgerGroupName,
                                    ledger.Balance,
                                    new List<LedgerLevelProfitAndLoss>
                                    {
                                new LedgerLevelProfitAndLoss(
                                    ledger.LedgerId,
                                    ledger.LedgerName,
                                    ledger.Balance,
                                    ledger.BalanceType
                                )
                                    }
                                )).ToList()
                            )).ToList()
                    )).ToList();

              
                var incomeGroups = masterGroups.Where(m => m.SectionName == "Income").ToList();
                var expenseGroups = masterGroups.Where(m => m.SectionName == "Expenses").ToList();

                decimal totalIncome = incomeGroups.Sum(m => m.Total);
                decimal totalExpenses = expenseGroups.Sum(m => m.Total);
                decimal grossProfitOrLoss = totalIncome - totalExpenses;
                decimal netProfitOrLoss = grossProfitOrLoss;


                // Duties & Taxes Calculation
                var dutiesAndTaxes = profitLossDetails
                    .Where(x => x.SubLedgerGroupId == LedgerGroupConstrants.DutiesAndTaxesLedgerId)
                    .Select(x => new DutiesAndTaxesDto(x.LedgerName, x.Balance,x.BalanceType))
                    .ToList();

                
                decimal totalDutiesAndTaxes = dutiesAndTaxes.Sum(d => d.Amount);
                decimal netProfitAfterTax = netProfitOrLoss - totalDutiesAndTaxes;

               
                var finalResponse = new ProfitAndLossFinalResponse(
                    incomeGroups,
                    expenseGroups,
                    grossProfitOrLoss,
                    netProfitOrLoss,
                    dutiesAndTaxes,          
                    totalDutiesAndTaxes,
                    netProfitAfterTax
                );


                
                PagedResult<ProfitAndLossFinalResponse> paginatedResult;
                var items = new List<ProfitAndLossFinalResponse> { finalResponse };

                if (PaginationRequest.IsPagination)
                {
                    int pageIndex = PaginationRequest.pageIndex <= 0 ? 1 : PaginationRequest.pageIndex;
                    int pageSize = PaginationRequest.pageSize <= 0 ? 10 : PaginationRequest.pageSize;

                    paginatedResult = new PagedResult<ProfitAndLossFinalResponse>
                    {
                        Items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                        TotalItems = items.Count,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    paginatedResult = new PagedResult<ProfitAndLossFinalResponse>
                    {
                        Items = items,
                        TotalItems = 1,
                        PageIndex = 1,
                        pageSize = 1
                    };
                }

                return Result<PagedResult<ProfitAndLossFinalResponse>>.Success(paginatedResult);
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine(ex);
                throw new Exception("An error occurred while generating Profit and Loss Report", ex);
            }

        }
    }
}
