using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Command.Counselor.AddCounselor;
using ES.Enrolment.Application.Enrolments.Command.Enquiry.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.Applicant;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.FilterApplicant;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.FilterConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.Counselors.Counselor;
using ES.Enrolment.Application.Enrolments.Queries.Counselors.FilterCounselor;
using ES.Enrolment.Application.Enrolments.Queries.CRMStudents.FilterCRMStudents;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.FilterInquery;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.InqueryById;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.Inquiry;
using ES.Enrolment.Application.Enrolments.Queries.FollowUp.FilterFollowUp;
using ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetAllUserProfile;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetUserProfileById;
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
            #region TrainingRegistration
            CreateMap<TrainingRegistration, AddTranningRegistrationResponse>().ReverseMap();

            CreateMap<FilterTrainingRegistrationResponse, TrainingRegistration>().ReverseMap();
            CreateMap<PagedResult<TrainingRegistration>, PagedResult<FilterTrainingRegistrationResponse>>().ReverseMap();
            #endregion

            #region FollowUps
            CreateMap<FollowUp, AddFollowUpResponse>().ReverseMap();

            CreateMap<FilterFollowUpResponse, FollowUp>().ReverseMap();
            CreateMap<PagedResult<FollowUp>, PagedResult<FilterFollowUpResponse>>().ReverseMap();

            #endregion


            #region ConsultancyClass
            CreateMap<ConsultancyClass, AddConsultancyClassResponse>().ReverseMap();

            CreateMap<ConsultancyClassResponse, ConsultancyClass>().ReverseMap();
            CreateMap<PagedResult<ConsultancyClass>, PagedResult<ConsultancyClassResponse>>().ReverseMap();


            CreateMap<FilterConsultancyClassResponse, ConsultancyClass>().ReverseMap();
            CreateMap<PagedResult<ConsultancyClass>, PagedResult<FilterConsultancyClassResponse>>().ReverseMap();
            #endregion
            #region Counselor
            CreateMap<Counselor, AddCounselorResponse>().ReverseMap();

            CreateMap<CounselorResponse, Counselor>().ReverseMap();
            CreateMap<PagedResult<Counselor>, PagedResult<CounselorResponse>>().ReverseMap();
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
            CreateMap<InquiryResponse, CrmLead>().ReverseMap();
            CreateMap<PagedResult<CrmLead>, PagedResult<InquiryResponse>>().ReverseMap();
            CreateMap<FilterInqueryResponse, CrmLead>().ReverseMap();
            CreateMap<PagedResult<CrmLead>, PagedResult<FilterInqueryResponse>>().ReverseMap();

            CreateMap<ApplicantResponse, CrmApplicant>().ReverseMap();
            CreateMap<PagedResult<CrmApplicant>, PagedResult<ApplicantResponse>>().ReverseMap();

            CreateMap<FilterApplicantResponse, CrmApplicant>().ReverseMap();
            CreateMap<PagedResult<CrmApplicant>, PagedResult<FilterApplicantResponse>>().ReverseMap();
            #endregion
            #region UserProfile
            CreateMap<UserProfile, UserProfileResponse>().ReverseMap();
            CreateMap<GetUserProfileByIdResponse, UserProfile>().ReverseMap();
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
