using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.Shared.Queries.GetFilterUserActivity;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;

namespace TN.Web.Controllers.UserActivity.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserActivityControllers : BaseController
    {
        private readonly IMediator _mediator;
        public UserActivityControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;

        }

        #region FilterUserActivity
        [HttpGet("GetFilterActivity")]
        public async Task<IActionResult> GetFilterActivity([FromQuery] UserActivityDTOs userActivityDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetFilterUserActivityQuery(paginationRequest, userActivityDTOs);
            var filterUserActivityResult = await _mediator.Send(query);
            #region Switch Statement
            return filterUserActivityResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterUserActivityResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterUserActivityResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(filterUserActivityResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
    }
}
