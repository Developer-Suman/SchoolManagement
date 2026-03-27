using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments
{
    public class NonRequiredDocumentsCommandHandler : IRequestHandler<NonRequiredDocumentsCommand, Result<NonRequiredDocumentsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public NonRequiredDocumentsCommandHandler(IDocumentsServices documentsServices, IMapper mapper)
        {
            _documentsServices = documentsServices;
            _mapper = mapper;

        }
        public async Task<Result<NonRequiredDocumentsResponse>> Handle(NonRequiredDocumentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var docCheckList = await _documentsServices.NonRequired(request.dockCheckListId);

                if (docCheckList.Errors.Any())
                {
                    var errors = string.Join(", ", docCheckList.Errors);
                    return Result<NonRequiredDocumentsResponse>.Failure(errors);
                }

                if (docCheckList is null || !docCheckList.IsSuccess)
                {
                    return Result<NonRequiredDocumentsResponse>.Failure(" ");
                }

                var docCheckListDisplay = _mapper.Map<NonRequiredDocumentsResponse>(docCheckList.Data);
                return Result<NonRequiredDocumentsResponse>.Success(docCheckListDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);


            }
        }
    }
}
