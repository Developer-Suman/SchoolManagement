using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan.RequestCommandMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.DeleteInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan.RequestCommandMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments.RequestCommandMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.DeletePayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments.RequestCommandMapper;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterPayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.PaymentsId;
using ES.Visa.Application.Visa.Command.VisaApplication.DeleteVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NV.Payment.Application.Payment.Command.DeletePayment;
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

        public async Task<IActionResult> AddInstallmentsPlan([FromBody] AddInstallmentsPlanRequest request)
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

        #region UpdateInstallmentsPlan
        [HttpPatch("UpdateInstallmentsPlan/{Id}")]

        public async Task<IActionResult> UpdateInstallmentsPlan(string Id, [FromForm] UpdateInstallmentsPlanRequest request)
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

        #region DeleteInstallmentsPlan
        [HttpDelete("DeleteInstallmentsPlan/{id}")]

        public async Task<IActionResult> DeleteInstallmentsPlan([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteInstallmentsPlanCommand(id);
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
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
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

        #region UpdatePayments
        [HttpPatch("UpdatePayments/{Id}")]

        public async Task<IActionResult> UpdatePayments(string Id, [FromForm] UpdatePaymentsRequest request)
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

        #region DeletePayments
        [HttpDelete("DeletePayments/{id}")]

        public async Task<IActionResult> DeletePayments([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeletePaymentsCommands(id);
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

        #region PaymentsById
        [HttpGet("PaymentsById/{PaymentsId}")]
        public async Task<IActionResult> PaymentsById([FromRoute] string PaymentsId)
        {
            var query = new PaymentsIdQuery(PaymentsId);
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

        #region FilterPayments
        [HttpGet("FilterPayments")]
        public async Task<IActionResult> FilterPayments([FromQuery] FilterPaymentsDTOs filterPaymentsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterPaymentsQuery(paginationRequest, filterPaymentsDTOs);
            var filteredResult = await _mediator.Send(query);
            #region Switch Statement
            return filteredResult switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = filteredResult.Data,
                    Message = filteredResult.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = filteredResult.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filteredResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
        #endregion
    }
}
