using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterApplicant
{
    public class FilterApplicantQueryHandler : IRequestHandler<FilterApplicantQuery, Result<PagedResult<FilterApplicantResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public FilterApplicantQueryHandler(IMapper mapper, IEnrolmentServices feeStructureServices)
        {
            _mapper = mapper;
            _enrolmentServices = feeStructureServices;
        }
        public async Task<Result<PagedResult<FilterApplicantResponse>>> Handle(FilterApplicantQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _enrolmentServices.FilterApplicant(request.PaginationRequest, request.FilterApplicantDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterApplicantResponse>>(result.Data);

                return Result<PagedResult<FilterApplicantResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterApplicantResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
