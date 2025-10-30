using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.GetStudentsById;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Queries.GetParentById
{
    public class GetAllParentByIdQueryHandler:IRequestHandler<GetAllParentByIdQuery,Result<GetParentByIdQueryResponse>>
    {
        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;

        public GetAllParentByIdQueryHandler(IStudentServices studentServices,IMapper mapper)
        {
            _studentServices = studentServices;
            _mapper = mapper;
            
        }

        public async Task<Result<GetParentByIdQueryResponse>> Handle(GetAllParentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var parentById = await _studentServices.GetParentById(request.id);
                return Result<GetParentByIdQueryResponse>.Success(parentById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching parents by using id", ex);
            }
        }
    }
}
