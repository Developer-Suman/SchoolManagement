using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.FilterSchoolClass
{
    public class FilterSchoolClassQueryHandler : IRequestHandler<FilterSchoolClassQuery, Result<PagedResult<FilterSchoolClassQueryResponse>>>
    {

        private readonly ISchoolClassInterface _schoolClassInterface;
        private readonly IMapper _mapper;

        public FilterSchoolClassQueryHandler(ISchoolClassInterface schoolClassInterface, IMapper mapper)
        {
            _schoolClassInterface = schoolClassInterface;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterSchoolClassQueryResponse>>> Handle(FilterSchoolClassQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolClassInterface.GetFilterSchoolClass(request.PaginationRequest, request.FilterSchoolClassDTOs);

                var schoolClassResult = _mapper.Map<PagedResult<FilterSchoolClassQueryResponse>>(result.Data);

                return Result<PagedResult<FilterSchoolClassQueryResponse>>.Success(schoolClassResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSchoolClassQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
