using AutoMapper;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan
{
    public class InstallmentPlanQueryHandler : IRequestHandler<InstallmentPlanQuery, Result<InstallmentPlanResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IInstallmentServices _installmentServices;

        public InstallmentPlanQueryHandler(IInstallmentServices installmentServices, IMapper mapper)
        {
            _mapper = mapper;
            _installmentServices = installmentServices;

        }
        public async Task<Result<InstallmentPlanResponse>> Handle(InstallmentPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _installmentServices.GetInstallmentPlan(request.id);
                return Result<InstallmentPlanResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
