using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Command.DeleteDocuments
{
    public class DeleteDocumentsCommandHandler : IRequestHandler<DeleteDocumentsCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public DeleteDocumentsCommandHandler(IDocumentsServices documentsServices,IMapper mapper)
        {
            _mapper = mapper;
            _documentsServices = documentsServices;
            
        }


        public async Task<Result<bool>> Handle(DeleteDocumentsCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeleteDocumentsCommand).Name
                  .Replace("Delete", "")
                  .Replace("Command", "");
            try
            {
                var delete = await _documentsServices.Delete(request.Id, cancellationToken);
                if (delete is null)
                {
                    return Result<bool>.Failure("NotFound", $"{entityName} not Found");
                }
                return Result<bool>.Success(true, $"{entityName} Deleted Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
