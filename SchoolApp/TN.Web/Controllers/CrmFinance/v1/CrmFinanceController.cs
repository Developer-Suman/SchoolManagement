using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan.RequestCommandMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments.RequestCommandMapper;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;

namespace TN.Web.Controllers.CrmFinance.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class CrmFinanceController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public CrmFinanceController(IMediator mediator, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;

        }

        #region InstallmentsPlan
        #region AddInstallmentsPlan
        [HttpPost("AddInstallmentsPlan")]

        public async Task<IActionResult> AddInstallmentsPlan([FromForm] AddInstallmentsPlanRequest request)
        {
            var command = request.ToCommand();
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddInstallmentsPlan),
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

        #region InstallmentPlanById
        [HttpGet("InstallmentPlan/{installmentPlanId}")]
        public async Task<IActionResult> InstallmentPlanById([FromRoute] string installmentPlanId)
        {
            var query = new InstallmentPlanQuery(installmentPlanId);
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

        #region FilterInstallmentPlan
        [HttpGet("FilterInstallmentPlan")]
        public async Task<IActionResult> FilterInstallmentPlan([FromQuery] FilterInstallmentPlanDTOs filterInstallmentPlanDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterInstallmentPlanQuery(paginationRequest, filterInstallmentPlanDTOs);
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

        #region Payment
        #region AddPayments
        [HttpPost("AddPayments")]

        public async Task<IActionResult> AddPayments([FromBody] AddPaymentsRequest request)
        {
            var command = request.ToCommand();
            var addResult = await _mediator.Send(command);
            #region Switch Statement
            return addResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(
                nameof(AddInstallmentsPlan),
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
        #endregion
    }
}
