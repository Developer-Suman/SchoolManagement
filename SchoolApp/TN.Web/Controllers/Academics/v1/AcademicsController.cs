using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddExam.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.AddExamResult.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.AddSchoolClass.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.Academics.Command.AddSubject.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.DeleteExam;
using ES.Academics.Application.Academics.Command.DeleteSchoolClass;
using ES.Academics.Application.Academics.Command.DeleteSubject;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateExam.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.UpdateExamResult;
using ES.Academics.Application.Academics.Command.UpdateExamResult.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass.RequestCommandMapper;
using ES.Academics.Application.Academics.Command.UpdateSubject;
using ES.Academics.Application.Academics.Command.UpdateSubject.RequestCommandMapper;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.ExamResult;
using ES.Academics.Application.Academics.Queries.ExamResultById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterExamResult;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using ES.Academics.Application.Academics.Queries.Subject;
using ES.Academics.Application.Academics.Queries.SubjectById;
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

        #region Subject
        #region AddSubject
        [HttpPost("AddSubject")]

        public async Task<IActionResult> AddSubjects([FromBody] AddSubjectRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addSubjectResult = await _mediator.Send(command);
            #region Switch Statement
            return addSubjectResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddSubjects), addSubjectResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addSubjectResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addSubjectResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region AllSubject
        [HttpGet("all-subject")]
        public async Task<IActionResult> GetAllSubject([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new SubjectQuery(paginationRequest);
            var allSubjectResult = await _mediator.Send(query);
            #region Switch Statement
            return allSubjectResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(allSubjectResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = allSubjectResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(allSubjectResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterSubject
        [HttpGet("FilterSubject")]
        public async Task<IActionResult> GetFilterSubject([FromQuery] FilterSubjectDTOs filterSubjectDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterSubjectQuery(paginationRequest, filterSubjectDTOs);
            var filterSubjectResult = await _mediator.Send(query);
            #region Switch Statement
            return filterSubjectResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterSubjectResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterSubjectResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterSubjectResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteSubject
        [HttpDelete("DeleteSubject/{id}")]

        public async Task<IActionResult> DeleteSubject([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteSubjectCommand(id);
            var deleteSubjectResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteSubjectResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteSubjectResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSubjectResult.Errors),
                _ => BadRequest("Invalid Fields for Delete Subject")
            };

            #endregion
        }
        #endregion

        #region SubjectById
        [HttpGet("{subjectId}")]

        public async Task<IActionResult> GetSubjectById([FromRoute] string subjectId)
        {
            var query = new SubjectByIdQuery(subjectId);
            var subjectByIdResult = await _mediator.Send(query);
            #region Switch Statement
            return subjectByIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(subjectByIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = subjectByIdResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(subjectByIdResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateSubject
        [HttpPatch("UpdateSubject/{Id}")]

        public async Task<IActionResult> UpdateSubject([FromRoute] string Id, [FromBody] UpdateSubjectRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateSubjectResult = await _mediator.Send(command);
            #region Switch Statement
            return updateSubjectResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSubjectResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateSubjectResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSubjectResult.Errors),
                _ => BadRequest("Invalid Fields for Update Subject")
            };

            #endregion
        }
        #endregion

        #endregion






        #region ExamResult
        #region AddExamResult
        [HttpPost("AddExamResult")]

        public async Task<IActionResult> AddExamResult([FromBody] AddExamResultRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addExamResultDetails = await _mediator.Send(command);
            #region Switch Statement
            return addExamResultDetails switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddExamResult), addExamResultDetails.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addExamResultDetails.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addExamResultDetails.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region AllExamResult
        [HttpGet("all-examResult")]
        public async Task<IActionResult> GetAllExamResult([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new ExamResultQuery(paginationRequest);
            var allexamResultDetails = await _mediator.Send(query);
            #region Switch Statement
            return allexamResultDetails switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(allexamResultDetails.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = allexamResultDetails.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(allexamResultDetails.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterExamResult
        [HttpGet("FilterExamResult")]
        public async Task<IActionResult> GetFilterExamResult([FromQuery] FilterExamResultDTOs filterExamResultDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterExamResultQuery(paginationRequest, filterExamResultDTOs);
            var filterExamResultDetails = await _mediator.Send(query);
            #region Switch Statement
            return filterExamResultDetails switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterExamResultDetails.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterExamResultDetails.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterExamResultDetails.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region DeleteExamResult
        [HttpDelete("DeleteExamResult/{id}")]

        public async Task<IActionResult> DeleteExamResult([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteExamCommand(id);
            var deleteExamResultDetails = await _mediator.Send(command);
            #region Switch Statement
            return deleteExamResultDetails switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteExamResultDetails.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteExamResultDetails.Errors),
                _ => BadRequest("Invalid Fields for Delete ExamResult")
            };

            #endregion
        }
        #endregion

        #region ExamResultById
        [HttpGet("ExamResult/{examResultId}")]
        public async Task<IActionResult> GetExamResultById([FromRoute] string examResultId)
        {
            var query = new ExamResultByIdQuery(examResultId);
            var examByIdResultDetails = await _mediator.Send(query);
            #region Switch Statement
            return examByIdResultDetails switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(examByIdResultDetails.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = examByIdResultDetails.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(examByIdResultDetails.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateExam
        [HttpPatch("UpdateExamResult/{Id}")]

        public async Task<IActionResult> UpdateExamResult([FromRoute] string Id, [FromBody] UpdateExamResultRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateExamResultDetails = await _mediator.Send(command);
            #region Switch Statement
            return updateExamResultDetails switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateExamResultDetails.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateExamResultDetails.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateExamResultDetails.Errors),
                _ => BadRequest("Invalid Fields for Update Exam Result")
            };

            #endregion
        }
        #endregion

        #endregion

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
