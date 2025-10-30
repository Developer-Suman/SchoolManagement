﻿using Microsoft.OpenApi.Models;

namespace TN.Web.Configs
{
    public static class ApplicationBuilderConfig
    {
        public static void Inject(WebApplicationBuilder builder)
        {
            #region ConfigureSwaggerForAuthentication
            builder.Services.AddSwaggerGen(
            option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "SCHOOLAPP API", Version = "V1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                    });
            }
            );
            #endregion

        }
    }
}
