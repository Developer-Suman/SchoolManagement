using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Entities.Crm.Lead;
using TN.Shared.Domain.Entities.Crm.Students;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Lead and Applicant
            CreateMap<CrmLead, AddInquiryResponse>().ReverseMap();
            CreateMap<CrmStudent, ConvertStudentResponse>().ReverseMap();
            CreateMap<CrmApplicant, ConvertApplicantResponse>().ReverseMap();
            CreateMap<FilterInqueryResponse, CrmLead>().ReverseMap();
            CreateMap<PagedResult<CrmLead>, PagedResult<FilterInqueryResponse>>().ReverseMap();
            #endregion
        }
    }
}
