using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.FilterConsultancyClass
{
    public class FilterConsultancyClassQueryHandler : IRequestHandler<FilterConsultancyClassQuery, Result<PagedResult<FilterConsultancyClassResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IConsultancyClassServices _consultancyClassServices;

        public FilterConsultancyClassQueryHandler(IMapper mapper, IConsultancyClassServices consultancyClassServices)
        {
            _mapper = mapper;
            _consultancyClassServices = consultancyClassServices;

        }
        public async Task<Result<PagedResult<FilterConsultancyClassResponse>>> Handle(FilterConsultancyClassQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterConsultancyClassQuery).Name
                   .Replace("Filter", "")
                   .Replace("Query", "");
            try
            {

                var result = await _consultancyClassServices.Filter(request.PaginationRequest, request.FilterConsultancyClassDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterConsultancyClassResponse>>(result.Data);

                return Result<PagedResult<FilterConsultancyClassResponse>>.Success(resultDisplay, $"{entityName} return Successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterConsultancyClassResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
