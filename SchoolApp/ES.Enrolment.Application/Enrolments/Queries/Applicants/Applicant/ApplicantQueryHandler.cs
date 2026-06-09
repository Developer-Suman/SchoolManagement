using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.FilterApplicant;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.Applicants.Applicant
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
            var entityName = typeof(ApplicantQuery).Name
           .Replace("Filter", "")
           .Replace("Query", "");


            try
            {
                var result = await _enrolmentServices.GetAllApplicant(request.PaginationRequest, cancellationToken);
                var resultDisplay = _mapper.Map<PagedResult<ApplicantResponse>>(result.Data);
                return Result<PagedResult<ApplicantResponse>>.Success(resultDisplay, $"{entityName} returned Successfully");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
