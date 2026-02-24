using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Entities.Crm.Lead;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region University
            CreateMap<University, AddUniversityResponse>().ReverseMap();

            CreateMap<FilterUniversityResponse, University>().ReverseMap();
            CreateMap<PagedResult<University>, PagedResult<FilterUniversityResponse>>().ReverseMap();
            #endregion

            #region Course
            CreateMap<Course, AddCourseResponse>().ReverseMap();


            CreateMap<CourseResponse, Course>().ReverseMap();
            CreateMap<PagedResult<Course>, PagedResult<CourseResponse>>().ReverseMap();

            CreateMap<FilterCourseResponse, Course>().ReverseMap();
            CreateMap<PagedResult<Course>, PagedResult<FilterCourseResponse>>().ReverseMap();

            #endregion

            #region Intake
            CreateMap<Intake, AddIntakeResponse>().ReverseMap();

            CreateMap<FilterIntakeResponse, Intake>().ReverseMap();
            CreateMap<PagedResult<Intake>, PagedResult<FilterIntakeResponse>>().ReverseMap();

            #endregion

            #region Requirements
            CreateMap<Requirement, AddRequirementsResponse>().ReverseMap();

            CreateMap<FilterRequirementsResponse, Requirement>().ReverseMap();
            CreateMap<PagedResult<Requirement>, PagedResult<FilterRequirementsResponse>>().ReverseMap();

            #endregion
        }
    }
}
