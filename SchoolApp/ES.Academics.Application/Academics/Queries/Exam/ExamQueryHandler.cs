using AutoMapper;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.Exam
{
    public class ExamQueryHandler : IRequestHandler<ExamQuery, Result<PagedResult<ExamQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IExamServices _examServices;

        public ExamQueryHandler(IMapper mapper, IExamServices examServices)
        {
            _mapper = mapper;
            _examServices = examServices;
        }


        public async Task<Result<PagedResult<ExamQueryResponse>>> Handle(ExamQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var exam = await _examServices.GetAllExam(request.PaginationRequest);
                var examResult = _mapper.Map<PagedResult<ExamQueryResponse>>(exam.Data);
                return Result< PagedResult<ExamQueryResponse>>.Success(examResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all exam", ex);
            }
        }
    }
}
