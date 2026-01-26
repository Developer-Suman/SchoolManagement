using AutoMapper;
using ES.Academics.Application.Academics.Command.AddAssignments;
using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.AddExamSession;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.Academics.Command.DeleteExam;
using ES.Academics.Application.Academics.Command.DeleteExamResult;
using ES.Academics.Application.Academics.Command.DeleteSchoolClass;
using ES.Academics.Application.Academics.Command.DeleteSubject;
using ES.Academics.Application.Academics.Command.EvaluteAssignments;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.Academics.Command.SubmitAssignments;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Queries.ClassByExamSession;
using ES.Academics.Application.Academics.Queries.ClassWithSubject;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.ExamResult;
using ES.Academics.Application.Academics.Queries.ExamResultById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterExamResult;
using ES.Academics.Application.Academics.Queries.FilterExamSession;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.Academics.Queries.GetAssignments;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using ES.Academics.Application.Academics.Queries.Subject;
using ES.Academics.Application.Academics.Queries.SubjectByClassId;
using ES.Academics.Application.Academics.Queries.SubjectById;
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
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PagedResult<Class>, PagedResult<ClassByExamSessionResponse>>().ReverseMap();


            #region Events
            CreateMap<Events, UpdateEventsCommand>().ReverseMap();
            CreateMap<Events, AddEventsResponse>().ReverseMap();


            #endregion
            #region Assignments
            CreateMap<Assignment, AddAssignmentsResponse>().ReverseMap();

            #endregion
            #region Assignment

            CreateMap<GetAssignmentsResponse, Assignment>().ReverseMap();
            CreateMap<PagedResult<Assignment>, PagedResult<GetAssignmentsResponse>>().ReverseMap();


            CreateMap<AssignmentStudent, AddAssignmentStudentsResponse>().ReverseMap();
            CreateMap<AssignmentStudent, EvaluteAssignmentsResponse>().ReverseMap();
            CreateMap<SubmitAssignmentsResponse, AssignmentStudent>().ReverseMap();
            #endregion
            #region ExamSession
            CreateMap<ExamSession, AddExamSessionResponse>().ReverseMap();
            CreateMap<FilterExamSessionResponse, ExamSession>().ReverseMap();
            CreateMap<PagedResult<ExamSession>, PagedResult<FilterExamSessionResponse>>().ReverseMap();
            #endregion

            #region SchoolClass

            CreateMap<ClassWithSubjectResponse, AcademicTeamClass>().ReverseMap();
            CreateMap<PagedResult<AcademicTeamClass>, PagedResult<ClassWithSubjectResponse>>().ReverseMap();

            CreateMap<Class, SchoolClassQueryResponse>()
                .ForMember(dest => dest.id, 
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.subjects,
                    opt => opt.MapFrom(src => src.Subjects));

            CreateMap<Subject, SubjectDTOs>();



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

            #region ExamResult

            CreateMap<ExamResult, DeleteExamResultCommand>().ReverseMap();
            CreateMap<Subject, SubjectByClassIdResponse>().ReverseMap();

            CreateMap<ExamResultByIdQuery, ExamResult>().ReverseMap();
            CreateMap<MarksObtainedDTOs, MarksObtained>().ReverseMap();

            CreateMap<ExamResultByIdResponse, ExamResult>().ReverseMap();

            CreateMap<ExamResultResponse, ExamResult>().ReverseMap();
            CreateMap<AddExamResultCommand, ExamResult>().ReverseMap();
            CreateMap<AddExamResultResponse, ExamResult>().ReverseMap();
            CreateMap<AddExamResultCommand, AddExamResultResponse>().ReverseMap();

            CreateMap<PagedResult<ExamResult>, PagedResult<ExamResultResponse>>().ReverseMap();

            CreateMap<FilterExamResultResponse, ExamResult>().ReverseMap();
            CreateMap<PagedResult<ExamResult>, PagedResult<FilterExamResultResponse>>().ReverseMap();
            #endregion

            #region Subject
            CreateMap<Subject, SubjectResponse>().ReverseMap();
            CreateMap<Subject, DeleteSubjectCommand>().ReverseMap();

            CreateMap<SubjectByIdQuery, Subject>().ReverseMap();

            CreateMap<SubjectByIdResponse, Subject>().ReverseMap();

            CreateMap<Subject, AddSubjectResponse>().ReverseMap();



            CreateMap<AddSubjectCommand, Subject>().ReverseMap();
            CreateMap<AddSubjectResponse, Subject>().ReverseMap();
            CreateMap<AddSubjectCommand, AddSubjectResponse>().ReverseMap();

            

            CreateMap<PagedResult<Subject>, PagedResult<SubjectResponse>>().ReverseMap();

            CreateMap<FilterSubjectResponse, Subject>().ReverseMap();
            CreateMap<PagedResult<Subject>, PagedResult<FilterSubjectResponse>>().ReverseMap();
            #endregion

        }
    }
}
