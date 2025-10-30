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

namespace ES.Student.Application.Student.Command.UpdateStudents
{
    public class UpdateStudentCommandHandler:IRequestHandler<UpdateStudentCommand,Result<UpdateStudentResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentServices _studentServices;
        private readonly IValidator<UpdateStudentCommand> _validator;

        public UpdateStudentCommandHandler(IStudentServices studentServices,IMapper mapper,IValidator<UpdateStudentCommand> validator)
        {
            _mapper= mapper;
            _studentServices = studentServices;
            _validator=validator;

        }

        public async Task<Result<UpdateStudentResponse>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateStudentResponse>.Failure(errors);

                }

                var updateStudent = await _studentServices.Update(request.id, request);

                if (updateStudent.Errors.Any())
                {
                    var errors = string.Join(", ", updateStudent.Errors);
                    return Result<UpdateStudentResponse>.Failure(errors);
                }

                if (updateStudent is null || !updateStudent.IsSuccess)
                {
                    return Result<UpdateStudentResponse>.Failure("Updates student failed");
                }

                var studentsDisplay = _mapper.Map<UpdateStudentResponse>(updateStudent.Data);
                return Result<UpdateStudentResponse>.Success(studentsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating students  by {request.id}", ex);
            }
        }
    }
}
