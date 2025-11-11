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

namespace ES.Certificate.Application.Certificates.Queries.IssuedCertificate
{
    public class IssuedCertificateQueryHandler : IRequestHandler<IssuedCertificateQuery, Result<PagedResult<IssuedCertificateResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IIssuedCertificateServices _issuedCertificateServices;

        public IssuedCertificateQueryHandler(IMapper mapper, IIssuedCertificateServices issuedCertificateServices)
        {
            _issuedCertificateServices = issuedCertificateServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<IssuedCertificateResponse>>> Handle(IssuedCertificateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var issuedCertificate = await _issuedCertificateServices.GetAllIssuedCertificate(request.PaginationRequest);
                var issuedCertificateResult = _mapper.Map<PagedResult<IssuedCertificateResponse>>(issuedCertificate.Data);
                return Result<PagedResult<IssuedCertificateResponse>>.Success(issuedCertificateResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all Issued Certificate", ex);
            }
        }
    }
}
