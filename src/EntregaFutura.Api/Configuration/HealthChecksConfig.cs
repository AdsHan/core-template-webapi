using EntregaFutura.Api.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntregaFutura.Api.Configuration
{
    public static class HealthChecksConfig
    {
        public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            // Adiciona o Health Checks
            services.AddHealthChecks()
                .AddMySql(configuration.GetConnectionString("DefaultConnection"), name: "Banco")
                .AddCheck("Entregas", new EntregasHealthCheck(configuration.GetConnectionString("DefaultConnection")));
            services.AddHealthChecksUI().AddInMemoryStorage();

            return services;
        }

        public static IApplicationBuilder UseHealthChecksConfiguration(this IApplicationBuilder app)
        {

            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/hc-ui";
                options.ApiPath = "/hc-ui-api";
            });

            return app;
        }
    }
}