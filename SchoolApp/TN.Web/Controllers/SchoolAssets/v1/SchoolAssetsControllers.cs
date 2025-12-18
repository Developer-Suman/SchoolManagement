using ES.Student.Application.Student.Queries.FilterStudents;
using ES.Student.Application.Student.Queries.GetAllStudents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems.RequestCommandMapper;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory;
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

        #region SchoolItemshistory


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
        [HttpPost("AddFilterItemHistory")]

        public async Task<IActionResult> AddFilterItemHistory([FromBody] AddSchoolItemHistoryRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddFilterItemHistory), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add SchoolItemHistory ")

            };

            #endregion
        }
        #endregion


        #endregion

        #region Contributors


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
