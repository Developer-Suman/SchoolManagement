using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.InqueryById;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.ApplicantsById
{
    public class ApplicantsByIdQueryHandler : IRequestHandler<ApplicantsByIdQuery, Result<ApplicantsByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public ApplicantsByIdQueryHandler(IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;

        }
        public async Task<Result<ApplicantsByIdResponse>> Handle(ApplicantsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var applicants = await _enrolmentServices.GetApplicants(request.id);
                return Result<ApplicantsByIdResponse>.Success(applicants.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Applicant using id", ex);
            }
        }
    }
}
