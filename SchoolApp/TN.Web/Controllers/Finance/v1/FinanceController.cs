using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure.RequestCommandMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType.RequestCommandMapper;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee.RequestCommandMapper;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords.RequestCommandMapper;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Communication.v1;

namespace TN.Web.Controllers.Finance.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class FinanceController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CommunicationController> _logger;

        public FinanceController(IMediator mediator, ILogger<CommunicationController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;
        }
        #region Payments Records


        #region FilterPaymentsRecords
        [HttpGet("FilterPaymentsRecords")]
        public async Task<IActionResult> FilterPaymentsRecords([FromQuery] FilterPaymentsRecordsDTOs filterPaymentsRecordsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterPaymentsRecordsQuery(paginationRequest, filterPaymentsRecordsDTOs);
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



        #region AddPaymentsRecords
        [HttpPost("AddPaymentsRecords")]

        public async Task<IActionResult> AddPaymentsRecords([FromBody] AddpaymentsRecordsRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddPaymentsRecords), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add")

            };

            #endregion
        }
        #endregion


        #endregion

        #region StudentFee


        #region FilterStudentFee
        [HttpGet("FilterStudentFee")]
        public async Task<IActionResult> FilterStudentFee([FromQuery] FilterStudentFeeDTOs filterStudentFeeDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterStudentFeeQuery(paginationRequest, filterStudentFeeDTOs);
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



        #region AddStudentFee
        [HttpPost("AddStudentFee")]

        public async Task<IActionResult> AddStudentFee([FromBody] AddStudentFeeRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddStudentFee), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add")

            };

            #endregion
        }
        #endregion


        #endregion

        #region FeeStructure


        #region FilterFeeStructure
        [HttpGet("FilterFeeStructure")]
        public async Task<IActionResult> FilterFeeStructure([FromQuery] FilterFeeStructureDTOs filterFeeStructureDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterFeeStructureQuery(paginationRequest, filterFeeStructureDTOs);
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



        #region AddFeeStructure
        [HttpPost("AddFeeStructure")]

        public async Task<IActionResult> AddFeeStructure([FromBody] AddFeeStructureRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddFeeStructure), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add")

            };

            #endregion
        }
        #endregion


        #endregion

        #region Feetype


        #region FilterFeetype
        [HttpGet("FilterFeetype")]
        public async Task<IActionResult> FilterFeetype([FromQuery] FilterFeeStructureDTOs filterFeeStructureDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterFeeStructureQuery(paginationRequest, filterFeeStructureDTOs);
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



        #region AddFeetype
        [HttpPost("AddFeetype")]

        public async Task<IActionResult> AddFeetype([FromBody] AddFeeTypeRequest request)
        {
            var command = request.ToCommand();
            var result = await _mediator.Send(command);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddFeetype), result.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { result.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid Fields for Add")

            };

            #endregion
        }
        #endregion


        #endregion
    }
}
