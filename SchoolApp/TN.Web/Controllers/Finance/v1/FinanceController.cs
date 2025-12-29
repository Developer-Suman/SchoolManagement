using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Communication.Application.Communication.Command.PublishNotice;
using ES.Communication.Application.Communication.Command.PublishNotice.RequestCommandMapper;
using ES.Communication.Application.Communication.Queries.NoticeById;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure.RequestCommandMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType.RequestCommandMapper;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee.RequestCommandMapper;
using ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee;
using ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee.RequestCommandMapper;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeType;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeType.RequestMapper;
using ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee;
using ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee.RequestMapper;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords.RequestCommandMapper;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureById;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeById;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.PaymentsRecordsById;
using ES.Student.Application.Student.Queries.GetAllStudents;
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



        #region StudentFeeSummary
        #region StudentFeeSummary
        [HttpGet("StudentFeeSummary")]
        public async Task<IActionResult> StudentFeeSummary([FromQuery] StudentFeeSummaryDTOs studentFeeSummaryDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new StudentFeeSummaryQuery(paginationRequest, studentFeeSummaryDTOs);
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

        #endregion
        #region Payments Records
        #region PaymentsRecordsById
        [HttpGet("PaymentsRecords/{PaymentsRecordsById}")]
        public async Task<IActionResult> GetPaymentsRecordsById([FromRoute] string PaymentsRecordsById)
        {
            var query = new PaymentsRecordsByIdQuery(PaymentsRecordsById);
            var PaymentsRecordsByIdResponse = await _mediator.Send(query);
            #region Switch Statement
            return PaymentsRecordsByIdResponse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(PaymentsRecordsByIdResponse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = PaymentsRecordsByIdResponse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(PaymentsRecordsByIdResponse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

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
        #region UpdateStudentFee
        [HttpPatch("UpdateStudentFee/{Id}")]

        public async Task<IActionResult> UpdateStudentFee([FromRoute] string Id, [FromBody] UpdateStudentFeeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var update = await _mediator.Send(command);
            #region Switch Statement
            return update switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(update.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = update.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(update.Errors),
                _ => BadRequest("Invalid Fields for Update")
            };

            #endregion
        }
        #endregion  



        #region StudentFeeById
        [HttpGet("StudentFee/{StudentFeeById}")]
        public async Task<IActionResult> GetStudentFeeById([FromRoute] string StudentFeeById)
        {
            var query = new StudentFeeByIdQuery(StudentFeeById);
            var StudentFeeByIdResponse = await _mediator.Send(query);
            #region Switch Statement
            return StudentFeeByIdResponse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(StudentFeeByIdResponse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = StudentFeeByIdResponse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(StudentFeeByIdResponse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion



        #region AssignFee
        [HttpPost("AssignFee")]
        public async Task<IActionResult> AssignFee([FromBody] AssignMonthlyFeeRequest request)
        {
            var command = request.ToCommand();
            var assign = await _mediator.Send(command);
            #region Switch Statement
            return assign switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AssignFee), assign.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assign.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assign.Errors),
                _ => BadRequest("Invalid Fields for assign")
            };

            #endregion

        }

        #endregion



        #region StudentFee
        [HttpGet("StudentFee")]
        public async Task<IActionResult> StudentFee([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new StudentFeeQuery(paginationRequest);
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

        #region FeeStructureById
        [HttpGet("FeeStructure/{FeeStructureById}")]
        public async Task<IActionResult> GetFeeStructureById([FromRoute] string FeeStructureById)
        {
            var query = new FeeStructureByIdQuery(FeeStructureById);
            var FeeStructureByIdResponse = await _mediator.Send(query);
            #region Switch Statement
            return FeeStructureByIdResponse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(FeeStructureByIdResponse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = FeeStructureByIdResponse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(FeeStructureByIdResponse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion


        #region FeeStructure
        [HttpGet("FeeStructure")]
        public async Task<IActionResult> FeeStructure([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FeeStructureQuery(paginationRequest);
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
            #region Feetype
            [HttpGet("Feetype")]
        public async Task<IActionResult> Feetype([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FeeTypeQuery(paginationRequest);
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
        #region UpdateFeeType
        [HttpPatch("UpdateFeeType/{Id}")]

        public async Task<IActionResult> UpdateFeeType([FromRoute] string Id, [FromBody] UpdateFeeTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var update = await _mediator.Send(command);
            #region Switch Statement
            return update switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(update.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = update.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(update.Errors),
                _ => BadRequest("Invalid Fields for Update")
            };

            #endregion
        }
        #endregion  
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
