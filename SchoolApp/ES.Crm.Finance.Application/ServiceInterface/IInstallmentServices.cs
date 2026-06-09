using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.InstallmentPaymentDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.ServiceInterface
{
    public interface IInstallmentServices
    {
        Task<Result<AddInstallmentsPlanResponse>> Add(AddInstallmentsPlanCommand addInstallmentsPlanCommand);
      
        Task<Result<InstallmentPlanResponse>> Get(string installmentPlanId, CancellationToken cancellationToken = default);
        Task<Result<UpdateInstallmentsPlanResponse>> Update(string installmentPlanId, UpdateInstallmentsPlanCommand updateInstallmentsPlanCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
       
        Task<Result<PagedResult<FilterInstallmentPlanResponse>>> Filter(PaginationRequest paginationRequest, FilterInstallmentPlanDTOs filterInstallmentPlanDTOs);
        Task<Result<PagedResult<InstallmentPaymentDetailsResponse>>> InstallmentPaymentDetails(PaginationRequest paginationRequest, InstallmentPaymentDetailsDTOs installmentPaymentDetailsDTOs);
    }
}
