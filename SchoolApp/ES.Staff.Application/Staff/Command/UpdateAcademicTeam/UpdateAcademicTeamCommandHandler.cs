using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.UpdateAcademicTeam
{
    public class UpdateAcademicTeamCommandHandler : IRequestHandler<UpdateAcademicTeamCommand, Result<UpdateAcademicTeamResponse>>
    {
        private readonly IValidator<UpdateAcademicTeamCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IAcademicTeamServices _academicTeamServices;

        public UpdateAcademicTeamCommandHandler(IValidator<UpdateAcademicTeamCommand> validator, IMapper mapper, IAcademicTeamServices academicTeamServices)
        {
            _validator = validator;
            _mapper = mapper;
            _academicTeamServices = academicTeamServices;
        }
        public async Task<Result<UpdateAcademicTeamResponse>> Handle(UpdateAcademicTeamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateAcademicTeamResponse>.Failure(errors);

                }

                var updateAcademicTeam = await _academicTeamServices.Update(request.id, request);

                if (updateAcademicTeam.Errors.Any())
                {
                    var errors = string.Join(", ", updateAcademicTeam.Errors);
                    return Result<UpdateAcademicTeamResponse>.Failure(errors);
                }

                if (updateAcademicTeam is null || !updateAcademicTeam.IsSuccess)
                {
                    return Result<UpdateAcademicTeamResponse>.Failure("Updates academicTeam failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateAcademicTeamResponse>(updateAcademicTeam.Data);
                return Result<UpdateAcademicTeamResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating", ex);
            }
        }
    }
}
