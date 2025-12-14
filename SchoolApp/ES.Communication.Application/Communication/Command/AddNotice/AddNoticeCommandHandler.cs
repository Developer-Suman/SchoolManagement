using AutoMapper;
using ES.Communication.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Command.AddNotice
{
    public class AddNoticeCommandHandler : IRequestHandler<AddNoticeCommand, Result<AddNoticeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly INoticeServices _noticeServices;
        private readonly IValidator<AddNoticeCommand> _validator;

        public AddNoticeCommandHandler(IMapper mapper, INoticeServices noticeServices, IValidator<AddNoticeCommand> validator)
        {
            _validator = validator;
            _noticeServices = noticeServices;
            _mapper = mapper;

        }



        public async Task<Result<AddNoticeResponse>> Handle(AddNoticeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddNoticeResponse>.Failure(errors);
                }

                var addParent = await _noticeServices.Add(request);

                if (addParent.Errors.Any())
                {
                    var errors = string.Join(", ", addParent.Errors);
                    return Result<AddNoticeResponse>.Failure(errors);
                }

                if (addParent is null || !addParent.IsSuccess)
                {
                    return Result<AddNoticeResponse>.Failure(" ");
                }

                var parentDisplay = _mapper.Map<AddNoticeResponse>(addParent.Data);
                return Result<AddNoticeResponse>.Success(parentDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Notice", ex);


            }
        }
    }
}
