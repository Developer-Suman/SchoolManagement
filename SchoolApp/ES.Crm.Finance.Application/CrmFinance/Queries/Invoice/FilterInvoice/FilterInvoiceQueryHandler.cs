using AutoMapper;
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

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInvoice
{
    public class FilterInvoiceQueryHandler : IRequestHandler<FilterInvoiceQuery, Result<PagedResult<FilterInvoiceResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceServices _invoiceServices;

        public FilterInvoiceQueryHandler(IMapper mapper, IInvoiceServices invoiceServices)
        {
            _invoiceServices = invoiceServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterInvoiceResponse>>> Handle(FilterInvoiceQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterInvoiceQuery).Name
                  .Replace("Filter", "")
                  .Replace("Query", "");
            try
            {

                var result = await _invoiceServices.Filter(request.PaginationRequest, request.FilterInvoiceDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterInvoiceResponse>>(result.Data);

                return Result<PagedResult<FilterInvoiceResponse>>.Success(filterResult, $"{entityName} return successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterInvoiceResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
