using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityId;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseId
{
    public class CourseIdQueryHandler : IRequestHandler<CourseIdQuery, Result<CourseIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICourseServices _courseServices;

        public CourseIdQueryHandler(IMapper mapper, ICourseServices courseServices)
        {
            _mapper = mapper;
            _courseServices = courseServices;
        }

        public async Task<Result<CourseIdResponse>> Handle(CourseIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _courseServices.Get(request.id);
                return Result<CourseIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
