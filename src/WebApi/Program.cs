using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApi.Data;

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
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

            builder.Services
                .AddAuthentication(IdentityConstants.BearerScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapScalarApiReference();

            app.MapControllers();
            app.MapIdentityApi<AppUser>();
            app.MapGeneratedMediatorEndpoints();
            app.Run();
        }
    }
}
