using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TN.Authentication.Domain.Entities;

namespace TN.Web.BaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public BaseController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            
        }

        protected IActionResult HandleFailureResult(IEnumerable<string> errors)
        {
            //Check the error message and return appropriate status code

            if (errors.Any(errors => errors.Contains("Unauthorized")))
            {

                var rawMessage = errors.FirstOrDefault(e =>
                    e.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase));

                var cleanedMessage = ExtractCleanMessage(rawMessage);

                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = cleanedMessage
                });

            }
            else if (errors.Any(errors => errors.Contains("NotFound", StringComparison.OrdinalIgnoreCase)))
            {
                var rawMessage = errors.FirstOrDefault(e =>
                    e.Contains("NotFound", StringComparison.OrdinalIgnoreCase));

                var cleanedMessage = ExtractCleanMessage(rawMessage);

                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = cleanedMessage
                });

            }
            else if (errors.Any(errors => errors.Contains("InsufficientFunds", StringComparison.OrdinalIgnoreCase)))
            {
                var rawMessage = errors.FirstOrDefault(e =>
                    e.Contains("InsufficientFunds", StringComparison.OrdinalIgnoreCase));

                var cleanedMessage = ExtractCleanMessage(rawMessage);

                return StatusCode(StatusCodes.Status402PaymentRequired, new
                {
                    StatusCode = StatusCodes.Status402PaymentRequired,
                    Message = cleanedMessage
                });
            }
            else if (errors.Any(errors => errors.Contains("ForbiddenAccess", StringComparison.OrdinalIgnoreCase)))
            {
                var rawMessage = errors.FirstOrDefault(e =>
                    e.Contains("ForbiddenAccess", StringComparison.OrdinalIgnoreCase));

                var cleanedMessage = ExtractCleanMessage(rawMessage);

                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = cleanedMessage
                });

            }

            else if (errors.Any(errors => errors.Contains("Conflict", StringComparison.OrdinalIgnoreCase)))
            {
                var rawMessage = errors.FirstOrDefault(e =>
                    e.Contains("Conflict", StringComparison.OrdinalIgnoreCase));

                var cleanedMessage = ExtractCleanMessage(rawMessage);

                return StatusCode(StatusCodes.Status409Conflict, new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = cleanedMessage
                });
            }
            else if (errors.Any(errors => errors.Contains("NoContent", StringComparison.OrdinalIgnoreCase)))
            {
                var rawMessage = errors.FirstOrDefault(e =>
                 e.Contains("NoContent", StringComparison.OrdinalIgnoreCase));

                var cleanedMessage = ExtractCleanMessage(rawMessage);

                return StatusCode(StatusCodes.Status204NoContent, new
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = cleanedMessage
                });

            }
            else
            {
                return BadRequest(errors);
            }
        }

        private static string ExtractCleanMessage(string? message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "An error occurred.";

            var commaIndex = message.IndexOf(',');

            if (commaIndex >= 0 && commaIndex < message.Length - 1)
            {
                return message[(commaIndex + 1)..].Trim();
            }

            return message.Trim();
        }
    }
}
