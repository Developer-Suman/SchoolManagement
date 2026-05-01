using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication;
using ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus;
using ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory;
using ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus;
using ES.Visa.Application.Visa.Queries.VisaStatus.VisaStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.ServiceInterface
{
    public interface IVisaServices
    {
        Task<Result<AddVisaApplicationResponse>> AddVisa(AddVisaApplicationCommand addVisaApplicationCommand);
        Task<Result<AddVisaStatusResponse>> AddVisaStatus(AddVisaStatusCommand addVisaStatusCommand);

        Task<Result<VisaApplicationResponse>> GetVisaApplication(string visaApplicationId, CancellationToken cancellationToken = default);
        Task<Result<VisaStatusQueryResponse>> GetVisaStatus(string visaStatusId, CancellationToken cancellationToken = default);
        Task<Result<UpdateVisaApplicationResponse>> UpdateVisaApplication(string visaApplicationId, UpdateVisaApplicationCommand updateVisaApplicationCommand);
        //Task<Result<PagedResult<EventsResponse>>> GetAllEvents(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        //Task<Result<EventsByIdResponse>> GetEvents(string eventsId, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteVisaApplication(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterVisaApplicationResponse>>> GetFilterVisaApplication(PaginationRequest paginationRequest, FilterVisaApplicationDTOs filterVisaApplicationDTOs);
        Task<Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>> GetFilterVisaApplicationStatusHistory(PaginationRequest paginationRequest, FilterVisaApplicationStatusHistoryDTOs filterVisaApplicationStatusHistoryDTOs);
        Task<Result<PagedResult<FilterVisaStatusResponse>>> GetFilterVisaStatus(PaginationRequest paginationRequest, FilterVisaStatusDTOs filterVisaStatusDTOs);
    }
}
