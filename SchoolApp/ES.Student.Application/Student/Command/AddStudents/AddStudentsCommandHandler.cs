using AutoMapper;
using ES.Student.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.AddStudents
{
    public class AddStudentsCommandHandler : IRequestHandler<AddStudentsCommand, Result<AddStudentsResponse>>
    {

        private readonly IMapper _mapper;
        private readonly IStudentServices _studentServices;
        private readonly IValidator<AddStudentsCommand> _validator;

        public AddStudentsCommandHandler(IMapper mapper, IValidator<AddStudentsCommand> validator,IStudentServices studentServices)
        {
            _validator = validator;
            _mapper = mapper;
            _studentServices = studentServices;

        }

        public async Task<Result<AddStudentsResponse>> Handle(AddStudentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddStudentsResponse>.Failure(errors);
                }

                var addStudents = await _studentServices.Add(request);

                if (addStudents.Errors.Any())
                {
                    var errors = string.Join(", ", addStudents.Errors);
                    return Result<AddStudentsResponse>.Failure(errors);
                }

                if (addStudents is null || !addStudents.IsSuccess)
                {
                    return Result<AddStudentsResponse>.Failure(" ");
                }

                var studentsDisplay = _mapper.Map<AddStudentsResponse>(request);
                return Result<AddStudentsResponse>.Success(studentsDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Students", ex);


            }
        }
    }
}
