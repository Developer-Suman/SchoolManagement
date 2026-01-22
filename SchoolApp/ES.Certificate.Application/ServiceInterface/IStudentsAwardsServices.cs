using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.AwardsById;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.FilterStudentsAwards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.ServiceInterface
{
    public interface IStudentsAwardsServices
    {

        #region Students Awards
        Task<Result<AddAwardsResponse>> Add(AddAwardsCommand addAwardsCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<AwardsResponse>>> GetAllAwardsResponse(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AwardsByIdResponse>> GetAwards(string awardsId, CancellationToken cancellationToken = default);
        Task<Result<UpdateAwardsResponse>> Update(string awardsId, UpdateAwardsCommand updateAwardsCommand);

        Task<Result<PagedResult<FilterStudentsAwardsResponse>>> GetFilterStudentsAwards(PaginationRequest paginationRequest, FilterStudentsAwardsDTOs filterStudentsAwardsDTOs);

        #endregion
    }
}
