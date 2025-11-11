using AutoMapper;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.Certificates.Queries.CertificateTemplate
{
    public class CertificateTemplateQueryHandler : IRequestHandler<CertificateTemplateQuery, Result<PagedResult<CertificateTemplateResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICertificateTemplateServices _certificateTemplateServices;

        public CertificateTemplateQueryHandler(IMapper mapper, ICertificateTemplateServices certificateTemplateServices)
        {
            _certificateTemplateServices = certificateTemplateServices;
            
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<CertificateTemplateResponse>>> Handle(CertificateTemplateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var certificateTemplate = await _certificateTemplateServices.GetAllCertificateTemplate(request.PaginationRequest);
                var certificateTemplateResult = _mapper.Map<PagedResult<CertificateTemplateResponse>>(certificateTemplate.Data);
                return Result<PagedResult<CertificateTemplateResponse>>.Success(certificateTemplateResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Certificate Template", ex);
            }
        }
    }
}
