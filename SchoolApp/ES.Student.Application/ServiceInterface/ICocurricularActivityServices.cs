using ES.Student.Application.CocurricularActivities.Command.AddActivity;
using ES.Student.Application.CocurricularActivities.Command.Addparticipation;
using ES.Student.Application.CocurricularActivities.Queries.Activity;
using ES.Student.Application.CocurricularActivities.Queries.FilterActivity;
using ES.Student.Application.CocurricularActivities.Queries.FilterParticipation;
using ES.Student.Application.Student.Queries.ActivityByEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.ServiceInterface
{
    public interface ICocurricularActivityServices
    {
        Task<Result<AddActivityResponse>> AddActivity(AddActivityCommand addActivityCommand);
        Task<Result<AddParticipationResponse>> AddParticipation(AddParticipationCommand addParticipationCommand);
        Task<Result<PagedResult<FilterActivityResponse>>> FilterActivity(PaginationRequest paginationRequest, FilterActivityDTOs filterActivityDTOs);
        Task<Result<PagedResult<FilterParticipationResponse>>> FilterParticipation(PaginationRequest paginationRequest, FilterParticipationDTOs filterParticipationDTOs);
        Task<Result<PagedResult<ActivityResponse>>> AllActivity(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ActivityByEventsResponse>>> ActivityByEvents(PaginationRequest paginationRequest, ActivityByEventsDTOs activityByEventsDTOs);
    }
}
