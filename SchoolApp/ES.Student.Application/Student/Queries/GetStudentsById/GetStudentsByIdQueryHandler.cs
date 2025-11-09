using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Queries.GetStudentsById
{
    public  class GetStudentsByIdQueryHandler: IRequestHandler<GetStudentsByIdQuery, Result<GetStudentsByIdQueryResponse>>
    {
        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;

        public GetStudentsByIdQueryHandler(IStudentServices studentServices,IMapper mapper)
        {
            _studentServices = studentServices;
            _mapper = mapper;
        }

        public async Task<Result<GetStudentsByIdQueryResponse>> Handle(GetStudentsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var studentById = await _studentServices.GetStudentById(request.id);
                return Result<GetStudentsByIdQueryResponse>.Success(studentById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching students by using id", ex);
            }
        }
    }
}
