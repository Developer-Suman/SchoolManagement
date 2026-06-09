using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.Documents.Queries.Documents.FilterDocuments
{
    public class FilterDocumentsQueryHandler : IRequestHandler<FilterDocumentsQuery, Result<PagedResult<FilterDocumentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public FilterDocumentsQueryHandler(IMapper mapper, IDocumentsServices documentsServices)
        {
            _mapper = mapper;
            _documentsServices = documentsServices;
        }
        public async Task<Result<PagedResult<FilterDocumentsResponse>>> Handle(FilterDocumentsQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterDocumentsQuery).Name
                    .Replace("Filter", "")
                    .Replace("Query", "");

            try
            {

                var result = await _documentsServices.FilterDocuments(request.FilterDocumentsDTOs, request.PaginationRequest);

                var resultDisplay = _mapper.Map<PagedResult<FilterDocumentsResponse>>(result.Data);

                return Result<PagedResult<FilterDocumentsResponse>>.Success(resultDisplay, $"{entityName} returned Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
