using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Web.BaseControllers;

namespace TN.Web.Controllers.Setup.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitializeControllers : BaseController
    {
        private readonly IInitializeServices _initializeServices;
        private readonly ILogger<InitializeControllers> _logger;
        public InitializeControllers(
            UserManager<ApplicationUser> userManager,
            ILogger<InitializeControllers> logger,
            IInitializeServices initializeServices,
            RoleManager<IdentityRole> roleManager):
            
            base(userManager, roleManager)
        {
            _logger = logger;
            _initializeServices = initializeServices;
            
        }

        [HttpPost()]
        public async Task<IActionResult> Initialize()
        {
            var initiallizeResult = await _initializeServices.InitializeAsync();
            #region switch
            return initiallizeResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(initiallizeResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(initiallizeResult.Errors),
                _ => BadRequest("Invalid Request")
            };

            #endregion


        }
    }
}
