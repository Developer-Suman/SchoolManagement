using AutoMapper;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Queries.CertificateTemplateById
{
    public class CertificateTemplateByIdQueryHandler : IRequestHandler<CertificateTemplateByIdQuery, Result<CertificateTemplateByIdResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICertificateTemplateServices _certificateTemplateServices;

        public CertificateTemplateByIdQueryHandler(IMapper mapper, ICertificateTemplateServices certificateTemplateServices)
        {
            _certificateTemplateServices = certificateTemplateServices;
            _mapper = mapper;
            
        }
        public async Task<Result<CertificateTemplateByIdResponse>> Handle(CertificateTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var certificateTemplateById = await _certificateTemplateServices.GetCertificateTemplate(request.id);
                return Result<CertificateTemplateByIdResponse>.Success(certificateTemplateById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Certificate Template by using id", ex);
            }
        }
    }
}
