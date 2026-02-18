using AutoMapper;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using ZXing;

namespace ES.Academics.Application.Academics.Command.ClosedAcademicYear
{
    public class ClosedAcademicYearCommandHandler : IRequestHandler<ClosedAcademicYearCommand, Result<ClosedAcademicYearResponse>>
    {
        private readonly IValidator<ClosedAcademicYearCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IStudentsPromotion _studentsPromotion;

        public ClosedAcademicYearCommandHandler(IValidator<ClosedAcademicYearCommand> validator, IStudentsPromotion studentsPromotion, IMapper mapper)
        {
            _mapper = mapper;
            _validator = validator;
            _studentsPromotion = studentsPromotion;
        }

        public async Task<Result<ClosedAcademicYearResponse>> Handle(ClosedAcademicYearCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<ClosedAcademicYearResponse>.Failure(errors);
                }

                var closedAcademicYear = await _studentsPromotion.CloseAcademicYear(request);

                if (closedAcademicYear.Errors.Any())
                {
                    var errors = string.Join(", ", closedAcademicYear.Errors);
                    return Result<ClosedAcademicYearResponse>.Failure(errors);
                }

                if (closedAcademicYear is null || !closedAcademicYear.IsSuccess)
                {
                    return Result<ClosedAcademicYearResponse>.Failure(" ");
                }

                var closedAcademicYearDisplay = _mapper.Map<ClosedAcademicYearResponse>(closedAcademicYear.Data);
                return Result<ClosedAcademicYearResponse>.Success(closedAcademicYearDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Closing", ex);


            }
        }
    }
}
