using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.CustomMiddleware.FiscalYearContext
{
    public class FiscalContextMiddleware
    {

        private readonly RequestDelegate _next;

        public FiscalContextMiddleware(RequestDelegate next)
        {
            _next = next;
            
        }


        public async Task InvokeAsync(HttpContext context, FiscalContext fiscalContext, IFiscalYearService fiscalYearService ,ITokenService tokenService )
        {

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                await _next(context);
                return;
            }
            var schoolId = tokenService.SchoolId().FirstOrDefault();

            if (!string.IsNullOrEmpty(schoolId))
            {
                var fiscalYear = await fiscalYearService.GetCurrentFiscalYearFromSettingsAsync();
                var academicYear = await fiscalYearService.GetAcademicYearFromSettingsAsync();
                fiscalContext.CurrentSchoolId = schoolId;
                fiscalContext.CurrentFiscalYearId = fiscalYear.Id;
                fiscalContext.CurrentAcademicYearId = academicYear.Id;
            }

            await _next(context);

        }
    }
}
