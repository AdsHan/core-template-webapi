using ApiCatalogo.Repository;
using AutoMapper;
using DevIO.Api.Extensions;
using DevIO.Business.Intefaces;
using EntregaFutura.Api.Services;
using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.DTO.Mappings;
using EntregaFutura.Repository.Model;
using EntregaFutura.Repository.Repository;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

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

            // Adciona a política CORS
            services.AddCors();

            // Adciona o AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Adiciona os serviços do tipo Scoped
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<DataService>();

            // Adiciona os controladores ao projeto            
            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false).AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Adiciona o Context
            services.AddDbContext<ApiDbContext>(options =>
                   options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

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
                ValidAudience = Configuration["TokenConfiguration:Audience"],
                ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
            });

            // Adiciona o Versionamento
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            // Adiciona o Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Entrega Futura",
                    Description = "Aplicativo B2B para controle de entregas",

                    Contact = new OpenApiContact
                    {
                        Name = "<--ALTERACAO-->",
                        Email = "<--ALTERACAO-->",
                        //Url = new Uri("<--ALTERACAO-->"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usar sobre ASP"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                // Define o schema de segurança (Nome do schema que vamos passar no authorization do Swagger)
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });

            // Adiciona o Health Checks
            services.AddHealthChecks()
                .AddMySql(Configuration.GetConnectionString("DefaultConnection"), name: "Banco")
                .AddCheck("Entregas", new HealthCheck(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHealthChecksUI().AddInMemoryStorage();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod());

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Entrega Futura V1");
            });

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
