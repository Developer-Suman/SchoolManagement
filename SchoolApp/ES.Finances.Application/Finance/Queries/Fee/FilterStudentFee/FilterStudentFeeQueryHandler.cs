using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee
{
    public class FilterStudentFeeQueryHandler : IRequestHandler<FilterStudentFeeQuery, Result<PagedResult<FilterStudentFeeResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentFeeServices _studentFeeServices;

        public FilterStudentFeeQueryHandler(IMapper mapper, IStudentFeeServices studentFeeServices)
        {
            _mapper = mapper;
            _studentFeeServices = studentFeeServices;


        }
        public async Task<Result<PagedResult<FilterStudentFeeResponse>>> Handle(FilterStudentFeeQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _studentFeeServices.Filter(request.PaginationRequest, request.FilterStudentFeeDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterStudentFeeResponse>>(result.Data);

                return Result<PagedResult<FilterStudentFeeResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterStudentFeeResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
