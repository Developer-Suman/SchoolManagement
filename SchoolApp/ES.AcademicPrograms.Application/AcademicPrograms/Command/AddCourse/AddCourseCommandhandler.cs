using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse
{
    public class AddCourseCommandhandler : IRequestHandler<AddCourseCommand, Result<AddCourseResponse>>
    {

        private readonly IValidator<AddCourseCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ICourseServices _courseServices;


        public AddCourseCommandhandler(IValidator<AddCourseCommand> validator, IMapper mapper, ICourseServices courseServices)
        {
            _validator = validator;
            _mapper = mapper;
            _courseServices = courseServices;
        }
        public async Task<Result<AddCourseResponse>> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddCourseResponse>.Failure(errors);
                }

                var add = await _courseServices.AddCourse(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddCourseResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddCourseResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddCourseResponse>(add.Data);
                return Result<AddCourseResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
