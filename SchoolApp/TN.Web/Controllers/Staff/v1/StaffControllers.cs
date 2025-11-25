using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AddAcademicTeam.RequestCommandMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.Setup.Command.AddInstitution;
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

        #region AcademicTeam
        #region AddAcademicTeam
        [HttpPost("AddAcademicTeam")]
        public async Task<IActionResult> AddAcademicTeam([FromBody] AddAcademicTeamRequest request)
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
