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

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType
{
    public class AddDocumentsTypeCommandHandler : IRequestHandler<AddDocumentsTypeCommand, Result<AddDocumentsTypeResponse>>
    {

        private readonly IValidator<AddDocumentsTypeCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public AddDocumentsTypeCommandHandler(IValidator<AddDocumentsTypeCommand> validator, IMapper mapper, IDocumentsServices documentsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _documentsServices = documentsServices;
        }
        public async Task<Result<AddDocumentsTypeResponse>> Handle(AddDocumentsTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddDocumentsTypeResponse>.Failure(errors);
                }

                var addExam = await _documentsServices.AddDocumentsType(request);

                if (addExam.Errors.Any())
                {
                    var errors = string.Join(", ", addExam.Errors);
                    return Result<AddDocumentsTypeResponse>.Failure(errors);
                }

                if (addExam is null || !addExam.IsSuccess)
                {
                    return Result<AddDocumentsTypeResponse>.Failure(" ");
                }

                var addExamDisplay = _mapper.Map<AddDocumentsTypeResponse>(addExam.Data);
                return Result<AddDocumentsTypeResponse>.Success(addExamDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
