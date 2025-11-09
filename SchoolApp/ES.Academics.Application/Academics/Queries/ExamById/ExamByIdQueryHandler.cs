using AutoMapper;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Queries.ExamById
{
    public class ExamByIdQueryHandler : IRequestHandler<ExamByIdQuery, Result<ExamByIdQueryResponse>>
    {
        private readonly IExamServices _examServices;
        private readonly IMapper _mapper;

        public ExamByIdQueryHandler(IExamServices examServices, IMapper mapper)
        {
            _examServices = examServices;
            _mapper = mapper;

        }
        public async Task<Result<ExamByIdQueryResponse>> Handle(ExamByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var examById = await _examServices.GetExam(request.id);
                return Result<ExamByIdQueryResponse>.Success(examById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Exam by using id", ex);
            }
        }
    }
}
