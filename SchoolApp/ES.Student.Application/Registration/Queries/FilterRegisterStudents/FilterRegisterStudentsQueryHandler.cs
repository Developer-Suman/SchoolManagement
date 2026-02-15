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

namespace ES.Student.Application.Registration.Queries.FilterRegisterStudents
{
    public class FilterRegisterStudentsQueryHandler : IRequestHandler<FilterRegisterStudentsQuery, Result<PagedResult<FilterRegisterStudentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IRegistrationServices _registrationServices;

        public FilterRegisterStudentsQueryHandler(IMapper mapper, IRegistrationServices registrationServices)
        {
            _mapper = mapper;
            _registrationServices = registrationServices;

        }
        public async Task<Result<PagedResult<FilterRegisterStudentsResponse>>> Handle(FilterRegisterStudentsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _registrationServices.GetFilterStudentRegistration(request.PaginationRequest, request.FilterRegisterStudentsDTOs);

                var registerStudentsFilter = _mapper.Map<PagedResult<FilterRegisterStudentsResponse>>(result.Data);

                return Result<PagedResult<FilterRegisterStudentsResponse>>.Success(registerStudentsFilter);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterRegisterStudentsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
