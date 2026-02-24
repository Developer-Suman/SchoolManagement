using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements
{
    public class FilterRequirementsQueryHandler : IRequestHandler<FilterRequirementsQuery, Result<PagedResult<FilterRequirementsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IRequirementsServices _requirementsServices;

        public FilterRequirementsQueryHandler(IMapper mapper, IRequirementsServices requirementsServices)
        {
            _mapper = mapper;
            _requirementsServices = requirementsServices;

        }
        public async Task<Result<PagedResult<FilterRequirementsResponse>>> Handle(FilterRequirementsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _requirementsServices.FilterRequirements(request.PaginationRequest, request.FilterRequirementsDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterRequirementsResponse>>(result.Data);

                return Result<PagedResult<FilterRequirementsResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterRequirementsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
    
}
