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

namespace ES.Student.Application.Student.Queries.FilterStudents
{
    public class FilterStudentsQueryHandler : IRequestHandler<FilterStudentsQuery, Result<PagedResult<FilterStudentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentServices _studentServices;

        public FilterStudentsQueryHandler(IMapper mapper, IStudentServices studentServices)
        {
           _mapper = mapper;
            _studentServices = studentServices;
            
        }
        public async Task<Result<PagedResult<FilterStudentsResponse>>> Handle(FilterStudentsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _studentServices.GetFilterStudent(request.PaginationRequest, request.FilterStudentsDTOs);

                var studentsFilter = _mapper.Map<PagedResult<FilterStudentsResponse>>(result.Data);

                return Result<PagedResult<FilterStudentsResponse>>.Success(studentsFilter);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterStudentsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
