using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool
{
    public  class UpdateJournalRefBySchoolCommandHandler:IRequestHandler<UpdateJournalRefBySchoolCommand, Result<UpdateJournalRefBySchoolResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateJournalRefBySchoolCommand> _validator;

        public UpdateJournalRefBySchoolCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateJournalRefBySchoolCommand> validator)
        {
            _settingServices=settingServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateJournalRefBySchoolResponse>> Handle(UpdateJournalRefBySchoolCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateJournalRefBySchoolResponse>.Failure(errors);
                }

                var journalRef = await _settingServices.UpdateJournalRefBySchool( request.schoolId, request.journalReferences, cancellationToken);

                if (journalRef.Errors.Any())
                {
                    var errors = string.Join(", ", journalRef.Errors);
                    return Result<UpdateJournalRefBySchoolResponse>.Failure(errors);
                }

                if (journalRef is null || !journalRef.IsSuccess)
                {
                    return Result<UpdateJournalRefBySchoolResponse>.Failure(" ");
                }

                var journalRefDisplay = _mapper.Map<UpdateJournalRefBySchoolResponse>(request);
                return Result<UpdateJournalRefBySchoolResponse>.Success(journalRefDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating journal reference number", ex);


            }
        }
    }
}
