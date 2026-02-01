using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors
{
    public class UpdateContributorsCommandHandler : IRequestHandler<UpdateContributorsCommand, Result<UpdateContributorsResponse>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateContributorsCommand> _validator;
        public UpdateContributorsCommandHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper, IValidator<UpdateContributorsCommand> validator)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateContributorsResponse>> Handle(UpdateContributorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateContributorsResponse>.Failure(errors);

                }

                var updateContributors = await _schoolAssetsServices.UpdateContributors(request.id, request);

                if (updateContributors.Errors.Any())
                {
                    var errors = string.Join(", ", updateContributors.Errors);
                    return Result<UpdateContributorsResponse>.Failure(errors);
                }

                if (updateContributors is null || !updateContributors.IsSuccess)
                {
                    return Result<UpdateContributorsResponse>.Failure("Updates units failed");
                }

                var Display = _mapper.Map<UpdateContributorsResponse>(updateContributors.Data);
                return Result<UpdateContributorsResponse>.Success(Display);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating units by {request.id}", ex);
            }
        }
    }
}
