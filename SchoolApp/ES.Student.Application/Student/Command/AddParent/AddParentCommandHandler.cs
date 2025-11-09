using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Student.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.AddParent
{
    public class AddParentCommandHandler:IRequestHandler<AddParentCommand,Result<AddParentResponse>>
    {
        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddParentCommand> _validator;

        public AddParentCommandHandler(IStudentServices studentServices,IMapper mapper,IValidator<AddParentCommand> validator)
        {
            _studentServices = studentServices;
            _mapper = mapper;
            _validator = validator;


        }

        public async Task<Result<AddParentResponse>> Handle(AddParentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddParentResponse>.Failure(errors);
                }

                var addParent = await _studentServices.Add(request);

                if (addParent.Errors.Any())
                {
                    var errors = string.Join(", ", addParent.Errors);
                    return Result<AddParentResponse>.Failure(errors);
                }

                if (addParent is null || !addParent.IsSuccess)
                {
                    return Result<AddParentResponse>.Failure(" ");
                }

                var parentDisplay = _mapper.Map<AddParentResponse>(request);
                return Result<AddParentResponse>.Success(parentDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding parents", ex);


            }
        }
    }
}
