using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Communication.Application.Communication.Queries.NoticeDisplay;
using ES.Student.Application.Registration.Command.RegisterMultipleStudents;
using ES.Student.Application.Registration.Command.RegisterMultipleStudents.RequestCommandMapper;
using ES.Student.Application.Registration.Command.RegisterStudents;
using ES.Student.Application.Registration.Command.RegisterStudents.RequestCommandMapper;
using ES.Student.Application.Registration.Queries.FilterRegisterStudents;
using ES.Student.Application.Student.Command.AddAttendances;
using ES.Student.Application.Student.Command.AddAttendances.RequestCommandMapper;
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
using ES.Student.Application.Student.Queries.AcademicYear;
using ES.Student.Application.Student.Queries.Attendance.AttendanceReport;
using ES.Student.Application.Student.Queries.FilterAttendances;
using ES.Student.Application.Student.Queries.FilterParents;
using ES.Student.Application.Student.Queries.FilterStudents;
using ES.Student.Application.Student.Queries.GetAllParent;
using ES.Student.Application.Student.Queries.GetAllStudents;
using ES.Student.Application.Student.Queries.GetParentById;
using ES.Student.Application.Student.Queries.GetStudentByClass;
using ES.Student.Application.Student.Queries.GetStudentForAttendance;
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

        #region Registration

        #region FilterRegisterStudents
        [HttpGet("FilterRegisterStudents")]
        public async Task<IActionResult> FilterRegisterStudents([FromQuery] FilterRegisterStudentsDTOs filterRegisterStudentsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterRegisterStudentsQuery(paginationRequest, filterRegisterStudentsDTOs);
            var filterRegisterStudentsResult = await _mediator.Send(query);
            #region Switch Statement
            return filterRegisterStudentsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterRegisterStudentsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterRegisterStudentsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterRegisterStudentsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        #region MultipleStudentRegistration
        [HttpPost("MultipleStudentRegistration")]

        public async Task<IActionResult> MultipleStudentRegistration([FromBody] RegisterMultipleStudentsRequest request)
        {

            var command = request.ToCommand();
            var register = await _mediator.Send(command);
            #region Switch Statement
            return register switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(MultipleStudentRegistration), register.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { register.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(register.Errors),
                _ => BadRequest("Invalid Fields for RegisterMultipleStudents ")

            };

            #endregion
        }
        #endregion

        #region StudentRegistration
        [HttpPost("StudentRegistration")]

        public async Task<IActionResult> StudentRegistration([FromBody] RegisterStudentsRequest request)
        {

            var command = request.ToCommand();
            var register = await _mediator.Send(command);
            #region Switch Statement
            return register switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(StudentRegistration), register.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { register.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(register.Errors),
                _ => BadRequest("Invalid Fields for RegisterStudents ")

            };

            #endregion
        }
        #endregion

        #endregion

        #region Attendance

        #region AttendanceReport
        [HttpGet("AttendanceReport")]
        public async Task<IActionResult> AttendanceReport([FromQuery] AttendanceReportDTOs attendanceReportDTOs)
        {
            var query = new AttendanceReportQuery(attendanceReportDTOs);
            var attendanceReportResult = await _mediator.Send(query);
            #region Switch Statement
            return attendanceReportResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(attendanceReportResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = attendanceReportResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(attendanceReportResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion




        #region AddStudentAttendence
        [HttpPost("AddStudentAttendence")]

        public async Task<IActionResult> AddStudentAttendence([FromBody] AddAttendanceRequest request)
        {

            var command = request.ToCommand();
            var addStudentAttendanceResult = await _mediator.Send(command);
            #region Switch Statement
            return addStudentAttendanceResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddStudentAttendence), addStudentAttendanceResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { addStudentAttendanceResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addStudentAttendanceResult.Errors),
                _ => BadRequest("Invalid Fields for AddStudentAttendance ")

            };

            #endregion
        }
        #endregion


        #region FilterStudentsAttendance
        [HttpGet("FilterStudentsAttendance")]
        public async Task<IActionResult> GetFilterStudentsAttendance([FromQuery] FilterAttendanceDTOs filterAttendanceDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterAttendanceQuery(paginationRequest, filterAttendanceDTOs);
            var filterAttendanceStudentResult = await _mediator.Send(query);
            #region Switch Statement
            return filterAttendanceStudentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterAttendanceStudentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterAttendanceStudentResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterAttendanceStudentResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
        #endregion

        #region Student  


        #region AllAcademicYear
        [HttpGet("AllAcademicYear")]
        public async Task<IActionResult> AllAcademicYear([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new AcademicYearQuery(paginationRequest);
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



        #region StudentsForAttendance
        [HttpGet("StudentsForAttendance")]
        public async Task<IActionResult> StudentsForAttendance()
        {
            var query = new StudentForAttendanceQuery();
            var displayStudents = await _mediator.Send(query);
            #region Switch Statement
            return displayStudents switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(displayStudents.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = displayStudents.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(displayStudents.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


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

        public async Task<IActionResult> AddStudents( [FromForm] AddStudentsRequest request)
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

        #region StudentsByClass
        [HttpGet("GetStudentByClass/{classId}")]
        public async Task<IActionResult> GetStudentByClass([FromQuery] string classId, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetStudentByClassQuery(paginationRequest,classId);
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
        #endregion

    }

}
