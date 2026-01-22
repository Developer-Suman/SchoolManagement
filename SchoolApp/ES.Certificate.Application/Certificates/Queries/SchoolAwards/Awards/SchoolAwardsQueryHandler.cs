using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.Certificates.Queries.SchoolAwards.Awards
{
    public class SchoolAwardsQueryHandler : IRequestHandler<SchoolAwardsQuery, Result<PagedResult<SchoolAwardsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolAwardsServices _schoolAwardsServices;
        public SchoolAwardsQueryHandler(IMapper mapper, ISchoolAwardsServices schoolAwardsServices)
        {
            _schoolAwardsServices = schoolAwardsServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<SchoolAwardsResponse>>> Handle(SchoolAwardsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var awards = await _schoolAwardsServices.GetAllAwardsResponse(request.PaginationRequest);
                var awardsResult = _mapper.Map<PagedResult<SchoolAwardsResponse>>(awards.Data);
                return Result<PagedResult<SchoolAwardsResponse>>.Success(awardsResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Awards", ex);
            }
        }
    }
}
