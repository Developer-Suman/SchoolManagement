using AutoMapper;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.FilterExam
{
    public class FilterExamQueryHandler : IRequestHandler<FilterExamQuery, Result<PagedResult<FilterExamResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IExamServices _examServices;

        public FilterExamQueryHandler(IMapper mapper, IExamServices examServices)
        {
            _mapper = mapper;
            _examServices = examServices;
        }
        public async Task<Result<PagedResult<FilterExamResponse>>> Handle(FilterExamQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _examServices.GetFilterExam(request.PaginationRequest, request.FilterExamDTOs);

                var examResult = _mapper.Map<PagedResult<FilterExamResponse>>(result.Data);

                return Result<PagedResult<FilterExamResponse>>.Success(examResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterExamResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
