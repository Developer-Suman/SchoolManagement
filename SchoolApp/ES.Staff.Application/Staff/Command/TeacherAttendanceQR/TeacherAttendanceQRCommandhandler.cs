using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.TeacherAttendanceQR
{
    public class TeacherAttendanceQRCommandhandler : IRequestHandler<TeacherAttendanceQRCommand, Result<TeacherAttendanceQRResponse>>
    {
        private readonly ITeacherAttendanceServices _teacherAttendanceServices;
        private readonly IMapper _mapper;
        private readonly IValidator<TeacherAttendanceQRCommand> _validator;

        public TeacherAttendanceQRCommandhandler(ITeacherAttendanceServices teacherAttendanceServices, IValidator<TeacherAttendanceQRCommand> validator, IMapper mapper)
        {
            _teacherAttendanceServices = teacherAttendanceServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<TeacherAttendanceQRResponse>> Handle(TeacherAttendanceQRCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<TeacherAttendanceQRResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<TeacherAttendanceQRResponse>.Failure("Invalid request");
                }

                var teacherAttendenceQR = await _teacherAttendanceServices.CreateQR(request);

                if (teacherAttendenceQR.Errors.Any())
                {
                    var errors = string.Join(", ", teacherAttendenceQR.Errors);
                    return Result<TeacherAttendanceQRResponse>.Failure(errors);
                }

                if (teacherAttendenceQR is null || !teacherAttendenceQR.IsSuccess)
                {
                    return Result<TeacherAttendanceQRResponse>.Failure("Add AssignClass Failed");
                }
                var teacherAttendenceQRDisplay = _mapper.Map<TeacherAttendanceQRResponse>(request);

                if (teacherAttendenceQRDisplay is null)
                {
                    return Result<TeacherAttendanceQRResponse>.Failure("Mapping to teacherAttendence Failed");
                }
                return Result<TeacherAttendanceQRResponse>.Success(teacherAttendenceQRDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong");
            }
        }
    }
}
