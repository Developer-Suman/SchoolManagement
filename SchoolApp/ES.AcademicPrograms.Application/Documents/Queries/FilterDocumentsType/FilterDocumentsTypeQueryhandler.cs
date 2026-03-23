using AutoMapper;
using ES.AcademicPrograms.Application.Documents.Queries.FilterDocuments;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.Documents.Queries.FilterDocumentsType
{
    public class FilterDocumentsTypeQueryhandler : IRequestHandler<FilterDocumentsTypeQuery, Result<PagedResult<FilterDocumentsTypeResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public FilterDocumentsTypeQueryhandler(IMapper mapper, IDocumentsServices documentsServices)
        {
            _mapper = mapper;
            _documentsServices = documentsServices;
        }
        public async Task<Result<PagedResult<FilterDocumentsTypeResponse>>> Handle(FilterDocumentsTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _documentsServices.FilterDocumentsType(request.FilterDocumentsTypeDTOs, request.PaginationRequest);

                var resultDetails = _mapper.Map<PagedResult<FilterDocumentsTypeResponse>>(result.Data);

                return Result<PagedResult<FilterDocumentsTypeResponse>>.Success(resultDetails);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterDocumentsTypeResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
