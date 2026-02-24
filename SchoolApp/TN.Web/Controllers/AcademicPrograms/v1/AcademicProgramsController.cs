using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Enrolments.v1;

namespace TN.Web.Controllers.AcademicPrograms.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class AcademicProgramsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AcademicProgramsController> _logger;

        public AcademicProgramsController(IMediator mediator, ILogger<AcademicProgramsController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;
        }



        #region Course
        #region GetAllCourse
        [HttpGet("GetAllCourse")]
        public async Task<IActionResult> Course([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new CourseQuery(paginationRequest);
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

        #endregion



        #region AddIntake
        [HttpPost("AddIntake")]

        public async Task<IActionResult> AddIntake([FromBody] AddIntakeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addIntake = await _mediator.Send(command);
            #region Switch Statement
            return addIntake switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddIntake), addIntake.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addIntake.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addIntake.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion


        #region AddRequirements
        [HttpPost("AddRequirements")]

        public async Task<IActionResult> AddRequirements([FromBody] AddRequirementsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addRequirements = await _mediator.Send(command);
            #region Switch Statement
            return addRequirements switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddRequirements), addRequirements.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addRequirements.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addRequirements.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion




        #region AddCourse
        [HttpPost("AddCourse")]

        public async Task<IActionResult> AddCourse([FromBody] AddCourseRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addCourse = await _mediator.Send(command);
            #region Switch Statement
            return addCourse switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddCourse), addCourse.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addCourse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addCourse.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion



        #region AddUniversity
        [HttpPost("AddUniversity")]

        public async Task<IActionResult> AddUniversity([FromBody] AddUniversityRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addUniversity = await _mediator.Send(command);
            #region Switch Statement
            return addUniversity switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddUniversity), addUniversity.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addUniversity.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addUniversity.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion


        #region FilterUniversity
        [HttpGet("FilterUniversity")]
        public async Task<IActionResult> GetFilterUniversity([FromQuery] FilterUniversityDTOs filterUniversityDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterUniversityQuery(paginationRequest, filterUniversityDTOs);
            var filterUniversity = await _mediator.Send(query);
            #region Switch Statement
            return filterUniversity switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterUniversity.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterUniversity.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterUniversity.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        #region FilterIntake
        [HttpGet("FilterIntake")]
        public async Task<IActionResult> GetFilterIntake([FromQuery] FilterIntakeDTOs filterIntakeDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterIntakeQuery(paginationRequest, filterIntakeDTOs);
            var filterIntake = await _mediator.Send(query);
            #region Switch Statement
            return filterIntake switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterIntake.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterIntake.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterIntake.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion



        #region FilterRequirements
        [HttpGet("FilterRequirements")]
        public async Task<IActionResult> GetFilterRequirements([FromQuery] FilterRequirementsDTOs filterRequirementsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterRequirementsQuery(paginationRequest, filterRequirementsDTOs);
            var filterRequirements = await _mediator.Send(query);
            #region Switch Statement
            return filterRequirements switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterRequirements.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterRequirements.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterRequirements.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion




        #region FilterCourse
        [HttpGet("FilterCourse")]
        public async Task<IActionResult> GetFilterCourse([FromQuery] FilterCourseDTOs filterCourseDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterCourseQuery(paginationRequest, filterCourseDTOs);
            var filterCourse = await _mediator.Send(query);
            #region Switch Statement
            return filterCourse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterCourse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterCourse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterCourse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
    }
}
