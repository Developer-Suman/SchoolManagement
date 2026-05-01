using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.Academics.Command.Events.DeleteEvents;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.Academics.Queries.Events.Events;
using ES.Academics.Application.Academics.Queries.Events.EventsById;
using ES.Academics.Application.Academics.Queries.Events.FilterEvents;
using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication.RequestCommandMapper;
using ES.Visa.Application.Visa.Command.VisaApplication.DeleteVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication.RequestCommandMapper;
using ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus;
using ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus.RequestCommandMapper;
using ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory;
using ES.Visa.Application.Visa.Queries.VisaStatus;
using ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;

namespace TN.Web.Controllers.VisaApplication.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class VisaApplicationController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;
        public VisaApplicationController(IMediator mediator, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;

        }

        #region VisaApplicationStatusHistory

        #region FilterVisaApplicationStatusHistory
        [HttpGet("FilterVisaApplicationStatusHistory")]
        public async Task<IActionResult> FilterVisaApplicationStatusHistory([FromQuery] FilterVisaApplicationStatusHistoryDTOs filterVisaApplicationStatusHistoryDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterVisaApplicationStatusHistoryQuery(paginationRequest, filterVisaApplicationStatusHistoryDTOs);
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filteredResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filteredResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion

        #region VisaStatus

        #region AddVisaStatus
        [HttpPost("AddVisaStatus")]

        public async Task<IActionResult> AddVisaStatus([FromBody] AddVisaStatusRequest request)
        {
            var command = request.ToCommand();
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddVisaApplication),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion




        #region FilterVisaStatus
        [HttpGet("FilterVisaStatus")]
        public async Task<IActionResult> FilterVisaStatus([FromQuery] FilterVisaStatusDTOs filterVisaStatusDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterVisaStatusQuery(paginationRequest, filterVisaStatusDTOs);
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filteredResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filteredResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #endregion



        #region VisaApplication
        #region AddVisaApplication
        [HttpPost("AddVisaApplication")]

        public async Task<IActionResult> AddVisaApplication([FromForm] AddVisaApplicationRequest request)
        {
            var command = request.ToCommand();
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddVisaApplication),
                new { id = addResult.Data.id },
                new
                {
                    Data = addResult.Data,
                    Message = addResult.Message,
                    StatusCode = StatusCodes.Status201Created
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = addResult.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region UpdateVisaApplication
        [HttpPatch("UpdateVisaApplication/{Id}")]

        public async Task<IActionResult> UpdateVisaApplication(string Id, [FromForm] UpdateVisaApplicationRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new ObjectResult(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                })
                {
                    StatusCode = StatusCodes.Status200OK
                },
                { IsSuccess: true, Data: null, Message: not null } => Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion
        }
        #endregion

        #region VisaApplicationById
        [HttpGet("VisaApplication/{visaApplicationId}")]
        public async Task<IActionResult> VisaApplicationById([FromRoute] string visaApplicationId)
        {
            var query = new VisaApplicationQuery(visaApplicationId);
            var queryResult = await _mediator.Send(query);
            #region Switch Statement
            return queryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(queryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = queryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(queryResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteVisaApplication
        [HttpDelete("DeleteVisaApplication/{id}")]

        public async Task<IActionResult> DeleteVisaApplication([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteVisaApplicationCommand(id);
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {

                { IsSuccess: true } => Ok(new
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = result.Message
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion
        }

        #endregion

        #region FilterVisaApplication
        [HttpGet("FilterVisaApplication")]
        public async Task<IActionResult> FilterVisaApplication([FromQuery] FilterVisaApplicationDTOs filterVisaApplicationDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterVisaApplicationQuery(paginationRequest, filterVisaApplicationDTOs);
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filteredResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filteredResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #endregion
    }
}
