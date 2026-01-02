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

namespace ES.Academics.Application.Academics.Queries.ClassWithSubject
{
    public class ClassWithSubjectQueryHandler : IRequestHandler<ClassWithSubjectQuery, Result<PagedResult<ClassWithSubjectResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolClassInterface _schoolClassInterface;

        public ClassWithSubjectQueryHandler(ISchoolClassInterface schoolClassInterface, IMapper mapper)
        {
            _schoolClassInterface = schoolClassInterface;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<ClassWithSubjectResponse>>> Handle(ClassWithSubjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var classWithSubject = await _schoolClassInterface.GetClassWithSubjects(request.paginationRequest);
                var classWithSubjectResult = _mapper.Map<PagedResult<ClassWithSubjectResponse>>(classWithSubject.Data);
                return Result<PagedResult<ClassWithSubjectResponse>>.Success(classWithSubjectResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching", ex);
            }
        }
    }
}
