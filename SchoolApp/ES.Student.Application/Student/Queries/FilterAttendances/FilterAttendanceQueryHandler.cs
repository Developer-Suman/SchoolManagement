using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.FilterParents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.FilterAttendances
{
    public class FilterAttendanceQueryHandler : IRequestHandler<FilterAttendanceQuery, Result<PagedResult<FilterAttendanceResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IAttendanceServices _attendanceServices;

        public FilterAttendanceQueryHandler(IAttendanceServices attendanceServices, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceServices = attendanceServices;


        }
        public async Task<Result<PagedResult<FilterAttendanceResponse>>> Handle(FilterAttendanceQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _attendanceServices.GetFilterStudentAttendance(request.PaginationRequest, request.FilterAttendanceDTOs);

                var attendanceFilter = _mapper.Map<PagedResult<FilterAttendanceResponse>>(result.Data);

                return Result<PagedResult<FilterAttendanceResponse>>.Success(attendanceFilter);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterAttendanceResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
