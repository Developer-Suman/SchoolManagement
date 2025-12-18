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

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory
{
    public class AddSchoolItemHistoryCommandhandler : IRequestHandler<AddSchoolItemHistoryCommand, Result<AddSchoolItemHistoryResponse>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddSchoolItemHistoryCommand> _validator;

        public AddSchoolItemHistoryCommandhandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper, IValidator<AddSchoolItemHistoryCommand> validator)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<AddSchoolItemHistoryResponse>> Handle(AddSchoolItemHistoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSchoolItemHistoryResponse>.Failure(errors);
                }

                var addSchoolItemsHistory = await _schoolAssetsServices.AddSchoolItemHistory(request);

                if (addSchoolItemsHistory.Errors.Any())
                {
                    var errors = string.Join(", ", addSchoolItemsHistory.Errors);
                    return Result<AddSchoolItemHistoryResponse>.Failure(errors);
                }

                if (addSchoolItemsHistory is null || !addSchoolItemsHistory.IsSuccess)
                {
                    return Result<AddSchoolItemHistoryResponse>.Failure(" ");
                }

                var schoolItemsDisplay = _mapper.Map<AddSchoolItemHistoryResponse>(addSchoolItemsHistory.Data);
                return Result<AddSchoolItemHistoryResponse>.Success(schoolItemsDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding SchoolItemHistory", ex);


            }
        }
    }
}
