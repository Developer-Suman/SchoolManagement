using AutoMapper;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.GetAssignments
{
    public class GetAssignmentsQueryHandler : IRequestHandler<GetAssignmentsQuery, Result<PagedResult<GetAssignmentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentServices _assignmentServices;

        public GetAssignmentsQueryHandler(IMapper mapper, ISubjectServices subjectServices, IAssignmentServices assignmentServices)
        {

            _mapper = mapper;
            _assignmentServices = assignmentServices;
        }
        public async Task<Result<PagedResult<GetAssignmentsResponse>>> Handle(GetAssignmentsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _assignmentServices.GetAssignments(request.PaginationRequest, request.GetAssignmentsDTOs);

                var getAssignment = _mapper.Map<PagedResult<GetAssignmentsResponse>>(result.Data);

                return Result<PagedResult<GetAssignmentsResponse>>.Success(getAssignment);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAssignmentsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
