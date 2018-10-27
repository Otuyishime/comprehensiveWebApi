using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using testWebAPI.DBs;
using testWebAPI.Filters;
using testWebAPI.Formatters;
using testWebAPI.Infrastructure;
using testWebAPI.Models.Resources;
using testWebAPI.Models.Services;

namespace testWebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env){
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configurationRoot = builder.Build();
        }

        public IConfigurationRoot _configurationRoot { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Initialize mapper
            Mapper.Initialize(mp => mp.AddProfile<MappingProfile>());

            services.AddMvc(opt =>
            {
                // Add the JsonException Filter
                opt.Filters.Add(typeof(JsonExceptionFilter));

                // configure the Ion formatter, more details about Ion Specification vist https://ionspec.org/
                var jsonOutputFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                opt.OutputFormatters.Remove(jsonOutputFormatter);
                opt.OutputFormatters.Add(new IonOutputFormatter(jsonOutputFormatter));
            });

            // add api versioning
            services.AddApiVersioning(opt => {
                opt.ApiVersionReader = new MediaTypeApiVersionReader();
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;   // report api version in headers in response
                opt.DefaultApiVersion = new ApiVersion(1, 0);

                // if no version is requested by the client, use the highest/newest version of a route
                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
            });

            // Load data from the configuration file and put data into IOption
            services.Configure<HotelInfo>(_configurationRoot.GetSection("Info"));

            // Add mysql db
            services.AddTransient<AppDb>(_ => new AppDb());

            // Add in-memory db
            // TODO: Swap this with a real db in production
            services.AddDbContext<HotelApiContext>(opt => opt.UseInMemoryDatabase(databaseName: "testInMemDb"));

            // use lower case controller names
            services.AddRouting(opt => opt.LowercaseUrls = true);

            // configure Servces for Dependency Injection
            // Since EF DbContext has scoped lifetime, any service using it has to be scoped as well
            services.AddScoped<IRoomService, DefaultRoomService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configurationRoot.GetSection("logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Seed some test data in development
            if (env.IsDevelopment())
            {
                // The DbContext is always added as a scoped element
                // A new scope has to be created from the scope factory
                var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HotelApiContext>();
                    // Add test data
                    SeedTestData(dbContext);
                }
            }

            // Add Strict-Transport-Security Header: form nwebsec.aspnetcore.middleware nuget package
            app.UseHsts(opt => {
                // Set how long the browser will remember HSTS setting
                // Common to set this value very high such as a Year
                opt.MaxAge(days: 180);
                // Include not only the main domain but also the sub-domains
                opt.IncludeSubdomains();
                // Allow the browser to assume the api uses HSTS if it's added to common list of HSTS enabled websites
                opt.Preload();
            });

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Something went wrong!");
            });
        }

        private static void SeedTestData(HotelApiContext context)
        {
            context.Rooms.Add(new Models.Entities.RoomEntity
            {
                Id = Guid.Parse("1f232c71-e314-482b-9272-a2ef02db8ce2"),
                Name = "MainTown Suite",
                Rate = 13499
            });
            context.Rooms.Add(new Models.Entities.RoomEntity {
                Id = Guid.Parse("7bcc1f44-b9c7-4dc6-8fbd-cea92b190802"),
                Name = "Memorial Lounge",
                Rate = 15499
            });

            context.SaveChanges();
        }
    }
}
