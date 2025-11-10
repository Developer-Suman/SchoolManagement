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

namespace ES.Academics.Application.Academics.Queries.Subject
{
    public class SubjectQueryHandler : IRequestHandler<SubjectQuery, Result<PagedResult<SubjectResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISubjectServices _subjectServices;

        public SubjectQueryHandler(IMapper mapper, ISubjectServices subjectServices)
        {
            _mapper = mapper;
            _subjectServices = subjectServices;
        }
        public async Task<Result<PagedResult<SubjectResponse>>> Handle(SubjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subject = await _subjectServices.GetAllSubject(request.PaginationRequest);
                var subjectResult = _mapper.Map<PagedResult<SubjectResponse>>(subject.Data);
                return Result<PagedResult<SubjectResponse>>.Success(subjectResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Subject", ex);
            }
        }
    }
}
