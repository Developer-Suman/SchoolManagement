using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCountry;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCountry.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity.RequestCommandMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Country;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CountryId;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseByUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseId;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCountry;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.University;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityByCountry;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityId;
using ES.AcademicPrograms.Application.Documents.Command.AddDocuments;
using ES.AcademicPrograms.Application.Documents.Command.AddDocuments.RequestCommandMapper;
using ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType;
using ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType.RequestCommandMapper;
using ES.AcademicPrograms.Application.Documents.Command.DeleteDocuments;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments.RequestCommandMapper;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.RequiredDocument;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.RequiredDocument.RequestCommandMapper;
using ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments;
using ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments.RequestCommandMapper;
using ES.AcademicPrograms.Application.Documents.Queries.Documents.DocumentsById;
using ES.AcademicPrograms.Application.Documents.Queries.Documents.FilterDocuments;
using ES.AcademicPrograms.Application.Documents.Queries.DocumentsType.DocumentsTypes;
using ES.AcademicPrograms.Application.Documents.Queries.DocumentsType.FilterDocumentsType;
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




        #region Documents
        #region UploadApplicantDocuments
        [HttpPatch("UploadApplicantDocuments/{Id}")]
        public async Task<IActionResult> UploadApplicantDocuments(string Id, [FromForm] UploadApplicantDocumentsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var commandResult = await _mediator.Send(command);
            #region Switch Statement
            return commandResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddDocuments), commandResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = commandResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(commandResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region AddDocuments
        [HttpPost("AddDocuments")]

        public async Task<IActionResult> AddDocuments([FromForm] AddDocumentsRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var commandResult = await _mediator.Send(command);
            #region Switch Statement
            return commandResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddDocuments), commandResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = commandResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(commandResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region DocumentsById
        [HttpGet("Documents/{documentsId}")]
        public async Task<IActionResult> DocumentsById([FromRoute] string documentsId)
        {
            var query = new DocumentsByIdQuery(documentsId);
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

        #region DeleteDocuments
        [HttpDelete("DeleteDocuments/{id}")]

        public async Task<IActionResult> DeleteDocuments([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteDocumentsCommand(id);
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

        #region FilterDocuments
        [HttpGet("FilterDocuments")]
        public async Task<IActionResult> FilterDocuments([FromQuery] FilterDocumentsDTOs filterDocumentsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterDocumentsQuery(paginationRequest, filterDocumentsDTOs);
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

        #region DocumentsType

        #region RequiredDocType
        [HttpPost("RequiredDocType")]
        public async Task<IActionResult> RequiredDocType([FromBody] RequiredDocumentsRequest request)
        {
            var command = request.ToCommand();
            var commandResult = await _mediator.Send(command);
            #region Switch Statement
            return commandResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(RequiredDocType), commandResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = commandResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(commandResult.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion

        }

        #endregion



        #region NonRequiredDocType
        [HttpPost("NonRequiredDocType")]
        public async Task<IActionResult> NonRequiredDocType([FromBody] NonRequiredDocumentsRequest request)
        {
            var command = request.ToCommand();
            var commandResult = await _mediator.Send(command);
            #region Switch Statement
            return commandResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(RequiredDocType), commandResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = commandResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(commandResult.Errors),
                _ => BadRequest("Invalid Fields")
            };

            #endregion

        }

        #endregion


        #region AllDocumentsType
        [HttpGet("AllDocumentsType")]
        public async Task<IActionResult> AllDocumentsType([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new DocumentsTypesQuery(paginationRequest);
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


        #region AddDocumentsType
        [HttpPost("AddDocumentsType")]

        public async Task<IActionResult> AddDocumentsType([FromBody] AddDocumentsTypeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var commandResult = await _mediator.Send(command);
            #region Switch Statement
            return commandResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddDocumentsType), commandResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = commandResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(commandResult.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region FilterDocumentsType
        [HttpGet("FilterDocumentsType")]
        public async Task<IActionResult> FilterDocumentsType([FromQuery] FilterDocumentsTypeDTOs filterDocumentsTypeDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterDocumentsTypeQuery(paginationRequest, filterDocumentsTypeDTOs);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #endregion

        #region Country
        #region AddCountry
        [HttpPost("AddCountry")]

        public async Task<IActionResult> AddCountry([FromBody] AddCountryRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addCountry = await _mediator.Send(command);
            #region Switch Statement
            return addCountry switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddIntake), addCountry.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addCountry.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addCountry.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region GetAllCountry
        [HttpGet("GetAllCountry")]
        public async Task<IActionResult> Country([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new CountryQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }


        #endregion


        #region FilterCountry
        [HttpGet("FilterCountry")]
        public async Task<IActionResult> FilterCountry([FromQuery] FilterCountryDTOs filterCountryDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterCountryQuery(paginationRequest, filterCountryDTOs);
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

        #region Course

        #region CourseById
        [HttpGet("CourseById/{courseId}")]
        public async Task<IActionResult> CourseById([FromRoute] string courseId)
        {
            var query = new CourseIdQuery(courseId);
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


        #region CourseByUniversity
        [HttpGet("CourseByUniversity/{universityId}")]
        public async Task<IActionResult> CourseByUniversity([FromRoute] string universityId, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new CourseByUniversityQuery(universityId, paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #region GetAllCourse
        [HttpGet("GetAllCourse")]
        public async Task<IActionResult> Course([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new CourseQuery(paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = (object?)null
                }),
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


        #region GetAllUniversity
        [HttpGet("University")]
        public async Task<IActionResult> University([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new UniversityQuery(paginationRequest);
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

        #region UniversityByCountry
        [HttpGet("UniversityByCountry/{countryId}")]
        public async Task<IActionResult> UniversityByCountry([FromRoute] string countryId, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new UniversityByCountryQuery(countryId, paginationRequest);
            var result = await _mediator.Send(query);
            #region Switch Statement
            return result switch
            {
                { IsSuccess: true, Data: not null } => Ok(new
                {
                    Data = result.Data,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status200OK
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = (object?)null
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(result.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion


        #region CountryById
        [HttpGet("CountryById/{countryId}")]
        public async Task<IActionResult> CountryById([FromRoute] string countryId)
        {
            var query = new CountryIdQuery(countryId);
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


        #region UniversityById
        [HttpGet("UniversityById/{universityId}")]
        public async Task<IActionResult> UniversityById([FromRoute] string universityId)
        {
            var query = new UniversityIdQuery(universityId);
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



        #region FilterIntake
        [HttpGet("FilterIntake")]
        public async Task<IActionResult> GetFilterIntake([FromQuery] FilterIntakeDTOs filterIntakeDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterIntakeQuery(paginationRequest, filterIntakeDTOs);
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



        #region FilterRequirements
        [HttpGet("FilterRequirements")]
        public async Task<IActionResult> GetFilterRequirements([FromQuery] FilterRequirementsDTOs filterRequirementsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterRequirementsQuery(paginationRequest, filterRequirementsDTOs);
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




        #region FilterCourse
        [HttpGet("FilterCourse")]
        public async Task<IActionResult> GetFilterCourse([FromQuery] FilterCourseDTOs filterCourseDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterCourseQuery(paginationRequest, filterCourseDTOs);
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
    }
}
