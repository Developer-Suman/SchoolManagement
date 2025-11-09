using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationBySchool;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany
{
    public class UpdateBillNumberGenerationBySchoolCommandHandler : IRequestHandler<UpdateBillNumberGenerationBySchoolCommand, Result<UpdateBillNumberGenerationBySchoolResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBillNumberGenerationBySchoolCommand> _validator;

        public UpdateBillNumberGenerationBySchoolCommandHandler(ISalesDetailsServices salesDetailsServices, IMapper mapper, IValidator<UpdateBillNumberGenerationBySchoolCommand> validator)
        {
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateBillNumberGenerationBySchoolResponse>> Handle(UpdateBillNumberGenerationBySchoolCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateBillNumberGenerationBySchoolResponse>.Failure(errors);
                }

                var updateBillNumberStatusBySchool = await _salesDetailsServices.UpdateBillNumberStatusBySchool(request);

                if (updateBillNumberStatusBySchool.Errors.Any())
                {
                    var errors = string.Join(", ", updateBillNumberStatusBySchool.Errors);
                    return Result<UpdateBillNumberGenerationBySchoolResponse>.Failure(errors);
                }

                if (updateBillNumberStatusBySchool is null || !updateBillNumberStatusBySchool.IsSuccess)
                {
                    return Result<UpdateBillNumberGenerationBySchoolResponse>.Failure(" ");
                }
                var updateBillNumberGeneratorResponse = _mapper.Map<UpdateBillNumberGenerationBySchoolResponse>(updateBillNumberStatusBySchool.Data);
                return Result<UpdateBillNumberGenerationBySchoolResponse>.Success(updateBillNumberGeneratorResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding sales details", ex);
            }
        }
    }
}
