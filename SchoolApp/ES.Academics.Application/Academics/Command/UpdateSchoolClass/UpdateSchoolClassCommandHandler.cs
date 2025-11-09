using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.UpdateSchoolClass
{
    public class UpdateSchoolClassCommandHandler : IRequestHandler<UpdateSchoolClassCommand, Result<UpdateSchoolClassResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ISchoolClassInterface _schoolClassInterface;
        private readonly IValidator<UpdateSchoolClassCommand> _validator;

        public UpdateSchoolClassCommandHandler(IValidator<UpdateSchoolClassCommand> validator, ISchoolClassInterface schoolClassInterface, IMapper mapper)
        {
            _mapper = mapper;
            _schoolClassInterface = schoolClassInterface;
            _validator = validator;

        }
        public async Task<Result<UpdateSchoolClassResponse>> Handle(UpdateSchoolClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSchoolClassResponse>.Failure(errors);

                }

                var updateSchoolClass = await _schoolClassInterface.Update(request.id, request);

                if (updateSchoolClass.Errors.Any())
                {
                    var errors = string.Join(", ", updateSchoolClass.Errors);
                    return Result<UpdateSchoolClassResponse>.Failure(errors);
                }

                if (updateSchoolClass is null || !updateSchoolClass.IsSuccess)
                {
                    return Result<UpdateSchoolClassResponse>.Failure("Updates SchoolClass failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateSchoolClassResponse>(updateSchoolClass.Data);
                return Result<UpdateSchoolClassResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating Ledger by {request.id}", ex);
            }
        }
    }
}
