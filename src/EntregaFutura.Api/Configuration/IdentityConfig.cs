using EntregaFutura.Api.Extensions;
using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EntregaFutura.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            // Adiciona o Context
            services.AddDbContext<ApiDbContext>(options =>
                   options.UseMySql(configuration.GetConnectionString("DefaultConnection")));

            // Adiciona o Identity
            services.AddIdentity<UsuarioModel, RegraModel>()
                  .AddErrorDescriber<IdentityPortugues>()
                  .AddEntityFrameworkStores<ApiDbContext>()
                  .AddDefaultTokenProviders();

            // Adiciona o JWT Bearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = configuration["TokenConfiguration:Audience"],
                ValidIssuer = configuration["TokenConfiguration:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
            });

            return services;
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {

            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}