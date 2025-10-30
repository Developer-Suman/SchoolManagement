using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusBySchool
{
    public class UpdateItemStatusBySchoolCommandHandler : IRequestHandler<UpdateItemStatusBySchoolCommand, Result<UpdateItemStatusBySchoolResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateItemStatusBySchoolCommand> _validator;

        public UpdateItemStatusBySchoolCommandHandler(IValidator<UpdateItemStatusBySchoolCommand> validator,ISettingServices settingServices, IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;

            
        }
        public async Task<Result<UpdateItemStatusBySchoolResponse>> Handle(UpdateItemStatusBySchoolCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateItemStatusBySchoolResponse>.Failure(errors);
                }

                var expiredDateItem = await _settingServices.UpdateItemStatusBySchool(request.schoolId, request.isExpiredDate,request.isSerialNo, cancellationToken);

                if (expiredDateItem.Errors.Any())
                {
                    var errors = string.Join(", ", expiredDateItem.Errors);
                    return Result<UpdateItemStatusBySchoolResponse>.Failure(errors);
                }

                if (expiredDateItem is null || !expiredDateItem.IsSuccess)
                {
                    return Result<UpdateItemStatusBySchoolResponse>.Failure(" ");
                }

                var customerDisplays = _mapper.Map<UpdateItemStatusBySchoolResponse>(request);
                return Result<UpdateItemStatusBySchoolResponse>.Success(customerDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Customer", ex);


            }
        }
    }
}
