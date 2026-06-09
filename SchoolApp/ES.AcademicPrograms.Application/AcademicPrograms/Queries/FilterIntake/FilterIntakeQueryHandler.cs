using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake
{
    public class FilterIntakeQueryHandler : IRequestHandler<FilterIntakeQuery, Result<PagedResult<FilterIntakeResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IIntakeServices _intakeServices ;


        public FilterIntakeQueryHandler(IMapper mapper, IIntakeServices intakeServices)
        {
            _mapper = mapper;
            _intakeServices = intakeServices;
            
        }

        public async Task<Result<PagedResult<FilterIntakeResponse>>> Handle(FilterIntakeQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterIntakeQuery).Name
                 .Replace("Filter", "")
                 .Replace("Query", "");
            try
            {

                var result = await _intakeServices.FilterIntake(request.PaginationRequest, request.FilterIntakeDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterIntakeResponse>>(result.Data);

                return Result<PagedResult<FilterIntakeResponse>>.Success(filterResult, $"{entityName} return successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterIntakeResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
