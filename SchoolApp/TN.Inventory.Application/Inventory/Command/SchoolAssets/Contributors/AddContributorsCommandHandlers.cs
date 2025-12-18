using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors
{
    public class AddContributorsCommandHandlers : IRequestHandler<AddContributorsCommand, Result<AddContributorsResponse>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddContributorsCommand> _validator;

        public AddContributorsCommandHandlers(ISchoolAssetsServices schoolAssetsServices, IMapper mapper, IValidator<AddContributorsCommand> validator)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<AddContributorsResponse>> Handle(AddContributorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddContributorsResponse>.Failure(errors);
                }

                var addContributors = await _schoolAssetsServices.AddContributors(request);

                if (addContributors.Errors.Any())
                {
                    var errors = string.Join(", ", addContributors.Errors);
                    return Result<AddContributorsResponse>.Failure(errors);
                }

                if (addContributors is null || !addContributors.IsSuccess)
                {
                    return Result<AddContributorsResponse>.Failure(" ");
                }

                var contributorsDisplay = _mapper.Map<AddContributorsResponse>(addContributors.Data);
                return Result<AddContributorsResponse>.Success(contributorsDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Contributors", ex);


            }
        }
    }
}
