using AutoMapper;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards;
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
    public class AddAwardsCommandHandler : IRequestHandler<AddSchoolAwardsCommand, Result<AddSchoolAwardsResponse>>
    {
        private readonly IValidator<AddSchoolAwardsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISchoolAwardsServices _schoolAwardsServices;

        public AddAwardsCommandHandler(IValidator<AddSchoolAwardsCommand> validator, IMapper mapper, ISchoolAwardsServices schoolAwardsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _schoolAwardsServices = schoolAwardsServices;
        }
        public async Task<Result<AddSchoolAwardsResponse>> Handle(AddSchoolAwardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSchoolAwardsResponse>.Failure(errors);
                }

                var add = await _schoolAwardsServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddSchoolAwardsResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddSchoolAwardsResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddSchoolAwardsResponse>(add.Data);
                return Result<AddSchoolAwardsResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
