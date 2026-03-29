using AutoMapper;
using ES.AcademicPrograms.Application.Documents.Command.AddDocuments;
using ES.AcademicPrograms.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments
{
    public class UploadApplicantDocumentsCommandHandler : IRequestHandler<UploadApplicantDocumentsCommand, Result<UploadApplicantDocumentsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;
        private readonly IValidator<UploadApplicantDocumentsCommand> _validator;

        public UploadApplicantDocumentsCommandHandler(IMapper mapper, IValidator<UploadApplicantDocumentsCommand> validator, IDocumentsServices documentsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _documentsServices = documentsServices;
            
        }
        public async Task<Result<UploadApplicantDocumentsResponse>> Handle(UploadApplicantDocumentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UploadApplicantDocumentsResponse>.Failure(errors);
                }

                var command = await _documentsServices.UploadApplicantDocuments(request);

                if (command.Errors.Any())
                {
                    var errors = string.Join(", ", command.Errors);
                    return Result<UploadApplicantDocumentsResponse>.Failure(errors);
                }

                if (command is null || !command.IsSuccess)
                {
                    return Result<UploadApplicantDocumentsResponse>.Failure(" ");
                }

                var commandDisplay = _mapper.Map<UploadApplicantDocumentsResponse>(command.Data);
                return Result<UploadApplicantDocumentsResponse>.Success(commandDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
