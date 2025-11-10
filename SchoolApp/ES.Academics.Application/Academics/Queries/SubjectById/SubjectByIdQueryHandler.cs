using AutoMapper;
using ES.Academics.Application.Academics.Queries.ExamResultById;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Queries.SubjectById
{
    public class SubjectByIdQueryHandler : IRequestHandler<SubjectByIdQuery, Result<SubjectByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISubjectServices _subjectServices;

        public SubjectByIdQueryHandler(IMapper mapper, ISubjectServices subjectServices)
        {
            _mapper = mapper;
            _subjectServices = subjectServices;
        }
        public async Task<Result<SubjectByIdResponse>> Handle(SubjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var subjectResultById = await _subjectServices.GetSubject(request.id);
                return Result<SubjectByIdResponse>.Success(subjectResultById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Subject by using id", ex);
            }
        }
    }
}
