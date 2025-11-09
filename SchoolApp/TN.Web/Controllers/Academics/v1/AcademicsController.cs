using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddExam.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.AddSchoolClass.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.DeleteExam;
using ES.Academics.Application.Academics.Command.DeleteSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateExam.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass.RequestCommandMapper;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Account.Application.Account.Command.DeleteLedger;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.Shared.Queries.GetFilterUserActivity;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Transactions.Application.Transactions.Command.AddReceipt;
using TN.Transactions.Application.Transactions.Queries.GetAllReceipt;
using TN.Web.BaseControllers;

namespace TN.Web.Controllers.Academics.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicsController : BaseController
    {
        private readonly IMediator _mediator;
        public AcademicsController(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }

        #region Exam
        #region AddExam
        [HttpPost("AddExam")]

        public async Task<IActionResult> AddExam([FromBody] AddExamRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addExamResult = await _mediator.Send(command);
            #region Switch Statement
            return addExamResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSchoolClass), addExamResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addExamResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addExamResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region AllExam
        [HttpGet("all-exam")]
        public async Task<IActionResult> GetAllExam([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new ExamQuery(paginationRequest);
            var allexamResult = await _mediator.Send(query);
            #region Switch Statement
            return allexamResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(allexamResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = allexamResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(allexamResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterExam
        [HttpGet("FilterExam")]
        public async Task<IActionResult> GetFilterExam([FromQuery] FilterExamDTOs filterExamDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterExamQuery(paginationRequest, filterExamDTOs);
            var filterExamResult = await _mediator.Send(query);
            #region Switch Statement
            return filterExamResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterExamResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterExamResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterExamResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteExam
        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> DeleteExam([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteExamCommand(id);
            var deleteExamResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteExamResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteExamResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteExamResult.Errors),
                _ => BadRequest("Invalid Fields for Delete SchoolClass")
            };

            #endregion
        }
        #endregion

        #region ExamById
        [HttpGet("Exam/{examId}")]
        public async Task<IActionResult> GetExamById([FromRoute] string examId)
        {
            var query = new ExamByIdQuery(examId);
            var examByIdResult = await _mediator.Send(query);
            #region Switch Statement
            return examByIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(examByIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = examByIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(examByIdResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateExam
        [HttpPatch("UpdateExam/{Id}")]

        public async Task<IActionResult> UpdateExam([FromRoute] string Id, [FromBody] UpdateExamRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateSchoolClass = await _mediator.Send(command);
            #region Switch Statement
            return updateSchoolClass switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSchoolClass.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSchoolClass.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSchoolClass.Errors),
                _ => BadRequest("Invalid Fields for Update SchoolClass")
            };

            #endregion
        }
        #endregion

        #endregion

        #region SchoolClass
        #region AddSchoolClass
        [HttpPost("AddSchoolClass")]

        public async Task<IActionResult> AddSchoolClass([FromBody] AddSchoolClassRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addSchoolClassResult = await _mediator.Send(command);
            #region Switch Statement
            return addSchoolClassResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSchoolClass), addSchoolClassResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addSchoolClassResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addSchoolClassResult.Errors),
                _ => BadRequest("Invalid Fields for Add transactions ")

            };

            #endregion
        }
        #endregion

        #region AllSchoolClass
        [HttpGet("all-SchoolClass")]
        public async Task<IActionResult> AllSchoolClass([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new SchoolClassQuery(paginationRequest);
            var schoolClassResult = await _mediator.Send(query);
            #region Switch Statement
            return schoolClassResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(schoolClassResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = schoolClassResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(schoolClassResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterSchoolClass
        [HttpGet("FilterSchoolClass")]
        public async Task<IActionResult> GetFilterSchoolClass([FromQuery] FilterSchoolClassDTOs filterSchoolClassDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterSchoolClassQuery(paginationRequest, filterSchoolClassDTOs);
            var filterSchoolClassResult = await _mediator.Send(query);
            #region Switch Statement
            return filterSchoolClassResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterSchoolClassResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterSchoolClassResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterSchoolClassResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteSchoolClass
        [HttpDelete("DeleteClass/{id}")]

        public async Task<IActionResult> DeleteClass([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSchoolClassCommand(id);
            var deleteSchoolClass = await _mediator.Send(command);
            #region Switch Statement
            return deleteSchoolClass switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteSchoolClass.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSchoolClass.Errors),
                _ => BadRequest("Invalid Fields for Delete SchoolClass")
            };

            #endregion
        }
        #endregion

        #region SchoolClassById
        [HttpGet("SchoolClass/{classId}")]
        public async Task<IActionResult> GetSchoolClassById([FromRoute] string classId)
        {
            var query = new SchoolClassByIdQuery(classId);
            var schoolClassResult = await _mediator.Send(query);
            #region Switch Statement
            return schoolClassResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(schoolClassResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = schoolClassResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(schoolClassResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateSchoolClass
        [HttpPatch("UpdateSchoolClass/{Id}")]

        public async Task<IActionResult> UpdateSchoolClass([FromRoute] string Id, [FromBody] UpdateSchoolClassRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateSchoolClass = await _mediator.Send(command);
            #region Switch Statement
            return updateSchoolClass switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSchoolClass.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSchoolClass.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSchoolClass.Errors),
                _ => BadRequest("Invalid Fields for Update SchoolClass")
            };

            #endregion
        }
        #endregion

        #endregion
    }
}
