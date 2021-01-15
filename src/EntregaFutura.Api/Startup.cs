using EntregaFutura.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntregaFutura.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApiConfiguration();

            services.AddIdentityConfiguration(Configuration);

            services.AddSwaggerConfiguration();

            services.AddHealthChecksConfiguration(Configuration);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseApiConfiguration(env);

            app.UseSwaggerConfiguration();

            app.UseSwaggerConfiguration();

            app.UseHealthChecksConfiguration();

        }
    }
}
