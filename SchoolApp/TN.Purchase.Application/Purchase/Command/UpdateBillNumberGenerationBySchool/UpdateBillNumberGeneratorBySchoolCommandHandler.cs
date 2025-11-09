using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationBySchool;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany
{
    public class UpdateBillNumberGeneratorBySchoolCommandHandler : IRequestHandler<UpdateBillNumberGeneratorBySchoolCommand, Result<UpdateBillNumberGeneratorBySchoolResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBillNumberGeneratorBySchoolCommand> _validator;

        public UpdateBillNumberGeneratorBySchoolCommandHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper, IValidator<UpdateBillNumberGeneratorBySchoolCommand> validator)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<UpdateBillNumberGeneratorBySchoolResponse>> Handle(UpdateBillNumberGeneratorBySchoolCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateBillNumberGeneratorBySchoolResponse>.Failure(errors);
                }

                var updateBillNumberStatusByCompany = await _purchaseDetailsServices.UpdateBillNumberStatusByCompany(request);

                if (updateBillNumberStatusByCompany.Errors.Any())
                {
                    var errors = string.Join(", ", updateBillNumberStatusByCompany.Errors);
                    return Result<UpdateBillNumberGeneratorBySchoolResponse>.Failure(errors);
                }

                if (updateBillNumberStatusByCompany is null || !updateBillNumberStatusByCompany.IsSuccess)
                {
                    return Result<UpdateBillNumberGeneratorBySchoolResponse>.Failure(" ");
                }
                var updateBillNumberGeneratorResponse = _mapper.Map<UpdateBillNumberGeneratorBySchoolResponse>(updateBillNumberStatusByCompany.Data);
                return Result<UpdateBillNumberGeneratorBySchoolResponse>.Success(updateBillNumberGeneratorResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding purchaseDetails", ex);
            }
        }
    }
}
