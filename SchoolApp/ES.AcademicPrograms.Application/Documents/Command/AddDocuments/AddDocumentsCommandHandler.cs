using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocuments
{
    public class AddDocumentsCommandHandler : IRequestHandler<AddDocumentsCommand, Result<AddDocumentsResponse>>
    {
        private readonly IValidator<AddDocumentsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public AddDocumentsCommandHandler(IValidator<AddDocumentsCommand> validator, IMapper mapper, IDocumentsServices documentsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _documentsServices = documentsServices;
        }
        public async Task<Result<AddDocumentsResponse>> Handle(AddDocumentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddDocumentsResponse>.Failure(errors);
                }

                var addExam = await _documentsServices.AddDocuments(request);

                if (addExam.Errors.Any())
                {
                    var errors = string.Join(", ", addExam.Errors);
                    return Result<AddDocumentsResponse>.Failure(errors);
                }

                if (addExam is null || !addExam.IsSuccess)
                {
                    return Result<AddDocumentsResponse>.Failure(" ");
                }

                var addExamDisplay = _mapper.Map<AddDocumentsResponse>(addExam.Data);
                return Result<AddDocumentsResponse>.Success(addExamDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
