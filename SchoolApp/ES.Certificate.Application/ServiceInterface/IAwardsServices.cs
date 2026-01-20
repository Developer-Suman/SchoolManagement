using ES.Certificate.Application.Certificates.Command.Awards.AddAwards;
using ES.Certificate.Application.Certificates.Queries.Awards;
using ES.Certificate.Application.Certificates.Queries.AwardsById;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.ServiceInterface
{
    public interface IAwardsServices
    {
        Task<Result<AddAwardsResponse>> Add(AddAwardsCommand addAwardsCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<AwardsResponse>>> GetAllAwardsResponse(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AwardsByIdResponse>> GetAwards(string certificateTemplateId, CancellationToken cancellationToken = default);
    }
}
