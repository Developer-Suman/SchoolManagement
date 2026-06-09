using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInvoice;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInstallmentInvoice
{
    public class FilterInstallmentInvoiceQueryHandler : IRequestHandler<FilterInstallmentInvoiceQuery, Result<PagedResult<FilterInstallmentInvoiceResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceServices _invoiceServices;

        public FilterInstallmentInvoiceQueryHandler(IMapper mapper, IInvoiceServices invoiceServices)
        {
            _invoiceServices = invoiceServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterInstallmentInvoiceResponse>>> Handle(FilterInstallmentInvoiceQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterInstallmentInvoiceQuery).Name
                  .Replace("Filter", "")
                  .Replace("Query", "");
            try
            {

                var result = await _invoiceServices.FilterInstallmentInvoice(request.PaginationRequest, request.FilterInstallmentInvoiceDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterInstallmentInvoiceResponse>>(result.Data);

                return Result<PagedResult<FilterInstallmentInvoiceResponse>>.Success(filterResult, $"{entityName} return successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterInstallmentInvoiceResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
