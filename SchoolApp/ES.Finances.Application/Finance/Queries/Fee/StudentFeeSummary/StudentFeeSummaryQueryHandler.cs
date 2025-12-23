using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary
{
    public class StudentFeeSummaryQueryHandler : IRequestHandler<StudentFeeSummaryQuery, Result<PagedResult<StudentFeeSummaryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentFeeServices _studentFeeServices;

        public StudentFeeSummaryQueryHandler(IMapper mapper, IStudentFeeServices studentFeeServices)
        {
            _mapper = mapper;
            _studentFeeServices = studentFeeServices;
        }
        public async Task<Result<PagedResult<StudentFeeSummaryResponse>>> Handle(StudentFeeSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _studentFeeServices.GetStudentFeeSummary(request.PaginationRequest, request.StudentFeeSummaryDTOs);

                var resultDisplay = _mapper.Map<PagedResult<StudentFeeSummaryResponse>>(result.Data);

                return Result<PagedResult<StudentFeeSummaryResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<StudentFeeSummaryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
