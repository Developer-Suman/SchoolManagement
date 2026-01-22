using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.AwardsById;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.ServiceInterface
{
    public interface ISchoolAwardsServices
    {
        Task<Result<AddSchoolAwardsResponse>> Add(AddSchoolAwardsCommand addAwardsCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<SchoolAwardsResponse>>> GetAllAwardsResponse(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<SchoolAwardsByIdResponse>> GetAwards(string awardsId, CancellationToken cancellationToken = default);
        Task<Result<UpdateSchoolAwardsResponse>> Update(string awardsId, UpdateSchoolAwardsCommand updateAwardsCommand);

        Task<Result<PagedResult<FilterSchoolAwardsResponse>>> GetFilterSchoolAwards(PaginationRequest paginationRequest, FilterSchoolAwardsDTOs filterSchoolAwardsDTOs);
    }
}
