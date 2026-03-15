using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.Applicant
{
    public class ApplicantQueryHandler : IRequestHandler<ApplicantQuery, Result<PagedResult<ApplicantResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public ApplicantQueryHandler(IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;

        }
        public async Task<Result<PagedResult<ApplicantResponse>>> Handle(ApplicantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _enrolmentServices.GetAllApplicant(request.PaginationRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
