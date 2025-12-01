using AutoMapper;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddExamSession
{
    public class AddExamSessionCommandHandler : IRequestHandler<AddExamSessionCommand, Result<AddExamSessionResponse>>
    {
        private readonly IValidator<AddExamSessionCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISeatPlanningServices _seatPlanningServices;
        public AddExamSessionCommandHandler(IValidator<AddExamSessionCommand> validator, IMapper mapper, ISeatPlanningServices seatPlanningServices)
        {
            _seatPlanningServices = seatPlanningServices;
            _validator = validator;
            _mapper = mapper;
            
        }
        public async Task<Result<AddExamSessionResponse>> Handle(AddExamSessionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddExamSessionResponse>.Failure(errors);
                }

                var addExamSession = await _seatPlanningServices.AddExamSession(request);

                if (addExamSession.Errors.Any())
                {
                    var errors = string.Join(", ", addExamSession.Errors);
                    return Result<AddExamSessionResponse>.Failure(errors);
                }

                if (addExamSession is null || !addExamSession.IsSuccess)
                {
                    return Result<AddExamSessionResponse>.Failure(" ");
                }

                var addExamSessionDetails = _mapper.Map<AddExamSessionResponse>(addExamSession.Data);
                return Result<AddExamSessionResponse>.Success(addExamSessionDetails);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Exam Session", ex);


            }
        }
    }
}
