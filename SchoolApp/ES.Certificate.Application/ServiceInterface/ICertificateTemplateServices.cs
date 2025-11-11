using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.ServiceInterface
{
    public interface ICertificateTemplateServices
    {
        Task<Result<AddCertificateTemplateResponse>> Add(AddCertificateTemplateCommand addCertificateTemplateCommand);
        Task<Result<PagedResult<CertificateTemplateResponse>>> GetAllCertificateTemplate(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<CertificateTemplateByIdResponse>> GetCertificateTemplate(string certificateTemplateId, CancellationToken cancellationToken = default);

        Task<Result<UpdateCertificateTemplateResponse>> Update(string certificateTemplateId, UpdateCertificateTemplateCommand updateCertificateTemplateCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterCertificateTemplateResponse>>> GetFilterCertificateTemplate(PaginationRequest paginationRequest, FilterCertificateTemplatesDTOs filterCertificateTemplatesDTOs);
    }
}
