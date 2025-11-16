using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificate;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.Certificates.Queries.GenerateCertificate
{
    public class GenerateCertificateQueryHandler : IRequestHandler<GenerateCertificateQuery, Result<GenerateCertificateResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IIssuedCertificateServices _issuedCertificateServices;

        public GenerateCertificateQueryHandler(IMapper mapper, IIssuedCertificateServices issuedCertificateServices)
        {
            _mapper = mapper;
            _issuedCertificateServices = issuedCertificateServices;
        }
        public async Task<Result<GenerateCertificateResponse>> Handle(GenerateCertificateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var generateCertificate = await _issuedCertificateServices.GenerateCertificate(request.MarksSheetDTOs);
                var generateCertificateResult = _mapper.Map<GenerateCertificateResponse>(generateCertificate.Data);
                return Result<GenerateCertificateResponse>.Success(generateCertificateResult);



            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Generate Certificate", ex);
            }
        }
    }
}
