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

namespace ES.Student.Application.Student.Command.ImportExcelForStudent
{
    public class StudentExcelCommandHandler : IRequestHandler<StudentExcelCommand, Result<StudentExcelResponse>>
    {

        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;
        private readonly IValidator<StudentExcelCommand> _validator;

        public StudentExcelCommandHandler(IStudentServices studentServices, IMapper mapper, IValidator<StudentExcelCommand> validator)
        {
            _studentServices = studentServices;
            _mapper = mapper;
            _validator = validator;
        }


        public async Task<Result<StudentExcelResponse>> Handle(StudentExcelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<StudentExcelResponse>.Failure(errors);
                }

                var itemsExcel = await _studentServices.AddStudentExcel(request.formFile);

                if (itemsExcel.Errors.Any())
                {
                    var errors = string.Join(", ", itemsExcel.Errors);
                    return Result<StudentExcelResponse>.Failure(errors);
                }

                if (itemsExcel is null || !itemsExcel.IsSuccess)
                {
                    return Result<StudentExcelResponse>.Failure(" ");
                }

                return Result<StudentExcelResponse>.Success(itemsExcel.Message);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Item", ex);


            }
        }
    }
}
