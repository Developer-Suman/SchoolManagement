using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Communication.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Command.UpdateNotice
{
    public class UpdateNoticeCommandHandler : IRequestHandler<UpdateNoticeCommand, Result<UpdateNoticeResponse>>
    {
        private readonly IValidator<UpdateNoticeCommand> _validator;
        public readonly IMapper _mapper;
        private readonly  INoticeServices _noticeServices;
        public UpdateNoticeCommandHandler(IValidator<UpdateNoticeCommand> validator, IMapper mapper, INoticeServices noticeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _noticeServices = noticeServices;

        }
        public async Task<Result<UpdateNoticeResponse>> Handle(UpdateNoticeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateNoticeResponse>.Failure(errors);

                }

                var updateNotice = await _noticeServices.Update(request.id, request);

                if (updateNotice.Errors.Any())
                {
                    var errors = string.Join(", ", updateNotice.Errors);
                    return Result<UpdateNoticeResponse>.Failure(errors);
                }

                if (updateNotice is null || !updateNotice.IsSuccess)
                {
                    return Result<UpdateNoticeResponse>.Failure("Updates Notice failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateNoticeResponse>(updateNotice.Data);
                return Result<UpdateNoticeResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Notice ", ex);
            }
        }
    }
}
