using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems
{
    public class UpdateSchoolItemsCommandHandler : IRequestHandler<UpdateSchoolItemsCommand, Result<UpdateSchoolItemsResponse>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSchoolItemsCommand> _validator;
        public UpdateSchoolItemsCommandHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper, IValidator<UpdateSchoolItemsCommand> validator)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateSchoolItemsResponse>> Handle(UpdateSchoolItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSchoolItemsResponse>.Failure(errors);

                }

                var updateSchoolItems = await _schoolAssetsServices.UpdateSchoolItems(request.id, request);

                if (updateSchoolItems.Errors.Any())
                {
                    var errors = string.Join(", ", updateSchoolItems.Errors);
                    return Result<UpdateSchoolItemsResponse>.Failure(errors);
                }

                if (updateSchoolItems is null || !updateSchoolItems.IsSuccess)
                {
                    return Result<UpdateSchoolItemsResponse>.Failure("Updates failed");
                }

                var Display = _mapper.Map<UpdateSchoolItemsResponse>(updateSchoolItems.Data);
                return Result<UpdateSchoolItemsResponse>.Success(Display);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating units by {request.id}", ex);
            }
        }
    }
}
