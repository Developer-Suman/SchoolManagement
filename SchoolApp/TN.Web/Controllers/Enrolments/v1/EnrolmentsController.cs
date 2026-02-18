using ES.Communication.Application.Communication.Command.AddNotice;
using ES.Communication.Application.Communication.Queries.FilterNotice;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Queries.FilterApplicant;
using ES.Enrolment.Application.Enrolments.Queries.FilterCRMStudents;
using ES.Enrolment.Application.Enrolments.Queries.FilterInquery;
using ES.Enrolment.Application.Enrolments.Queries.GetAllUserProfile;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
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
using TN.Web.Controllers.Communication.v1;

namespace TN.Web.Controllers.Enrolments.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class EnrolmentsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EnrolmentsController> _logger;

        public EnrolmentsController(IMediator mediator, ILogger<EnrolmentsController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;
        }

        #region FilterCRMStudents
        [HttpGet("FilterCRMStudents")]
        public async Task<IActionResult> FilterCRMStudents([FromQuery] FilterCRMStudentsDTOs filterCRMStudentsDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterCRMStudentsQuery(paginationRequest, filterCRMStudentsDTOs);
            var filterCRMStudents = await _mediator.Send(query);
            #region Switch Statement
            return filterCRMStudents switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterCRMStudents.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterCRMStudents.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterCRMStudents.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterApplicants
        [HttpGet("FilterApplicants")]
        public async Task<IActionResult> GetFilterApplicants([FromQuery] FilterApplicantDTOs filterApplicantDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterApplicantQuery(paginationRequest, filterApplicantDTOs);
            var filterApplicants = await _mediator.Send(query);
            #region Switch Statement
            return filterApplicants switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterApplicants.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterApplicants.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterApplicants.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region FilterInquery
        [HttpGet("FilterInquery")]
        public async Task<IActionResult> GetFilterInquery([FromQuery] FilterInquiryDTOs filterInquiryDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterInquiryQuery(paginationRequest, filterInquiryDTOs);
            var filterInquery = await _mediator.Send(query);
            #region Switch Statement
            return filterInquery switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterInquery.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterInquery.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterInquery.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddInquiry
        [HttpPost("AddInquiry")]

        public async Task<IActionResult> AddInquiry([FromBody] AddInquiryRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addInquiry = await _mediator.Send(command);
            #region Switch Statement
            return addInquiry switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddInquiry), addInquiry.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addInquiry.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addInquiry.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region ConvertToApplicant
        [HttpPost("ConvertToApplicant")]

        public async Task<IActionResult> ConvertToApplicant([FromBody] ConvertApplicantRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var convert = await _mediator.Send(command);
            #region Switch Statement
            return convert switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(ConvertToApplicant), convert.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = convert.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(convert.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region ConvertToStudents
        [HttpPost("ConvertToStudents")]

        public async Task<IActionResult> ConvertToStudents([FromBody] ConvertStudentRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var convert = await _mediator.Send(command);
            #region Switch Statement
            return convert switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(ConvertToStudents), convert.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = convert.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(convert.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region UserProfile
        #region GetAllUserProfile
        [HttpGet("GetAllUserProfile")]
        public async Task<IActionResult> UserProfile([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllUserProfileQuery(paginationRequest);
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
    }
}
