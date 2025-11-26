using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Infrastructure.Data;

namespace TN.Shared.Infrastructure.Authorization
{
    public class TeacherClassHandler : AuthorizationHandler<TeacherClassRequirement, string>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TeacherClassHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TeacherClassRequirement requirement, string classIdstr)
        {
            try
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    context.Fail();
                    return;
                }

                var userId = userIdClaim.Value;


                // Check if the teacher owns the class
                var isOwner = await _applicationDbContext.AcademicTeamClass
                    .AnyAsync(tc => tc.AcademicTeam.UserId == userId && tc.ClassId == classIdstr);

                if (isOwner)
                    context.Succeed(requirement);
                else
                    context.Fail();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while giving authorization");
            }
        }
    }
}
