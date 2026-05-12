using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructure;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.DueSlip
{
    public class DueSlipQueryHandler : IRequestHandler<DueSlipQuery, Result<PagedResult<DueSlipResponse>>>
    {
        private readonly IStudentFeeServices _studentFeeServices;
        private readonly IMapper _mapper;

        public DueSlipQueryHandler(IStudentFeeServices studentFeeServices, IMapper mapper)
        {
            _mapper = mapper;
            _studentFeeServices = studentFeeServices;
        }
        public async Task<Result<PagedResult<DueSlipResponse>>> Handle(DueSlipQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var results = await _studentFeeServices.GetDueSlip(request.PaginationRequest, request.DueSlipDTOs);
                var resultsDisplay = _mapper.Map<PagedResult<DueSlipResponse>>(results.Data);
                return Result<PagedResult<DueSlipResponse>>.Success(resultsDisplay);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
