using ES.Communication.Application.Communication.Command.AddNotice;
using ES.Communication.Application.Communication.Queries.FilterNotice;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry.RequestCommandMapper;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant.RequestCommandMapper;
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
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddInquiry), convert.Data),
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
        public async Task<IActionResult> GetAllUserProfile([FromQuery] PaginationRequest paginationRequest)
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
