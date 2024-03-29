
using IdentityApi.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace IdentityApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(/*x => x.Filters.Add<ApiKeyAuthFilter>()*/);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "The API Key to access the API",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "x-api-key",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    { scheme, new List<string>() }
                };
                c.AddSecurityRequirement(requirement);
            });

            builder.Services.AddScoped<ApiKeyAuthFilter>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Disabled for not to redirect to HTTPS when testing App. Should be reenabled if launched.
            //app.UseHttpsRedirection();

            //app.UseMiddleware<ApiKeyAuthMiddleware>();

            app.UseAuthorization();
            //app.UseAuthentication();

            app.MapControllers();

            //var group = app.MapGroup("weather").AddEndpointFilter<ApiKeyEndpointFilter>();

            app.MapGet("testUser", () =>
            {
                return Enumerable.Range(1, 5).Select(index => new User
                {
                    UserName = "nicklas@selsmark.dk",
                    Password = "S3lsm4rk",
                }).ToArray();
            }).AddEndpointFilter<ApiKeyEndpointFilter>();

            app.Run();
        }
    }
}
