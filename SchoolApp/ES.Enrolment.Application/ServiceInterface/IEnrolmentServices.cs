using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using ES.Enrolment.Application.Enrolments.Queries.GetAllUserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.ServiceInterface
{
    public interface IEnrolmentServices
    {
        Task<Result<AddInquiryResponse>> AddInquiry(AddInquiryCommand addInquiryCommand);
        Task<Result<ConvertApplicantResponse>> ConvertToApplicant(ConvertApplicantCommand convertApplicantCommand);
        Task<Result<PagedResult<FilterInqueryResponse>>> FilterInquery(PaginationRequest paginationRequest, FilterInquiryDTOs filterInquiryDTOs);
        Task<Result<PagedResult<GetAllUserProfileResponse>>> UserProfile(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}
