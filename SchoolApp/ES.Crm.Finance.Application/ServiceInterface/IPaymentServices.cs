using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.ServiceInterface
{
    public interface IPaymentServices
    {
        Task<Result<AddPaymentsResponse>> Addpayment(AddPaymentsCommand addPaymentsCommand);

        //Task<Result<InstallmentPlanResponse>> GetInstallmentPlan(string installmentPlanId, CancellationToken cancellationToken = default);
        //Task<Result<UpdateVisaApplicationResponse>> UpdateVisaApplication(string visaApplicationId, UpdateVisaApplicationCommand updateVisaApplicationCommand);
        //Task<Result<bool>> DeleteVisaApplication(string id, CancellationToken cancellationToken);

        //Task<Result<PagedResult<FilterInstallmentPlanResponse>>> GetFilterInstallmentPlan(PaginationRequest paginationRequest, FilterInstallmentPlanDTOs filterInstallmentPlanDTOs);
    }
}
