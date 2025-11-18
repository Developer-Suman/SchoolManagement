using AutoMapper;
using ES.Academics.Application.Academics.Queries.Subject;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.SubjectByClassId
{
    public class SubjectByClassIdQueryHandler : IRequestHandler<SubjectByClassIdQuery, Result<List<SubjectByClassIdResponse>>>
    {
        private readonly IExamResultServices _examResultServices;
        private readonly IMapper _mapper;


        public SubjectByClassIdQueryHandler(IExamResultServices examResultServices, IMapper mapper)
        {
            _examResultServices = examResultServices;
            _mapper = mapper;

        }
        public async Task<Result<List<SubjectByClassIdResponse>>> Handle(SubjectByClassIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subject = await _examResultServices.GetSubjectByClass(request.classId);
                var subjectResult = _mapper.Map<List<SubjectByClassIdResponse>>(subject.Data);
                return Result<List<SubjectByClassIdResponse>>.Success(subjectResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Subject", ex);
            }
        }
    }
}
