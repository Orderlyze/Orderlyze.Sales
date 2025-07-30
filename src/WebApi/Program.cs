using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using WebApi.Data;
using WebApi.Helpers.Authentication;
using WebApi.Startup;
using WixApi;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddShinyMediator(x => x.AddGeneratedEndpoints());
            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("SalesDb")
            );
            builder.Services.AddControllers();
            
            // Add WixApi services
            builder.Services.AddWixApi(builder.Configuration);
            
            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .WithHeaders("wix-account-id", "wix-site-id", "Authorization", "Content-Type", "Accept", "X-Requested-With");
                });
            });
            
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(
                "v1",
                options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddDocumentTransformer<IdentityOperationIdTransformer>();
                }
            );

            builder
                .Services.AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

            builder
                .Services.AddAuthentication(IdentityConstants.BearerScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                    options
                        .AddPreferredSecuritySchemes("BearerAuth")
                        .AddHttpAuthentication(
                            "BearerAuth",
                            auth =>
                            {
                                auth.Token = "eyJhbGciOiJ...";
                            }
                        )
                );
            }

            // Disabled for local development with UnoApp
            // app.UseHttpsRedirection();
            
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapIdentityApi<AppUser>();
            app.FixOpenApiErrorShinyMediator();
            app.MapGeneratedMediatorEndpoints();
            app.Run();
        }
    }
}
