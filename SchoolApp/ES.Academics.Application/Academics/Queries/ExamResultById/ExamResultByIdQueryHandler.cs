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

namespace ES.Academics.Application.Academics.Queries.ExamResultById
{
    public class ExamResultByIdQueryHandler : IRequestHandler<ExamResultByIdQuery, Result<ExamResultByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IExamResultServices _examResultServices;

        public ExamResultByIdQueryHandler(IMapper mapper, IExamResultServices examResultServices)
        {
            _mapper = mapper;
            _examResultServices = examResultServices;
        }
        public async Task<Result<ExamResultByIdResponse>> Handle(ExamResultByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var examResultById = await _examResultServices.GetExamResult(request.id);
                return Result<ExamResultByIdResponse>.Success(examResultById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Class by using id", ex);
            }
        }
    }
}
