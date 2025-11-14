using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.FilterParents
{
    public class FilterParentsQueryHandler : IRequestHandler<FilterParentsQuery, Result<PagedResult<FilterParentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentServices _studentServices;

        public FilterParentsQueryHandler(IMapper mapper, IStudentServices studentServices)
        {
            _mapper = mapper;
            _studentServices = studentServices;
            
        }
        public async Task<Result<PagedResult<FilterParentsResponse>>> Handle(FilterParentsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _studentServices.GetFilterParents(request.PaginationRequest, request.FilterParentsDTOs);

                var parentsFilter = _mapper.Map<PagedResult<FilterParentsResponse>>(result.Data);

                return Result<PagedResult<FilterParentsResponse>>.Success(parentsFilter);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterParentsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
