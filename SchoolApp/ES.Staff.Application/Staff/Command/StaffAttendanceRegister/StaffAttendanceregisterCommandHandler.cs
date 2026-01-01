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

namespace ES.Staff.Application.Staff.Command.StaffAttendanceRegister
{
    public class StaffAttendanceregisterCommandHandler : IRequestHandler<StaffAttendanceregisterCommand, Result<StaffAttendanceregisterResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<StaffAttendanceregisterCommand> _validator;
        private readonly ITeacherAttendanceServices _teacherAttendanceServices;

        public StaffAttendanceregisterCommandHandler(IMapper mapper, IValidator<StaffAttendanceregisterCommand> validator, ITeacherAttendanceServices teacherAttendanceServices)
        {
            _validator = validator;
            _mapper = mapper;
            _teacherAttendanceServices = teacherAttendanceServices;
        }
        public async Task<Result<StaffAttendanceregisterResponse>> Handle(StaffAttendanceregisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<StaffAttendanceregisterResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<StaffAttendanceregisterResponse>.Failure("Invalid request");
                }

                var teacherAttendanceRegister = await _teacherAttendanceServices.ResitserStaffAttendance(request);

                if (teacherAttendanceRegister.Errors.Any())
                {
                    var errors = string.Join(", ", teacherAttendanceRegister.Errors);
                    return Result<StaffAttendanceregisterResponse>.Failure(errors);
                }

                if (teacherAttendanceRegister is null || !teacherAttendanceRegister.IsSuccess)
                {
                    return Result<StaffAttendanceregisterResponse>.Failure("Add Failed");
                }
                var assignClassDisplay = _mapper.Map<StaffAttendanceregisterResponse>(request);

                if (assignClassDisplay is null)
                {
                    return Result<StaffAttendanceregisterResponse>.Failure("Mapping Failed");
                }
                return Result<StaffAttendanceregisterResponse>.Success(assignClassDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong");
            }
        }
    }
}
