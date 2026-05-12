using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterPayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.PaymentsId;
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

        Task<Result<PaymentsIdResponse>> Get(string paymentsId, CancellationToken cancellationToken = default);
        Task<Result<UpdatePaymentsResponse>> Update(string paymentsId, UpdatePaymentsCommand updatePaymentsCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);

        Task<Result<PagedResult<FilterPaymentsResponse>>> Filter(PaginationRequest paginationRequest, FilterPaymentsDTOs filterPaymentsDTOs);
    }
}
