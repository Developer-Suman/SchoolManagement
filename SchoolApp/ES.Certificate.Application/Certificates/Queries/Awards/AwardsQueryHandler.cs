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

namespace ES.Certificate.Application.Certificates.Queries.Awards
{
    public class AwardsQueryHandler : IRequestHandler<AwardsQuery, Result<PagedResult<AwardsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IAwardsServices _awardsServices;
        public AwardsQueryHandler(IMapper mapper, IAwardsServices awardsServices)
        {
            _awardsServices = awardsServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<AwardsResponse>>> Handle(AwardsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var awards = await _awardsServices.GetAllAwardsResponse(request.PaginationRequest);
                var awardsResult = _mapper.Map<PagedResult<AwardsResponse>>(awards.Data);
                return Result<PagedResult<AwardsResponse>>.Success(awardsResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Awards", ex);
            }
        }
    }
}
