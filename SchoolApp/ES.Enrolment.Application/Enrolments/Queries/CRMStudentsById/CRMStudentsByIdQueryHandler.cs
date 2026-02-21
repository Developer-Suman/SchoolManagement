using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.ApplicantsById;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.CRMStudentsById
{
    public class CRMStudentsByIdQueryHandler : IRequestHandler<CRMStudentsByIdQuery, Result<CRMStudentsByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;
        public CRMStudentsByIdQueryHandler(IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;

        }
        public async Task<Result<CRMStudentsByIdResponse>> Handle(CRMStudentsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var crmStudents = await _enrolmentServices.GetCRMStudents(request.id);
                return Result<CRMStudentsByIdResponse>.Success(crmStudents.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching CRMStudents using id", ex);
            }
        }
    }
}
