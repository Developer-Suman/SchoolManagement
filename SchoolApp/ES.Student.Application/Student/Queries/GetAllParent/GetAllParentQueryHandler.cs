using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.GetAllStudents;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.GetAllParent
{
    public class GetAllParentQueryHandler:IRequestHandler<GetAllParentQuery,Result<PagedResult<GetAllParentQueryResponse>>>
    {
        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;

        public GetAllParentQueryHandler(IStudentServices studentServices,IMapper mapper)
        {
            _studentServices = studentServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllParentQueryResponse>>> Handle(GetAllParentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allParents = await _studentServices.GetAllParent(request.PaginationRequest, cancellationToken);
                var allParentsDisplay = _mapper.Map<PagedResult<GetAllParentQueryResponse>>(allParents.Data);
                return Result<PagedResult<GetAllParentQueryResponse>>.Success(allParentsDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
