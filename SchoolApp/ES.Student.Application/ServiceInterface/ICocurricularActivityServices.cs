using ES.Student.Application.CocurricularActivities.Command.Activity.AddActivity;
using ES.Student.Application.CocurricularActivities.Command.Activity.UpdateActivity;
using ES.Student.Application.CocurricularActivities.Command.Participation.Addparticipation;
using ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation;
using ES.Student.Application.CocurricularActivities.Queries.Activities.Activity;
using ES.Student.Application.CocurricularActivities.Queries.Activities.ActivityById;
using ES.Student.Application.CocurricularActivities.Queries.Activities.FilterActivity;
using ES.Student.Application.CocurricularActivities.Queries.Participation.FilterParticipation;
using ES.Student.Application.CocurricularActivities.Queries.Participation.ParticipationById;
using ES.Student.Application.Student.Command.UpdateParent;
using ES.Student.Application.Student.Queries.ActivityByEvents;
using ES.Student.Application.Student.Queries.GetStudentsById;
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
        Task<Result<ActivityByIdResponse>> GetActivityById(string id, CancellationToken cancellationToken = default);
        Task<Result<ParticipationByIdResponse>> GetParticipationById(string id, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteActivity(string id, CancellationToken cancellationToken);
        Task<Result<UpdateActivityResponse>> UpdateActivity(string id, UpdateActivityCommand updateActivityCommand);

        Task<Result<bool>> DeleteParticipation(string id, CancellationToken cancellationToken);
        Task<Result<UpdateParticipationResponse>> UpdateParticipation(string id, UpdateParticipationCommand updateParticipationCommand);
        Task<Result<PagedResult<FilterActivityResponse>>> FilterActivity(PaginationRequest paginationRequest, FilterActivityDTOs filterActivityDTOs);
        Task<Result<PagedResult<FilterParticipationResponse>>> FilterParticipation(PaginationRequest paginationRequest, FilterParticipationDTOs filterParticipationDTOs);
        Task<Result<PagedResult<ActivityResponse>>> AllActivity(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ActivityByEventsResponse>>> ActivityByEvents(PaginationRequest paginationRequest, ActivityByEventsDTOs activityByEventsDTOs);
    }
}
