using AutoMapper;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.FilterExamResult
{
    public class FilterExamResultQueryHandler : IRequestHandler<FilterExamResultQuery, Result<PagedResult<FilterExamResultResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IExamResultServices _examResultServices;

        public FilterExamResultQueryHandler(IMapper mapper, IExamResultServices examResultServices)
        {
            _examResultServices = examResultServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterExamResultResponse>>> Handle(FilterExamResultQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _examResultServices.GetFilterExamResult(request.PaginationRequest, request.FilterExamResultDTOs);

                var examResult = _mapper.Map<PagedResult<FilterExamResultResponse>>(result.Data);

                return Result<PagedResult<FilterExamResultResponse>>.Success(examResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterExamResultResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
