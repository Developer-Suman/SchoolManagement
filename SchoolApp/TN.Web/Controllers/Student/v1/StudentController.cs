using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Student.Application.Student.Command.AddParent;
using ES.Student.Application.Student.Command.AddParent.ReqeustCommandMapper;
using ES.Student.Application.Student.Command.AddStudents;
using ES.Student.Application.Student.Command.AddStudents.RequestCommandMapper;
using ES.Student.Application.Student.Command.DeleteParent;
using ES.Student.Application.Student.Command.DeleteStudents;
using ES.Student.Application.Student.Command.UpdateParent;
using ES.Student.Application.Student.Command.UpdateParent.RequestCommandMapper;
using ES.Student.Application.Student.Command.UpdateStudents;
using ES.Student.Application.Student.Command.UpdateStudents.RequestCommandMapper;
using ES.Student.Application.Student.Queries.FilterParents;
using ES.Student.Application.Student.Queries.FilterStudents;
using ES.Student.Application.Student.Queries.GetAllParent;
using ES.Student.Application.Student.Queries.GetAllStudents;
using ES.Student.Application.Student.Queries.GetParentById;
using ES.Student.Application.Student.Queries.GetStudentsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NV.Payment.Application.Payment.Command.AddPayment.RequestCommandMapper;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;


namespace TN.Web.Controllers.Student.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : BaseController 
    { 

        private readonly IMediator _mediator;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IMediator mediator,ILogger<StudentController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Student  

        #region FilterStudents
        [HttpGet("FilterStudents")]
        public async Task<IActionResult> GetFilterStudent([FromQuery] FilterStudentsDTOs filterStudentsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterStudentsQuery(paginationRequest, filterStudentsDTOs);
            var filterStudentResult = await _mediator.Send(query);
            #region Switch Statement
            return filterStudentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterStudentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterStudentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterStudentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterParents
        [HttpGet("FilterParents")]
        public async Task<IActionResult> GetFilterParents([FromQuery] FilterParentsDTOs filterParentsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterParentsQuery(paginationRequest, filterParentsDTOs);
            var filterParentsResult = await _mediator.Send(query);
            #region Switch Statement
            return filterParentsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterParentsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterParentsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterParentsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddStudent
        [HttpPost("AddStudents")]

        public async Task<IActionResult> AddStudents([FromBody] AddStudentsRequest request)
        {
          

            var command = request.ToCommand();
            var addStudentResult = await _mediator.Send(command);
            #region Switch Statement
            return addStudentResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddStudents), addStudentResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { addStudentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addStudentResult.Errors),
                _ => BadRequest("Invalid Fields for Add student ")

            };

            #endregion
        }
        #endregion

        #region AllStudents
        [HttpGet("all-Students")]
        public async Task<IActionResult> AllStudents([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllStudentQuery(paginationRequest);
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

        #region StudentsById
        [HttpGet("StudentsBy/{id}")]
        public async Task<IActionResult> GetStudentsById([FromRoute] string id)
        {
            var query = new GetStudentsByIdQuery(id);
            var studentResult = await _mediator.Send(query);
            #region Switch Statement
            return studentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(studentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { studentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(studentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region UpdateParents
        [HttpPatch("UpdateParents/{id}")]

        public async Task<IActionResult> UpdateParents([FromRoute] string id, [FromBody] UpdateParentRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(id);
            var parentResult = await _mediator.Send(command);
            #region Switch Statement
            return parentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(parentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { parentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(parentResult.Errors),
                _ => BadRequest("Invalid Fields for Update parents")
            };

            #endregion


        }
        #endregion

        #region UpdateStudents
        [HttpPatch("UpdateStudents/{id}")]

        public async Task<IActionResult> UpdateStudents([FromRoute] string id, [FromBody] UpdateStudentRequest request)
        {
            //Mapping command and request
            var command = request.ToUpdateStudentCommand(id);
            var studentResult = await _mediator.Send(command);
            #region Switch Statement
            return studentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(studentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { studentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(studentResult.Errors),
                _ => BadRequest("Invalid Fields for Update student")
            };

            #endregion


        }
        #endregion

        #region DeleteStudents
        [HttpDelete("DeleteStudents/{id}")]

        public async Task<IActionResult> DeleteStudents([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteStudentsCommand(id);
            var deleteStudentsResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteStudentsResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { deleteStudentsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteStudentsResult.Errors),
                _ => BadRequest("Invalid Fields for Delete Students")
            };

            #endregion
        }
        #endregion

        #region AllParents
        [HttpGet("all-Parents")]
        public async Task<IActionResult> AllParents([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllParentQuery(paginationRequest);
            var parentResult = await _mediator.Send(query);
            #region Switch Statement
            return parentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(parentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { parentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(parentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion

        #region ParentsById
        [HttpGet("GetParentsBy/{id}")]
        public async Task<IActionResult> GetParentsById([FromRoute] string id)
        {
            var query = new GetAllParentByIdQuery(id);
            var parentResult = await _mediator.Send(query);
            #region Switch Statement
            return parentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(parentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { parentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(parentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteParents
        [HttpDelete("DeleteParents/{id}")]

        public async Task<IActionResult> DeleteParents([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteParentCommand(id);
            var deleteParentResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteParentResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { deleteParentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteParentResult.Errors),
                _ => BadRequest("Invalid Fields for Delete parents")
            };

            #endregion
        }
        #endregion

        #region AddParent
        [HttpPost("AddParent")]

        public async Task<IActionResult> AddParent([FromBody] AddParentRequest request)
        {


            var command = request.ToCommand();
            var addParentResult = await _mediator.Send(command);
            #region Switch Statement
            return addParentResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddStudents), addParentResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { addParentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addParentResult.Errors),
                _ => BadRequest("Invalid Fields for Add Parent ")

            };

            #endregion
        }
        #endregion

    }
}
#endregion