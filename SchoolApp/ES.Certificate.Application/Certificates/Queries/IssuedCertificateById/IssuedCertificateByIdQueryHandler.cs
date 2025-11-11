using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Queries.IssuedCertificateById
{
    public record IssuedCertificateByIdQueryHandler : IRequestHandler<IssuedCertificateByIdQuery, Result<IssuedCertificateByIdResponse>>
    {
        private readonly IMapper _mapper;

        private readonly IIssuedCertificateServices _issuedCertificateServices;
        public IssuedCertificateByIdQueryHandler(IMapper mapper, IIssuedCertificateServices issuedCertificateServices)
        {
            _issuedCertificateServices = issuedCertificateServices;
            _mapper = mapper;
            
        }
        public async Task<Result<IssuedCertificateByIdResponse>> Handle(IssuedCertificateByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var IssuedcertificateById = await _issuedCertificateServices.GetIssuedCertificate(request.id);
                return Result<IssuedCertificateByIdResponse>.Success(IssuedcertificateById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching IssuedCertificate BuId by using id", ex);
            }
        }
    }
}
