using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.GetAllParent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.GetStudentForAttendance
{
    public class StudentForAttendanceQueryHandler : IRequestHandler<StudentForAttendanceQuery, Result<List<StudentForAttendanceResponse>>>
    {
        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;

        public StudentForAttendanceQueryHandler(IStudentServices studentServices, IMapper mapper)
        {
            _studentServices = studentServices;
            _mapper = mapper;

        }
        public async Task<Result<List<StudentForAttendanceResponse>>> Handle(StudentForAttendanceQuery request, CancellationToken cancellationToken)
        {

                var allStudentForAttendance = await _studentServices.GetStudentForAttendance();
                if (!allStudentForAttendance.IsSuccess)
                {
                    // Return failure with the errors from the service
                    return Result<List<StudentForAttendanceResponse>>.Failure(
                        allStudentForAttendance.Errors?.ToArray()
                    );
                }
                var allParentsDisplay = _mapper.Map<List<StudentForAttendanceResponse>>(allStudentForAttendance?.Data);
                return Result<List<StudentForAttendanceResponse>>.Success(allParentsDisplay);


        }
    }
}
