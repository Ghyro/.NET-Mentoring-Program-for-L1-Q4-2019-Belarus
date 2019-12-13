using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Potestas.Analyzers;
using Potestas.Context;
using Potestas.Interfaces;
using Potestas.Storages;
using Potestas.Web.Interfaces;
using Potestas.Web.Mappers;
using Potestas.Web.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Potestas.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            services.AddCors();

            services.AddSingleton(ConfigureMapper());
            services.AddScoped<IEnergyObservationService, EnergyObservationService>();
            services.AddScoped<IEnergyObservationStorage<IEnergyObservation>, BsonStorage<IEnergyObservation>>();
            services.AddScoped<IEnergyObservationAnalyzer<IEnergyObservation>, BsonAnalyzer<IEnergyObservation>>();
            services.AddScoped<IAnalyzer, Analyzer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseMvc();
        }

        private static IMapper ConfigureMapper()
        {
            var mapperConfig = new MapperConfiguration(m => m.AddProfile(new EnergyObservationMapper()));
            return mapperConfig.CreateMapper();
        }
    }
}
