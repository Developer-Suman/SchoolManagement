
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.SchoolReports.AttendanceReport;
using TN.Reports.Application.SchoolReports.CoCurricularActivityReport;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.BaseControllers;
using TN.Web.Controllers.Student.v1;

namespace TN.Web.Controllers.SchoolReport.v1
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolReportsControllers : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SchoolReportsControllers> _logger;

        public SchoolReportsControllers(IMediator mediator, ILogger<SchoolReportsControllers> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region AttendanceReport
        [HttpGet("AttendanceReport")]
        public async Task<IActionResult> AttendanceReport([FromQuery] AttendanceReportDTOs attendanceReportDTOs)
        {
            var query = new AttendanceReportQuery(attendanceReportDTOs);
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

        #region CoCurricularActivitiesReport
        [HttpGet("CoCurricularActivitiesReport")]
        public async Task<IActionResult> CoCurricularActivitiesReport([FromQuery] CoCurricularActivitiesReportDTOs coCurricularActivitiesReportDTOs, [FromQuery] PaginationRequest paginationRequest)
        {
            var query = new CoCurricularActivitiesReportQuery(paginationRequest,coCurricularActivitiesReportDTOs);
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
    }
}
