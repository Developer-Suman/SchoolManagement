using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using ZXing;

namespace ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards
{
    public class FilterSchoolAwardsQueryHandler : IRequestHandler<FilterSchoolAwardsQuery, Result<PagedResult<FilterSchoolAwardsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolAwardsServices _schoolAwardsServices;

        public FilterSchoolAwardsQueryHandler(IMapper mapper, ISchoolAwardsServices schoolAwardsServices)
        {
            _schoolAwardsServices = schoolAwardsServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterSchoolAwardsResponse>>> Handle(FilterSchoolAwardsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolAwardsServices.GetFilterSchoolAwards(request.PaginationRequest, request.FilterSchoolAwardsDTOs);

                var filterSchoolAwardsResult = _mapper.Map<PagedResult<FilterSchoolAwardsResponse>>(result.Data);

                return Result<PagedResult<FilterSchoolAwardsResponse>>.Success(filterSchoolAwardsResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSchoolAwardsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
