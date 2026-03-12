using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.GetAllStudents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.StudentFromRegistration
{
    public class StudentFromRegistrationQueryHandler : IRequestHandler<StudentFromRegistrationQuery, Result<PagedResult<StudentFromRegistrationResponse>>>
    {
        private readonly IStudentServices _studnetServices;
        private readonly IMapper _mapper;


        public StudentFromRegistrationQueryHandler(IStudentServices studentServices, IMapper mapper)
        {
            _studnetServices = studentServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<StudentFromRegistrationResponse>>> Handle(StudentFromRegistrationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var all = await _studnetServices.StudentFromRegistration(request.PaginationRequest, cancellationToken);
                var allDisplay = _mapper.Map<PagedResult<StudentFromRegistrationResponse>>(all.Data);
                return Result<PagedResult<StudentFromRegistrationResponse>>.Success(allDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
