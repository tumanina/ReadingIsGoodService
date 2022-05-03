using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ReadingIsGoodService.Common.Authentication;
using ReadingIsGoodService.Common.Extensions;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Customers.Logic.Configuration;
using ReadingIsGoodService.Data.Configuration;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReadingIsGoodService.Customers.Api
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
            services.AddApplicationInsightsTelemetry();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            ConfigureSwagger(services);

            services.ConfigureAuth();
            services.ConfigureDataLayer(Configuration);
            services.ConfigureLogic();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customers Api v1"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                //todo use different exceptions to cover 404, 400 and etc
                context.Response.StatusCode = 200;
                var message = new BaseApiModel { Errors = new List<string> { exception.GetMessage() } };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(message));
            }));
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customers Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer Token", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header Token. Enter : \"Bearer YourTokenHere\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer Token",
                    BearerFormat = "JWT",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer Token"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }

    }
}
