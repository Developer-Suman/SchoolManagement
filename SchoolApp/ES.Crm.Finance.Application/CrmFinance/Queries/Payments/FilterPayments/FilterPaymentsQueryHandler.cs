using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterPayments
{
    public class FilterPaymentsQueryHandler : IRequestHandler<FilterPaymentsQuery, Result<PagedResult<FilterPaymentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;


        public FilterPaymentsQueryHandler(IMapper mapper, IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterPaymentsResponse>>> Handle(FilterPaymentsQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterPaymentsQuery).Name
                   .Replace("Filter", "")
                   .Replace("Query", "");

            try
            {

                var result = await _paymentServices.Filter(request.PaginationRequest, request.FilterPaymentsDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterPaymentsResponse>>(result.Data);

                return Result<PagedResult<FilterPaymentsResponse>>.Success(filterResult, $"{entityName} return successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterPaymentsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
