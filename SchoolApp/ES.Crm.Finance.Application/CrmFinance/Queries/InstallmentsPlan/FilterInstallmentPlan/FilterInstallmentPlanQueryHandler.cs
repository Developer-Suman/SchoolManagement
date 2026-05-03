using AutoMapper;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan
{
    public class FilterInstallmentPlanQueryHandler : IRequestHandler<FilterInstallmentPlanQuery, Result<PagedResult<FilterInstallmentPlanResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IInstallmentServices _installmentServices;


        public FilterInstallmentPlanQueryHandler(IMapper mapper, IInstallmentServices installmentServices)
        {
            _installmentServices = installmentServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterInstallmentPlanResponse>>> Handle(FilterInstallmentPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _installmentServices.GetFilterInstallmentPlan(request.PaginationRequest, request.FilterInstallmentPlanDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterInstallmentPlanResponse>>(result.Data);

                return Result<PagedResult<FilterInstallmentPlanResponse>>.Success(filterResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterInstallmentPlanResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
