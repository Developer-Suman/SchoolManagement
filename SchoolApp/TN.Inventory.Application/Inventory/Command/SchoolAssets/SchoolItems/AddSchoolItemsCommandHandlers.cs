using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems
{
    public class AddSchoolItemsCommandHandlers : IRequestHandler<AddSchoolItemsCommand, Result<AddSchoolItemsResponse>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddSchoolItemsCommand> _validator;

        public AddSchoolItemsCommandHandlers(ISchoolAssetsServices schoolAssetsServices, IMapper mapper, IValidator<AddSchoolItemsCommand> validator)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<AddSchoolItemsResponse>> Handle(AddSchoolItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSchoolItemsResponse>.Failure(errors);
                }

                var addSchoolItems = await _schoolAssetsServices.AddSchoolItems(request);

                if (addSchoolItems.Errors.Any())
                {
                    var errors = string.Join(", ", addSchoolItems.Errors);
                    return Result<AddSchoolItemsResponse>.Failure(errors);
                }

                if (addSchoolItems is null || !addSchoolItems.IsSuccess)
                {
                    return Result<AddSchoolItemsResponse>.Failure(" ");
                }

                var schoolItemsDisplay = _mapper.Map<AddSchoolItemsResponse>(addSchoolItems.Data);
                return Result<AddSchoolItemsResponse>.Success(schoolItemsDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding SchoolItems", ex);


            }
        }
    }
}
