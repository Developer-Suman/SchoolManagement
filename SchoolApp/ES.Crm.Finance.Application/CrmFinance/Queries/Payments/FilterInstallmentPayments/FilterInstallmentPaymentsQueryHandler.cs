using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterPayments;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterInstallmentPayments
{
    public class FilterInstallmentPaymentsQueryHandler : IRequestHandler<FilterInstallmentPaymentsQuery, Result<PagedResult<FilterInstallmentPaymentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;

        public FilterInstallmentPaymentsQueryHandler(IMapper mapper, IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterInstallmentPaymentsResponse>>> Handle(FilterInstallmentPaymentsQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterInstallmentPaymentsQuery).Name
                   .Replace("Filter", "")
                   .Replace("Query", "");

            try
            {

                var result = await _paymentServices.FilterInstallmentPayment(request.PaginationRequest, request.FilterInstallmentPaymentsDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterInstallmentPaymentsResponse>>(result.Data);

                return Result<PagedResult<FilterInstallmentPaymentsResponse>>.Success(filterResult, $"{entityName} return successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterInstallmentPaymentsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
