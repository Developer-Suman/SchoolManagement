using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.District;
using TN.Setup.Application.Setup.Queries.School;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Company
{
    public sealed class GetAllSchoolQueryHandler : IRequestHandler<GetAllSchoolQuery, Result<PagedResult<GetAllSchoolQueryResponse>>>
    {
        private readonly ISchoolServices _schoolServices;
        private readonly IMapper _mapper;

        public GetAllSchoolQueryHandler(ISchoolServices schoolServices, IMapper mapper) 
        {

            _schoolServices = schoolServices;
            _mapper= mapper;
        }
        public async Task<Result<PagedResult<GetAllSchoolQueryResponse>>> Handle(GetAllSchoolQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSchool = await _schoolServices.GetAllSchool(request.PaginationRequest, cancellationToken);
                var allSchoolDisplay = _mapper.Map<PagedResult<GetAllSchoolQueryResponse>>(allSchool.Data);

                return Result<PagedResult<GetAllSchoolQueryResponse>>.Success(allSchoolDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all school", ex);
            }
        }
    }
}
