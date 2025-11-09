
using TN.Setup.Application.Setup.Command.AddSchool;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase;
using TN.Setup.Application.Setup.Command.UpdateSchool;
using TN.Setup.Application.Setup.Queries.FilterSchoolByDate;
using TN.Setup.Application.Setup.Queries.GetSchoolDetailsBySchoolId;
using TN.Setup.Application.Setup.Queries.School;
using TN.Setup.Application.Setup.Queries.SchoolById;
using TN.Setup.Application.Setup.Queries.SchoolByInstitutionId;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using static TN.Authentication.Domain.Entities.School;


namespace TN.Setup.Application.ServiceInterface
{
    public interface ISchoolServices
    {
        Task<Result<PagedResult<GetAllSchoolQueryResponse>>> GetAllSchool(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AddSchoolResponse>> Add(AddSchoolCommand addSchoolCommand);
        Task<Result<UpdateSchoolResponse>> Update(string schoolId, UpdateSchoolCommand updateSchoolCommand);
        Task<Result<GetSchoolByIdResponse>> GetSchoolById(string schoolId, CancellationToken cancellationToken = default);
        Task<Result<List<GetSchoolByInstitutionIdResponse>>> GetSchoolByInstitutionId(string institutionId, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<UpdateBillNumberStatusForPurchaseResponse>> Update(string id, BillNumberGenerationType type);
        Task<Result<IEnumerable<FilterSchoolByDateQueryResponse>>> GetSchoolFilter(FilterSchoolDTOs filterSchoolDTOs , CancellationToken cancellationToken);
        Task<Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>> GetSchoolDetailsByInstitutionId(string institutionId, CancellationToken cancellationToken = default);
        

    
    }
}
