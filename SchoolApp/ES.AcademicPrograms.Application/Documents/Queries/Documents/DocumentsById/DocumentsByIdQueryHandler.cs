using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Queries.Documents.DocumentsById
{
    public class DocumentsByIdQueryHandler : IRequestHandler<DocumentsByIdQuery, Result<DocumentsByIdResponse>>
    {
        private readonly IDocumentsServices _documentsServices;
        private readonly IMapper _mapper;

        public DocumentsByIdQueryHandler(IDocumentsServices documentsServices, IMapper mapper)
        {
            _documentsServices = documentsServices;
            _mapper = mapper;

        }
        public async Task<Result<DocumentsByIdResponse>> Handle(DocumentsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _documentsServices.DocumentsById(request.documentsId);
                return Result<DocumentsByIdResponse>.Success(result.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching by using id", ex);
            }
        }
    }
}
