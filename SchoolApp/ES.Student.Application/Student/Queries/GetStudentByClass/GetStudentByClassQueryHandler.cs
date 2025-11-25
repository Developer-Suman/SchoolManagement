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

namespace ES.Student.Application.Student.Queries.GetStudentByClass
{
    public class GetStudentByClassQueryHandler : IRequestHandler<GetStudentByClassQuery, Result<PagedResult<GetStudentByClassResponse>>>
    {
        private readonly IStudentServices _studnetServices;
        private readonly IMapper _mapper;

        public GetStudentByClassQueryHandler(IStudentServices studentServices, IMapper mapper)
        {
            _studnetServices = studentServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<GetStudentByClassResponse>>> Handle(GetStudentByClassQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allStudent = await _studnetServices.GetStudent(request.paginationRequest, request.classId);
                var allStudentDisplay = _mapper.Map<PagedResult<GetStudentByClassResponse>>(allStudent.Data);
                return Result<PagedResult<GetStudentByClassResponse>>.Success(allStudentDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
