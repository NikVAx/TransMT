using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TrackMS.WebAPI.Shared.Settings;

public class SwaggerGenSettings
{
    public static Action<SwaggerGenOptions> Default { get; } = options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "TrackMS API",
            Version = "v1",
            Contact = new OpenApiContact
            {
                Email = "nik.vasilenko1203@gmail.com",
                Name = "Nikita Vasilenko",
            },
            Description = ""
        });

        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                Array.Empty<string>()
                }});

        options.DescribeAllParametersInCamelCase();
    };
}
