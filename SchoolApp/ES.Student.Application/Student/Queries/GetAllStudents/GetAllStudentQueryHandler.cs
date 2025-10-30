using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.GetAllStudents
{
    public class GetAllStudentQueryHandler : IRequestHandler<GetAllStudentQuery, Result<PagedResult<GetAllStudentQueryResponse>>>
    {
        private readonly IStudentServices _studnetServices;
        private readonly IMapper _mapper;

        public GetAllStudentQueryHandler(IStudentServices studentServices,IMapper mapper)
        {
            _studnetServices= studentServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<GetAllStudentQueryResponse>>> Handle(GetAllStudentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allStudent = await _studnetServices.GetAllStudents(request.PaginationRequest, cancellationToken);
                var allStudentDisplay = _mapper.Map<PagedResult<GetAllStudentQueryResponse>>(allStudent.Data);
                return Result<PagedResult<GetAllStudentQueryResponse>>.Success(allStudentDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
