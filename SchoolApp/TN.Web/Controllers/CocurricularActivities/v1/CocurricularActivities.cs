
using ES.Student.Application.CocurricularActivities.Command.AddActivity;
using ES.Student.Application.CocurricularActivities.Command.AddActivity.RequestCommandMapper;
using ES.Student.Application.CocurricularActivities.Command.Addparticipation;
using ES.Student.Application.CocurricularActivities.Command.Addparticipation.RequestCommandMapper;
using ES.Student.Application.CocurricularActivities.Queries.Activity;
using ES.Student.Application.CocurricularActivities.Queries.FilterActivity;
using ES.Student.Application.CocurricularActivities.Queries.FilterParticipation;
using ES.Student.Application.Student.Queries.ActivityByEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Certificate.v1;

namespace TN.Web.Controllers.CocurricularActivities.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class CocurricularActivities : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CertificateController> _logger;

        public CocurricularActivities(IMediator mediator, ILogger<CertificateController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;
        }


        #region Activity

        #region ActivityByEvents
        [HttpGet("ActivityByEvents")]
        public async Task<IActionResult> ActivityByEvents([FromQuery] ActivityByEventsDTOs activityByEventsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new ActivityByEventsQuery(activityByEventsDTOs, paginationRequest);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region Activity
        [HttpGet("Activity")]
        public async Task<IActionResult> Activity([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new ActivityQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion

        #region FilterActivity
        [HttpGet("FilterActivity")]
        public async Task<IActionResult> FilterActivity([FromQuery] FilterActivityDTOs filterActivityDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterActivityQuery(filterActivityDTOs, paginationRequest);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddActivity
        [HttpPost("AddActivity")]

        public async Task<IActionResult> AddActivity([FromBody] AddActivityRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var add = await _mediator.Send(command);
            #region Switch Statement
            return add switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddActivity), add.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = add.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(add.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #endregion


        #region Participation

        #region AddParticipation
        [HttpPost("AddParticipation")]

        public async Task<IActionResult> AddParticipation([FromBody] AddParticipationRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var add = await _mediator.Send(command);
            #region Switch Statement
            return add switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddParticipation), add.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = add.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(add.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion


        #region FilterParticipation
        [HttpGet("FilterParticipation")]
        public async Task<IActionResult> FilterParticipation([FromQuery] FilterParticipationDTOs filterParticipationDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterParticipationQuery(filterParticipationDTOs, paginationRequest);
            var filter = await _mediator.Send(query);
            #region Switch Statement
            return filter switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filter.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filter.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filter.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion
    }
}
