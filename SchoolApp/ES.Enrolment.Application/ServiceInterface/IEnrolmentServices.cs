using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Queries.ApplicantsById;
using ES.Enrolment.Application.Enrolments.Queries.CRMStudentsById;
using ES.Enrolment.Application.Enrolments.Queries.FilterApplicant;
using ES.Enrolment.Application.Enrolments.Queries.FilterCRMStudents;
using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using ES.Enrolment.Application.Enrolments.Queries.GetAllUserProfile;
using ES.Enrolment.Application.Enrolments.Queries.GetUserProfileByUser;
using ES.Enrolment.Application.Enrolments.Queries.InqueryById;
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

        Task<Result<GetUserProfileByUserResponse>> GetUserProfile(string userId, CancellationToken cancellationToken = default);
        Task<Result<ApplicantsByIdResponse>> GetApplicants(string id, CancellationToken cancellationToken = default);
        Task<Result<CRMStudentsByIdResponse>> GetCRMStudents(string id, CancellationToken cancellationToken = default);
        Task<Result<InqueryByIdResponse>> GetInquiry(string id, CancellationToken cancellationToken = default);

    }
}
