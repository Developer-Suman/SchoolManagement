using ES.Student.Application.Student.Queries.FilterStudents;
using ES.Student.Application.Student.Queries.GetAllStudents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.DeleteConversionFactor;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteContributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteSchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteSchoolItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolAssetsReport;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItems;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.Sales.Command.AddSalesItems;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Sales.v1;

namespace TN.Web.Controllers.SchoolAssets.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolAssetsControllers : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SalesControllers> _logger;

        public SchoolAssetsControllers(IMediator mediator, ILogger<SalesControllers> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
            _logger = logger;

        }


        #region SchoolAssetsReport
        [HttpGet("SchoolAssetsReport")]
        public async Task<IActionResult> SchoolAssetsReport([FromQuery] SchoolAssetsReportDTOs schoolAssetsReportDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new SchoolAssetsReportQuery(paginationRequest, schoolAssetsReportDTOs);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region SchoolItemshistory

        #region DeleteSchoolItemHistory
        [HttpDelete("DeleteSchoolItemHistory/{id}")]

        public async Task<IActionResult> DeleteSchoolItemHistory([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSchoolItemHistoryCommand(id);
            var deleteSchoolItemHistory = await _mediator.Send(command);
            #region Switch Statement
            return deleteSchoolItemHistory switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteSchoolItemHistory.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSchoolItemHistory.Errors),
                _ => BadRequest("Invalid Fields for Add SchoolItemHistory")
            };

            #endregion
        }
        #endregion

        #region UpdateSchoolItemHistory
        [HttpPatch("UpdateSchoolItemHistory/{id}")]

        public async Task<IActionResult> UpdateSchoolItemHistory([FromRoute] string id, [FromBody] UpdateSchoolItemHistoryRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateSchoolItemHistoryResult = await _mediator.Send(command);
            #region Switch Statement
            return updateSchoolItemHistoryResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSchoolItemHistoryResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSchoolItemHistoryResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSchoolItemHistoryResult.Errors),
                _ => BadRequest("Invalid Fields for Update SchoolItemHistory")
            };

            #endregion


        }
        #endregion

        #region FilterSchoolItemsHistory
        [HttpGet("FilterSchoolItemsHistory")]
        public async Task<IActionResult> GetFilterSchoolItemsHistory([FromQuery] FilterSchoolItemsHistoryDTOs filterSchoolItemsHistoryDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterSchoolItemsHistoryQuery(paginationRequest, filterSchoolItemsHistoryDTOs);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddFilterItemHistory
        [HttpPost("AddSchoolItemHistory")]

        public async Task<IActionResult> AddSchoolItemHistory([FromBody] AddSchoolItemHistoryRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSchoolItemHistory), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add SchoolItemHistory ")

            };

            #endregion
        }
        #endregion


        #endregion

        #region Contributors

        #region DeleteContributors
        [HttpDelete("DeleteContributors/{id}")]

        public async Task<IActionResult> DeleteContributors([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteContributorsCommand(id);
            var deleteContributors = await _mediator.Send(command);
            #region Switch Statement
            return deleteContributors switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteContributors.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteContributors.Errors),
                _ => BadRequest("Invalid Fields for Contributors")
            };

            #endregion
        }
        #endregion
        #region UpdateContributors
        [HttpPatch("UpdateContributors/{id}")]

        public async Task<IActionResult> UpdateContributors([FromRoute] string id, [FromBody] UpdateContributorsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateContributorsResult = await _mediator.Send(command);
            #region Switch Statement
            return updateContributorsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateContributorsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateContributorsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateContributorsResult.Errors),
                _ => BadRequest("Invalid Fields for Update Contributors")
            };

            #endregion


        }
        #endregion

        #region AllContributors
        [HttpGet("all-Contributors")]
        public async Task<IActionResult> AllContributors([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new ContributorsQuery(paginationRequest);
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


        #region FilterContributors
        [HttpGet("FilterContributors")]
        public async Task<IActionResult> GetFilterContributors([FromQuery] FilterContributorsDTOs filterContributorsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterContributorsQuery(paginationRequest, filterContributorsDTOs);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        #region AddContributors
        [HttpPost("AddContributors")]

        public async Task<IActionResult> AddContributors([FromBody] AddContributorsRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddContributors), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add SchoolItems ")

            };

            #endregion
        }
        #endregion


        #endregion

        #region SchoolItems

        #region DeleteSchoolItems
        [HttpDelete("DeleteSchoolItems/{id}")]

        public async Task<IActionResult> DeleteSchoolItems([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSchoolItemsCommand(id);
            var deleteSchoolItems = await _mediator.Send(command);
            #region Switch Statement
            return deleteSchoolItems switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteSchoolItems.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSchoolItems.Errors),
                _ => BadRequest("Invalid Fields for Delete School Items")
            };

            #endregion
        }
        #endregion

        #region UpdateSchoolItems
        [HttpPatch("UpdateSchoolItems/{id}")]

        public async Task<IActionResult> UpdateSchoolItems([FromRoute] string id, [FromBody] UpdateSchoolitemsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var updateSchoolItemsResult = await _mediator.Send(command);
            #region Switch Statement
            return updateSchoolItemsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSchoolItemsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSchoolItemsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSchoolItemsResult.Errors),
                _ => BadRequest("Invalid Fields for Update SchoolItems")
            };

            #endregion


        }
        #endregion

        #region AllSchoolItems
        [HttpGet("all-SchoolItems")]
        public async Task<IActionResult> AllSchoolItems([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new SchoolItemsQuery(paginationRequest);
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



        #region FilterSchoolItems
        [HttpGet("FilterSchoolItems")]
        public async Task<IActionResult> GetFilterSchoolItems([FromQuery] FilterSchoolItemsDTOs filterSchoolItemsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterSchoolItemsQuery(paginationRequest, filterSchoolItemsDTOs);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(result.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        #region AddSchoolItems
        [HttpPost("AddSchoolItems")]

        public async Task<IActionResult> AddSchoolItems([FromBody] AddSchoolItemsRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSchoolItems), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add SchoolItems ")

            };

            #endregion
        }
        #endregion


        #endregion
    }
}
