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

namespace ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate
{
    public class FilterCertificateTemplateQueryHandler : IRequestHandler<FilterCertificateTemplateQuery, Result<PagedResult<FilterCertificateTemplateResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICertificateTemplateServices _certificateTemplateServices;

        public FilterCertificateTemplateQueryHandler(IMapper mapper, ICertificateTemplateServices certificateTemplateServices)
        {
            _certificateTemplateServices = certificateTemplateServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<FilterCertificateTemplateResponse>>> Handle(FilterCertificateTemplateQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _certificateTemplateServices.GetFilterCertificateTemplate(request.PaginationRequest, request.FilterCertificateTemplatesDTOs);

                var certificateTemplateResult = _mapper.Map<PagedResult<FilterCertificateTemplateResponse>>(result.Data);

                return Result<PagedResult<FilterCertificateTemplateResponse>>.Success(certificateTemplateResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterCertificateTemplateResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
