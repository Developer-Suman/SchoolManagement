using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificateById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.ServiceInterface
{
    public interface IIssuedCertificateServices
    {
        Task<Result<AddIssuedCertificateResponse>> Add(AddIssuedCertificateCommand addIssuedCertificateCommand);
        Task<Result<PagedResult<IssuedCertificateResponse>>> GetAllIssuedCertificate(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<IssuedCertificateByIdResponse>> GetIssuedCertificate(string issuedCertificateId, CancellationToken cancellationToken = default);

        Task<Result<UpdateIssuedCertificateResponse>> Update(string issuedCertificateId, UpdateIssuedCertificateCommand  updateIssuedCertificateCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterIssuedCertificateResponse>>> GetFilterIssuedCertificate(PaginationRequest paginationRequest, FilterIssuedCertificateDTOs filterIssuedCertificateDTOs);
    }
}
