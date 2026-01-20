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

namespace ES.Certificate.Application.Certificates.Command.Awards.UpdateAwards
{
    public class UpdateAwardsCommandHandler : IRequestHandler<UpdateAwardsCommand, Result<UpdateAwardsResponse>>
    {
        private readonly IValidator<UpdateAwardsCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IAwardsServices _awardsServices;
        public UpdateAwardsCommandHandler(IValidator<UpdateAwardsCommand> validator, IAwardsServices awardsServices, IMapper mapper)
        {
            _awardsServices = awardsServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<UpdateAwardsResponse>> Handle(UpdateAwardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateAwardsResponse>.Failure(errors);

                }

                var updateAwards = await _awardsServices.Update(request.Id, request);

                if (updateAwards.Errors.Any())
                {
                    var errors = string.Join(", ", updateAwards.Errors);
                    return Result<UpdateAwardsResponse>.Failure(errors);
                }

                if (updateAwards is null || !updateAwards.IsSuccess)
                {
                    return Result<UpdateAwardsResponse>.Failure("Update Award failed");
                }

                var UpdateCertificateTemplateDisplay = _mapper.Map<UpdateAwardsResponse>(updateAwards.Data);
                return Result<UpdateAwardsResponse>.Success(UpdateCertificateTemplateDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Awards", ex);
            }
        }
    }
}
