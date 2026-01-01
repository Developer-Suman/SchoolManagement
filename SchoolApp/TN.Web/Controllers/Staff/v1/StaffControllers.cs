using ES.Academics.Application.Academics.Queries.FilterExamResult;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureById;
using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AddAcademicTeam.RequestCommandMapper;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam.RequestCommandMapper;
using ES.Staff.Application.Staff.Command.StaffAttendanceRegister;
using ES.Staff.Application.Staff.Command.StaffAttendanceRegister.RequestCommandMapper;
using ES.Staff.Application.Staff.Command.TeacherAttendanceQR;
using ES.Staff.Application.Staff.Command.TeacherAttendanceQR.RequestCommandMapper;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam.RequestCommandMapper;
using ES.Staff.Application.Staff.Queries.AcademicTeam;
using ES.Staff.Application.Staff.Queries.AcademicTeamById;
using ES.Staff.Application.Staff.Queries.FilterAcademicTeam;
using ES.Student.Application.Student.Queries.FilterStudents;
using ES.Student.Application.Student.Queries.GetAllStudents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Setup.v1;

namespace TN.Web.Controllers.Staff.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffControllers : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StaffControllers> _logger;
        public StaffControllers(IMediator mediator, ILogger<StaffControllers> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;

        }

        #region TeacherAttendance

        #region AddStaffAttendanceRegister
        [HttpPost("AddStaffAttendanceRegister")]
        public async Task<IActionResult> AddStaffAttendanceRegister([FromForm] StaffAttendanceRegisterRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var attendanceRegister = await _mediator.Send(command);
            #region Switch Statement
            return attendanceRegister switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddStaffAttendanceRegister), attendanceRegister.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = attendanceRegister.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(attendanceRegister.Errors),
                _ => BadRequest("Invalid Fields for Add AttendanceRegister")
            };

            #endregion
        }
        #endregion


        #region AddTeacherQR
        [HttpPost("AddTeacherQR")]
        public async Task<IActionResult> AddTeacherQR([FromForm] TeacherAttendanceQRRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var teacherQR = await _mediator.Send(command);
            #region Switch Statement
            return teacherQR switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddTeacherQR), teacherQR.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = teacherQR.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(teacherQR.Errors),
                _ => BadRequest("Invalid Fields for Add AddTeacherQR")
            };

            #endregion
        }
        #endregion

        #endregion

        #region AssignAndUnAssignedClass

        #region UnAssignClass
        [HttpPost("UnAssignClass")]
        public async Task<IActionResult> UnAssignClass([FromBody] UnAssignClassRequest request)
        {
            var command = request.ToCommand();
            var UnAssignClass = await _mediator.Send(command);
            #region Switch Statement
            return UnAssignClass switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(UnAssignClass), UnAssignClass.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = UnAssignClass.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(UnAssignClass.Errors),
                _ => BadRequest("Invalid Fields for UnAssigned to Class")
            };

            #endregion

        }

        #endregion


        #region AssignClass
        [HttpPost("AssignClass")]
        public async Task<IActionResult> AssignClass([FromBody] AssignClassRequest request)
        {
            var command = request.ToCommand();
            var assignClass = await _mediator.Send(command);
            #region Switch Statement
            return assignClass switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AssignClass), assignClass.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = assignClass.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignClass.Errors),
                _ => BadRequest("Invalid Fields for Add AcademicTeam")
            };

            #endregion

        }

        #endregion

        #endregion

        #region AcademicTeam
        #region AcademicTeamById
        [HttpGet("AcademicTeam/{AcademicTeamById}")]
        public async Task<IActionResult> GetAcademicTeamById([FromRoute] string AcademicTeamById)
        {
            var query = new AcademicTeamByIdQuery(AcademicTeamById);
            var AcademicTeamByIdResponse = await _mediator.Send(query);
            #region Switch Statement
            return AcademicTeamByIdResponse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(AcademicTeamByIdResponse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = AcademicTeamByIdResponse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(AcademicTeamByIdResponse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region AllAcademicTeams
        [HttpGet("all-AcademicTeam")]
        public async Task<IActionResult> AcademicTeams([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new AcademicTeamQuery(paginationRequest);
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

        #region FilterStudents
        [HttpGet("FilterAcademicTeam")]
        public async Task<IActionResult> GetFilterAcademicTeam([FromQuery] FilterAcademicTeamDTOs filterAcademicTeamDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterAcademicTeamQuery(paginationRequest, filterAcademicTeamDTOs);
            var filterAcademicTeamResult = await _mediator.Send(query);
            #region Switch Statement
            return filterAcademicTeamResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterAcademicTeamResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterAcademicTeamResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterAcademicTeamResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #region AddAcademicTeam
        [HttpPost("AddAcademicTeam")]
        public async Task<IActionResult> AddAcademicTeam([FromForm] AddAcademicTeamRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addAcademicTeam = await _mediator.Send(command);
            #region Switch Statement
            return addAcademicTeam switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddAcademicTeam), addAcademicTeam.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addAcademicTeam.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addAcademicTeam.Errors),
                _ => BadRequest("Invalid Fields for Add AcademicTeam")
            };

            #endregion
        }
        #endregion
        #endregion
    }
}
