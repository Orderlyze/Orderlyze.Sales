using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace WebApi.Helpers.Authentication
{
    internal sealed class BearerSecuritySchemeTransformer(
        IAuthenticationSchemeProvider authenticationSchemeProvider
    ) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken
        )
        {
            var schemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            // Check for the Identity Bearer scheme (IdentityConstants.BearerScheme) or "Bearer"
            if (schemes.Any(s => s.Name == IdentityConstants.BearerScheme || s.Name == "Bearer"))
            {
                const string schemeName = "BearerAuth";
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??=
                    new Dictionary<string, OpenApiSecurityScheme>();
                document.Components.SecuritySchemes[schemeName] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Enter JWT token in the format: Bearer {your token}",
                };
                document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();
                document.SecurityRequirements.Add(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = schemeName,
                                },
                            },
                            Array.Empty<string>()
                        },
                    }
                );
            }
        }
    }
}
