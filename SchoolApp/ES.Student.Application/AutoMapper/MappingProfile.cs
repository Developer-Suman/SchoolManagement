using AutoMapper;
using ES.Student.Application.Student.Command.AddParent;
using ES.Student.Application.Student.Command.AddStudents;
using ES.Student.Application.Student.Command.UpdateParent;
using ES.Student.Application.Student.Command.UpdateStudents;
using ES.Student.Application.Student.Queries.FilterParents;
using ES.Student.Application.Student.Queries.FilterStudents;
using ES.Student.Application.Student.Queries.GetAllParent;
using ES.Student.Application.Student.Queries.GetAllStudents;
using ES.Student.Application.Student.Queries.GetParentById;
using ES.Student.Application.Student.Queries.GetStudentsById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region Student
            CreateMap<AddStudentsResponse, StudentData>().ReverseMap();
            CreateMap<AddStudentsCommand, StudentData>().ReverseMap();

            CreateMap<GetAllStudentQueryResponse, StudentData>().ReverseMap();
            CreateMap<PagedResult<StudentData>, PagedResult<GetAllStudentQueryResponse>>().ReverseMap();

            CreateMap<GetStudentsByIdQueryResponse, StudentData>().ReverseMap();

            CreateMap<StudentData, UpdateStudentCommand>().ReverseMap();

            CreateMap<FilterStudentsResponse, StudentData>().ReverseMap();
            CreateMap<PagedResult<StudentData>, PagedResult<FilterStudentsResponse>>().ReverseMap();
            #endregion

            #region Parent
            CreateMap<GetAllParentQueryResponse, Parent>().ReverseMap();
            CreateMap<AddParentResponse, Parent>().ReverseMap();
            CreateMap<PagedResult<Parent>, PagedResult<GetAllParentQueryResponse>>().ReverseMap();
            CreateMap<GetParentByIdQueryResponse, Parent>().ReverseMap();
            CreateMap<AddParentResponse, Parent>().ReverseMap();
            CreateMap<AddParentCommand, Parent>().ReverseMap();
            CreateMap<UpdateParentResponse, Parent>().ReverseMap();
            CreateMap<UpdateParentCommand, Parent>().ReverseMap();


            CreateMap<FilterParentsResponse, Parent>().ReverseMap();
            CreateMap<PagedResult<Parent>, PagedResult<FilterParentsResponse>>().ReverseMap();
            #endregion
        }
    }
}
