using AutoMapper;
using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication;
using ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus;
using ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory;
using ES.Visa.Application.Visa.Queries.VisaStatus;
using ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region FilterVisaApplicationStatusHistory


            CreateMap<FilterVisaApplicationStatusHistoryResponse, VisaApplicationStatusHistory>().ReverseMap();
            CreateMap<PagedResult<VisaApplicationStatusHistory>, PagedResult<FilterVisaApplicationStatusHistoryResponse>>().ReverseMap();

            #endregion


            #region VisaStatus
            CreateMap<AddVisaStatusCommand, VisaStatus>().ReverseMap();
            CreateMap<AddVisaStatusResponse, VisaStatus>().ReverseMap();


            CreateMap<FilterVisaStatusResponse, VisaStatus>().ReverseMap();
            CreateMap<PagedResult<VisaStatus>, PagedResult<FilterVisaStatusResponse>>().ReverseMap();

            #endregion


            #region VisaApplication
            CreateMap<AddVisaApplicationCommand, VisaApplication>().ReverseMap();
            CreateMap<AddVisaApplicationResponse, VisaApplication>().ReverseMap();


            CreateMap<FilterVisaApplicationResponse, VisaApplication>().ReverseMap();
            CreateMap<PagedResult<VisaApplication>, PagedResult<FilterVisaApplicationResponse>>().ReverseMap();

            CreateMap<VisaApplication, UpdateVisaApplicationCommand>().ReverseMap();
            //CreateMap<ExamResult, DeleteExamResultCommand>().ReverseMap();
            //CreateMap<Subject, SubjectByClassIdResponse>().ReverseMap();

            //CreateMap<ExamResultByIdQuery, ExamResult>().ReverseMap();
            CreateMap<UpdateVisaApplicationResponseDTOs, VisaApplicationDocument>().ReverseMap();

            CreateMap<VisaApplicationResponse, VisaApplication>().ReverseMap();

            //CreateMap<ExamResultResponse, ExamResult>().ReverseMap();

            //CreateMap<AddExamResultCommand, AddExamResultResponse>().ReverseMap();

            //CreateMap<PagedResult<ExamResult>, PagedResult<ExamResultResponse>>().ReverseMap();

            //CreateMap<FilterExamResultResponse, ExamResult>().ReverseMap();
            //CreateMap<PagedResult<ExamResult>, PagedResult<FilterExamResultResponse>>().ReverseMap();
            #endregion
        }
    }
}
