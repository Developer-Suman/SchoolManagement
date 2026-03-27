using AutoMapper;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.RequiredDocument
{
    public class RequiredDocumentsCommandHandler : IRequestHandler<RequiredDocumentsCommand, Result<RequiredDocumentsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public RequiredDocumentsCommandHandler(IDocumentsServices documentsServices, IMapper mapper)
        {
            _documentsServices = documentsServices;
            _mapper = mapper;

        }
        public async Task<Result<RequiredDocumentsResponse>> Handle(RequiredDocumentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var docCheckList = await _documentsServices.Required(request.dockCheckListId);

                if (docCheckList.Errors.Any())
                {
                    var errors = string.Join(", ", docCheckList.Errors);
                    return Result<RequiredDocumentsResponse>.Failure(errors);
                }

                if (docCheckList is null || !docCheckList.IsSuccess)
                {
                    return Result<RequiredDocumentsResponse>.Failure(" ");
                }

                var docCheckListDisplay = _mapper.Map<RequiredDocumentsResponse>(docCheckList.Data);
                return Result<RequiredDocumentsResponse>.Success(docCheckListDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);


            }
        }
    }
}
