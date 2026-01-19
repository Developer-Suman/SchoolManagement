using AutoMapper;
using ES.Certificate.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.Awards.AddAwards
{
    public class AddAwardsCommandHandler : IRequestHandler<AddAwardsCommand, Result<AddAwardsResponse>>
    {
        private readonly IValidator<AddAwardsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IAwardsServices _awardsServices;

        public AddAwardsCommandHandler(IValidator<AddAwardsCommand> validator, IMapper mapper, IAwardsServices awardsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _awardsServices = awardsServices;
        }
        public async Task<Result<AddAwardsResponse>> Handle(AddAwardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddAwardsResponse>.Failure(errors);
                }

                var add = await _awardsServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddAwardsResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddAwardsResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddAwardsResponse>(add.Data);
                return Result<AddAwardsResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
