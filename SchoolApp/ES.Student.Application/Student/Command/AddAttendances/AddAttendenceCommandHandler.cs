using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.AddParent;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.AddAttendances
{
    public class AddAttendenceCommandHandler : IRequestHandler<AddAttendenceCommand, Result<List<AddAttendanceResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IAttendanceServices _attendanceServices;
        private readonly IValidator<AddAttendenceCommand> _validator;

        public AddAttendenceCommandHandler(IMapper mapper, IAttendanceServices attendanceServices, IValidator<AddAttendenceCommand> validator)
        {
            _validator = validator;
            _attendanceServices = attendanceServices;
            _mapper = mapper;

        }

        public async Task<Result<List<AddAttendanceResponse>>> Handle(AddAttendenceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<List<AddAttendanceResponse>>.Failure(errors);
                }

                var addAttendance = await _attendanceServices.MarkBulkAsync(request);

                if (addAttendance.Errors.Any())
                {
                    var errors = string.Join(", ", addAttendance.Errors);
                    return Result<List<AddAttendanceResponse>>.Failure(errors);
                }

                if (addAttendance is null || !addAttendance.IsSuccess)
                {
                    return Result<List<AddAttendanceResponse>>.Failure(" ");
                }

                var attendanceDisplay = _mapper.Map<List<AddAttendanceResponse>>(addAttendance.Data);
                return Result<List<AddAttendanceResponse>>.Success(attendanceDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Attendences", ex);


            }
        }
    }
}
