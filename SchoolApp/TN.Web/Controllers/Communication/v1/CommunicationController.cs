using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificateById;
using ES.Communication.Application.Communication.Command.AddNotice;
using ES.Communication.Application.Communication.Command.AddNotice.RequestCommandMapper;
using ES.Communication.Application.Communication.Command.PublishNotice;
using ES.Communication.Application.Communication.Command.PublishNotice.RequestCommandMapper;
using ES.Communication.Application.Communication.Command.UnPublishNotice;
using ES.Communication.Application.Communication.Command.UnPublishNotice.RequestCommandMapper;
using ES.Communication.Application.Communication.Queries.FilterNotice;
using ES.Communication.Application.Communication.Queries.NoticeById;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
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
using TN.Web.Controllers.Certificate.v1;

namespace TN.Web.Controllers.Communication.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class CommunicationController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CommunicationController> _logger;
        public CommunicationController(IMediator mediator, ILogger<CommunicationController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _logger = logger;
            _mediator = mediator;
        }

        #region Notice

        #region PublishNotice
        [HttpPost("PublishNotice")]
        public async Task<IActionResult> PublishNotice([FromBody] PublishNoticeRequest request)
        {
            var command = request.ToCommand();
            var publishNotice = await _mediator.Send(command);
            #region Switch Statement
            return publishNotice switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(PublishNotice), publishNotice.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = publishNotice.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(publishNotice.Errors),
                _ => BadRequest("Invalid Fields for publishNotice")
            };

            #endregion

        }

        #endregion


        #region UnPublishNotice
        [HttpPost("UnPublishNotice")]
        public async Task<IActionResult> UnPublishNotice([FromBody] UnPublishNoticeRequest request)
        {
            var command = request.ToCommand();
            var unPublishNotice = await _mediator.Send(command);
            #region Switch Statement
            return unPublishNotice switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(UnPublishNotice), unPublishNotice.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = unPublishNotice.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(unPublishNotice.Errors),
                _ => BadRequest("Invalid Fields for UnPublishNotice")
            };

            #endregion

        }

        #endregion

        #region FilterNotice
        [HttpGet("FilterNotice")]
        public async Task<IActionResult> GetFilterNotice([FromQuery] FilterNoticeDTOs filterNoticeDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new FilterNoticeQuery(paginationRequest, filterNoticeDTOs);
            var filterNotice = await _mediator.Send(query);
            #region Switch Statement
            return filterNotice switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterNotice.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterNotice.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterNotice.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion


        #region AddNotice
        [HttpPost("AddNotice")]

        public async Task<IActionResult> AddNotice([FromBody] AddnoticeRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addNoticeCommand = await _mediator.Send(command);
            #region Switch Statement
            return addNoticeCommand switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddNotice), addNoticeCommand.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addNoticeCommand.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addNoticeCommand.Errors),
                _ => BadRequest("Invalid Fields ")

            };

            #endregion
        }
        #endregion

        #region NoticeById
        [HttpGet("Notice/{NoticeId}")]
        public async Task<IActionResult> GetNoticeById([FromRoute] string NoticeId)
        {
            var query = new NoticeByIdQuery(NoticeId);
            var noticeByIdResponse = await _mediator.Send(query);
            #region Switch Statement
            return noticeByIdResponse switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(noticeByIdResponse.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = noticeByIdResponse.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(noticeByIdResponse.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #endregion
    }
}
