using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.UpdateStudents;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.UpdateParent
{
    public class UpdateParentCommandHandler : IRequestHandler<UpdateParentCommand, Result<UpdateParentResponse>>
    {

        private readonly IMapper _mapper;
        private readonly IStudentServices _studentServices;
        private readonly IValidator<UpdateParentCommand> _validator;

        public UpdateParentCommandHandler(IMapper mapper, IStudentServices studentServices, IValidator<UpdateParentCommand> validator)
        {
            _validator = validator;
            _mapper = mapper;
            _studentServices = studentServices;
            
        }
        public async Task<Result<UpdateParentResponse>> Handle(UpdateParentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateParentResponse>.Failure(errors);

                }

                var updateParents = await _studentServices.UpdateParent(request.id, request);

                if (updateParents.Errors.Any())
                {
                    var errors = string.Join(", ", updateParents.Errors);
                    return Result<UpdateParentResponse>.Failure(errors);
                }

                if (updateParents is null || !updateParents.IsSuccess)
                {
                    return Result<UpdateParentResponse>.Failure("Updates parents failed");
                }

                var parentsDisplay = _mapper.Map<UpdateParentResponse>(updateParents.Data);
                return Result<UpdateParentResponse>.Success(parentsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating students  by {request.id}", ex);
            }
        }
    }
}
