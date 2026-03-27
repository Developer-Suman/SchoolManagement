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

namespace ES.AcademicPrograms.Application.Documents.Queries.DocumentsType.DocumentsTypes
{
    public class DocumentsTypesQueryHandler : IRequestHandler<DocumentsTypesQuery, Result<PagedResult<DocumentsTypesResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public DocumentsTypesQueryHandler(IMapper mapper, IDocumentsServices documentsServices)
        {
            _documentsServices = documentsServices;

            _mapper = mapper;
        }
        public async Task<Result<PagedResult<DocumentsTypesResponse>>> Handle(DocumentsTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _documentsServices.DocumentsType(request.PaginationRequest);
                var resultDetails = new PagedResult<DocumentsTypesResponse>
                {
                    Items = result.Data.Items.Select(x => new DocumentsTypesResponse(
                        id: x.id,          // assuming Entity has Id property
                        name: x.name
                    )).ToList(),

                    PageIndex = result.Data.PageIndex,
                    pageSize = result.Data.pageSize,
                    TotalItems = result.Data.TotalItems
                };

                return Result<PagedResult<DocumentsTypesResponse>>.Success(resultDetails);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all", ex);
            }
        }
    }
}
