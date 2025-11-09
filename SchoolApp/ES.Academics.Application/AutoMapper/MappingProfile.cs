using AutoMapper;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.DeleteExam;
using ES.Academics.Application.Academics.Command.DeleteSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.DeleteLedger;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Domain.Entities;
using TN.Setup.Application.Setup.Queries.SchoolById;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region SchoolClass

            CreateMap<Class, DeleteSchoolClassCommand>().ReverseMap();

            CreateMap<SchoolClassByIdQuery, Class>().ReverseMap();

            CreateMap<SchoolClassByIdResponse, Class>().ReverseMap();


            CreateMap<UpdateSchoolClassResponse, Class>().ReverseMap();
            CreateMap<UpdateSchoolClassCommand, Class>().ReverseMap();
            CreateMap<UpdateSchoolClassCommand, UpdateSchoolClassResponse>().ReverseMap();


            CreateMap<SchoolClassQueryResponse, Class>().ReverseMap();
            CreateMap<AddSchoolClassCommand, Class>().ReverseMap();
            CreateMap<AddSchoolClassResponse, Class>().ReverseMap();
            CreateMap<AddSchoolClassCommand, AddSchoolClassResponse>().ReverseMap();

            CreateMap<PagedResult<Class>, PagedResult<SchoolClassQueryResponse>>().ReverseMap();

            CreateMap<FilterSchoolClassQueryResponse, Class>().ReverseMap();
            CreateMap<PagedResult<Class>, PagedResult<FilterSchoolClassQueryResponse>>().ReverseMap();
            #endregion

            #region Exam

            CreateMap<Exam, DeleteExamCommand>().ReverseMap();

            CreateMap<ExamByIdQuery, Exam>().ReverseMap();

            CreateMap<ExamByIdQueryResponse, Exam>().ReverseMap();

            CreateMap<ExamQueryResponse, Exam>().ReverseMap();
            CreateMap<AddExamCommand, Exam>().ReverseMap();
            CreateMap<AddExamResponse, Exam>().ReverseMap();
            CreateMap<AddExamCommand, AddExamResponse>().ReverseMap();

            CreateMap<PagedResult<Exam>, PagedResult<ExamQueryResponse>>().ReverseMap();

            CreateMap<FilterExamResponse, Exam>().ReverseMap();
            CreateMap<PagedResult<Exam>, PagedResult<FilterExamResponse>>().ReverseMap();
            #endregion

        }
    }
}
