using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Command.Enquiry.AddInquiry;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.Applicant;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.ApplicantsById;
using ES.Enrolment.Application.Enrolments.Queries.Applicants.FilterApplicant;
using ES.Enrolment.Application.Enrolments.Queries.CRMStudents.CRMStudentsById;
using ES.Enrolment.Application.Enrolments.Queries.CRMStudents.FilterCRMStudents;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.FilterInquery;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.InqueryById;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.Inquiry;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetAllUserProfile;
using ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetUserProfileById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.ServiceInterface
{
    public interface IEnrolmentServices
    {
        Task<Result<AddInquiryResponse>> AddInquiry(AddInquiryCommand addInquiryCommand);
        Task<Result<ConvertApplicantResponse>> ConvertToApplicant(ConvertApplicantCommand convertApplicantCommand);
        Task<Result<ConvertStudentResponse>> ConvertToStudents(ConvertStudentCommand convertStudentCommand);
        Task<Result<PagedResult<FilterInqueryResponse>>> FilterInquery(PaginationRequest paginationRequest, FilterInquiryDTOs filterInquiryDTOs);
        Task<Result<PagedResult<FilterApplicantResponse>>> FilterApplicant(PaginationRequest paginationRequest, FilterApplicantDTOs filterApplicantDTOs);
        Task<Result<PagedResult<FilterCRMStudentsResponse>>> FilterCRMStudents(PaginationRequest paginationRequest, FilterCRMStudentsDTOs filterCRMStudentsDTOs);
        Task<Result<PagedResult<GetAllUserProfileResponse>>> UserProfile(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<InquiryResponse>>> GetAllInquiry(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ApplicantResponse>>> GetAllApplicant(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);

        Task<Result<GetUserProfileByIdResponse>> GetUserProfile(string userId, CancellationToken cancellationToken = default);
        Task<Result<ApplicantsByIdResponse>> GetApplicants(string id, CancellationToken cancellationToken = default);
        Task<Result<CRMStudentsByIdResponse>> GetCRMStudents(string id, CancellationToken cancellationToken = default);
        Task<Result<InqueryByIdResponse>> GetInquiry(string id, CancellationToken cancellationToken = default);

    }
}
