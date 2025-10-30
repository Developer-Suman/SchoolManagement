﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAcademicsInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;

            #endregion
        }
    }
}
