using AutoMapper;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.ExamResult
{
    public class ExamResultQueryHandler : IRequestHandler<ExamResultQuery, Result<PagedResult<ExamResultResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IExamResultServices _examResultServices;

        public ExamResultQueryHandler(IMapper mapper, IExamResultServices examResultServices)
        {
            _mapper = mapper;
            _examResultServices = examResultServices;
        }
        public async Task<Result<PagedResult<ExamResultResponse>>> Handle(ExamResultQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var examResult = await _examResultServices.GetAllExamResult(request.PaginationRequest);
                var examResultDisplay = _mapper.Map<PagedResult<ExamResultResponse>>(examResult.Data);
                return Result<PagedResult<ExamResultResponse>>.Success(examResultDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all exam result", ex);
            }
        }
    }
}
