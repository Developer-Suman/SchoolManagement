using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.AddCounselor;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Queries.FilterApplicant;
using ES.Enrolment.Application.Enrolments.Queries.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.FilterCounselor;
using ES.Enrolment.Application.Enrolments.Queries.FilterCRMStudents;
using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using ES.Enrolment.Application.Enrolments.Queries.GetAllUserProfile;
using ES.Enrolment.Application.Enrolments.Queries.GetUserProfileByUser;
using ES.Enrolment.Application.Enrolments.Queries.InqueryById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.Crm.Lead;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.Entities.Crm.Students;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Counselor
            CreateMap<Counselor, AddCounselorResponse>().ReverseMap();

            CreateMap<FilterCounselorResponse, Counselor>().ReverseMap();
            CreateMap<PagedResult<Counselor>, PagedResult<FilterCounselorResponse>>().ReverseMap();
            #endregion

            #region Appointment
            CreateMap<Appointment, AddAppointmentResponse>().ReverseMap();

            CreateMap<FilterAppointmentResponse, Appointment>().ReverseMap();
            CreateMap<PagedResult<Appointment>, PagedResult<FilterAppointmentResponse>>().ReverseMap();

            #endregion
            #region Lead and Applicant
            CreateMap<CrmLead, AddInquiryResponse>().ReverseMap();
            CreateMap<CrmStudent, ConvertStudentResponse>().ReverseMap();
            CreateMap<CrmApplicant, ConvertApplicantResponse>().ReverseMap();
            CreateMap<FilterInqueryResponse, CrmLead>().ReverseMap();
            CreateMap<PagedResult<CrmLead>, PagedResult<FilterInqueryResponse>>().ReverseMap();

            CreateMap<FilterApplicantResponse, CrmApplicant>().ReverseMap();
            CreateMap<PagedResult<CrmApplicant>, PagedResult<FilterApplicantResponse>>().ReverseMap();
            #endregion
            #region UserProfile
            CreateMap<UserProfile, UserProfileResponse>().ReverseMap();
            CreateMap<GetUserProfileByUserResponse, UserProfile>().ReverseMap();
            CreateMap<GetAllUserProfileResponse, UserProfile>().ReverseMap();
            CreateMap<PagedResult<UserProfile>, PagedResult<GetAllUserProfileResponse>>().ReverseMap();
            #endregion
            #region CRM Students

            CreateMap<FilterCRMStudentsResponse, CrmStudent>().ReverseMap();
            CreateMap<PagedResult<CrmStudent>, PagedResult<FilterCRMStudentsResponse>>().ReverseMap();

            #endregion
        }
    }
}
