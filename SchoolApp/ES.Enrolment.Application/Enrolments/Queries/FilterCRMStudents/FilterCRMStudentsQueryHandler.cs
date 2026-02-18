using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.FilterCRMStudents;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterCRMStudents
{
    public class FilterCRMStudentsQueryHandler : IRequestHandler<FilterCRMStudentsQuery, Result<PagedResult<FilterCRMStudentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;
        public FilterCRMStudentsQueryHandler(IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;
        }
        public async Task<Result<PagedResult<FilterCRMStudentsResponse>>> Handle(FilterCRMStudentsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _enrolmentServices.FilterCRMStudents(request.PaginationRequest, request.FilterCRMStudentsDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterCRMStudentsResponse>>(result.Data);

                return Result<PagedResult<FilterCRMStudentsResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterCRMStudentsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
