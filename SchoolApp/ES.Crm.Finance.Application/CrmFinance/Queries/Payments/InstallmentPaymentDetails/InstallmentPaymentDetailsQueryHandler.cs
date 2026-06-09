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

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.InstallmentPaymentDetails
{
    public class InstallmentPaymentDetailsQueryHandler : IRequestHandler<InstallmentPaymentDetailsQuery, Result<PagedResult<InstallmentPaymentDetailsResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IInstallmentServices _installmentServices;

        public InstallmentPaymentDetailsQueryHandler(IInstallmentServices installmentServices, IMapper mapper)
        {
            _installmentServices = installmentServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<InstallmentPaymentDetailsResponse>>> Handle(InstallmentPaymentDetailsQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(InstallmentPaymentDetailsQuery).Name
                .Replace("Filter", "")
                .Replace("Query", "");

            try
            {
                var result = await _installmentServices.InstallmentPaymentDetails(request.PaginationRequest, request.InstallmentPaymentDetailsDTOs);

                return Result<PagedResult<InstallmentPaymentDetailsResponse>>.Success(result.Data, $"{entityName} returned successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<InstallmentPaymentDetailsResponse>>
                    .Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
