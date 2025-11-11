using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate
{
    public class FilterIssuedCertificateQueryHandler : IRequestHandler<FilterIssuedCertificateQuery, Result<PagedResult<FilterIssuedCertificateResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IIssuedCertificateServices _issuedCertificateServices;

        public FilterIssuedCertificateQueryHandler(IMapper mapper, IIssuedCertificateServices issuedCertificateServices)
        {
            _issuedCertificateServices = issuedCertificateServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<FilterIssuedCertificateResponse>>> Handle(FilterIssuedCertificateQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _issuedCertificateServices.GetFilterIssuedCertificate(request.PaginationRequest, request.FilterIssuedCertificateDTOs);

                var issuedCertificateResult = _mapper.Map<PagedResult<FilterIssuedCertificateResponse>>(result.Data);

                return Result<PagedResult<FilterIssuedCertificateResponse>>.Success(issuedCertificateResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterIssuedCertificateResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
