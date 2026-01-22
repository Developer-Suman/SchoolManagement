using AutoMapper;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards
{
    public class UpdateAwardsCommandHandler : IRequestHandler<UpdateSchoolAwardsCommand, Result<UpdateSchoolAwardsResponse>>
    {
        private readonly IValidator<UpdateSchoolAwardsCommand> _validator;
        public readonly IMapper _mapper;
        private readonly ISchoolAwardsServices _schoolAwardsServices;
        public UpdateAwardsCommandHandler(IValidator<UpdateSchoolAwardsCommand> validator, ISchoolAwardsServices schoolAwardsServices, IMapper mapper)
        {
            _schoolAwardsServices = schoolAwardsServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<UpdateSchoolAwardsResponse>> Handle(UpdateSchoolAwardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSchoolAwardsResponse>.Failure(errors);

                }

                var updateAwards = await _schoolAwardsServices.Update(request.Id, request);

                if (updateAwards.Errors.Any())
                {
                    var errors = string.Join(", ", updateAwards.Errors);
                    return Result<UpdateSchoolAwardsResponse>.Failure(errors);
                }

                if (updateAwards is null || !updateAwards.IsSuccess)
                {
                    return Result<UpdateSchoolAwardsResponse>.Failure("Update Award failed");
                }

                var UpdateCertificateTemplateDisplay = _mapper.Map<UpdateSchoolAwardsResponse>(updateAwards.Data);
                return Result<UpdateSchoolAwardsResponse>.Success(UpdateCertificateTemplateDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Awards", ex);
            }
        }
    }
}
