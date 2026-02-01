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

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory
{
    public class UpdateSchoolItemHistoryCommandHandler : IRequestHandler<UpdateSchoolItemHistoryCommand, Result<UpdateSchoolItemHistoryResponse>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSchoolItemHistoryCommand> _validator;
        public UpdateSchoolItemHistoryCommandHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper, IValidator<UpdateSchoolItemHistoryCommand> validator)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<Result<UpdateSchoolItemHistoryResponse>> Handle(UpdateSchoolItemHistoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSchoolItemHistoryResponse>.Failure(errors);

                }

                var updateSchoolItemHistory = await _schoolAssetsServices.UpdateSchoolItemHistory(request.id, request);

                if (updateSchoolItemHistory.Errors.Any())
                {
                    var errors = string.Join(", ", updateSchoolItemHistory.Errors);
                    return Result<UpdateSchoolItemHistoryResponse>.Failure(errors);
                }

                if (updateSchoolItemHistory is null || !updateSchoolItemHistory.IsSuccess)
                {
                    return Result<UpdateSchoolItemHistoryResponse>.Failure("Updates units failed");
                }

                var Display = _mapper.Map<UpdateSchoolItemHistoryResponse>(updateSchoolItemHistory.Data);
                return Result<UpdateSchoolItemHistoryResponse>.Success(Display);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating units by {request.id}", ex);
            }
        }
    }
}
