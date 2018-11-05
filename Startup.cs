using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation;
using testWebAPI.DBs;
using testWebAPI.Filters;
using testWebAPI.Formatters;
using testWebAPI.Infrastructure;
using testWebAPI.Models;
using testWebAPI.Models.Entities;
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
            // Add mysql db
            services.AddTransient<AppDb>(_ => new AppDb());

            // Add in-memory db
            // TODO: Swap this with a real db in production
            services.AddDbContext<HotelApiContext>(opt => 
            { 
                opt.UseInMemoryDatabase(databaseName: "testInMemDb"); 
                opt.UseOpenIddict(); // use openIddict for tokens
            });

            services.AddOpenIddict()
            // Register the OpenIddict core services.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the EF Core stores/models.
                options.UseEntityFrameworkCore().UseDbContext<HotelApiContext>();
            })
            // Register the OpenIddict server handler.
            .AddServer(options =>
            {
                // Register the ASP.NET Core MVC services used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.UseMvc();
                // Enable the token endpoint.
                options.EnableTokenEndpoint("/api/token");
                // Enable the password flow.
                options.AllowPasswordFlow();
                // Accept anonymous clients (i.e clients that don't send a client_id).
                options.AcceptAnonymousClients();
                //// During development, you can disable the HTTPS requirement.
                //options.DisableHttpsRequirement();
            })
            // Register the OpenIddict validation handler.
            // Note: the OpenIddict validation handler is only compatible with the
            // default token format or with reference tokens and cannot be used with
            // JWT tokens. For JWT tokens, use the Microsoft JWT bearer handler.
            .AddValidation();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });

            // Map some of the default claim names to the proper OpenID Connect claim names
            services.Configure<IdentityOptions>(opt =>
            {
                opt.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                opt.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                opt.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Add ASP.NET Core Identity
            services.AddIdentity<UserEntity, UserRoleEntity>()
                    .AddEntityFrameworkStores<HotelApiContext>()
                    .AddDefaultTokenProviders();

            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Add Response Caching
            services.AddResponseCaching();

            // Initialize mapper
            Mapper.Initialize(mp => mp.AddProfile<MappingProfile>());

            services.AddMvc(opt =>
            {
                // Add Filters
                opt.Filters.Add(typeof(JsonExceptionFilter));
                opt.Filters.Add(typeof(LinkRewritingFilter));

                // configure the Ion formatter, more details about Ion Specification vist https://ionspec.org/
                var jsonOutputFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                opt.OutputFormatters.Remove(jsonOutputFormatter);
                opt.OutputFormatters.Add(new IonOutputFormatter(jsonOutputFormatter));

                // add cache profile for cache-control header
                // "Static" is a name for any response you want the client to cache
                opt.CacheProfiles.Add("Static", new CacheProfile
                {
                    Duration = 86400 // full day
                });
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

            // use lower case controller names
            services.AddRouting(opt => opt.LowercaseUrls = true);

            // Load data from the configuration file and put data into IOption
            services.Configure<HotelOptions>(_configurationRoot);
            services.Configure<HotelInfo>(_configurationRoot.GetSection("Info"));
            services.Configure<PagingOptions>(_configurationRoot.GetSection("DefaultPagingOptions"));

            // configure Servces for Dependency Injection
            // Since EF DbContext has scoped lifetime, any service using it has to be scoped as well
            services.AddScoped<IRoomService, DefaultRoomService>();
            services.AddScoped<IOpeningService, DefaultOpeningService>();
            services.AddScoped<IBookingService, DefaultBookingService>();
            services.AddScoped<IDateLogicService, DefaultDateLogicService>();
            services.AddScoped<IUserService, DefaultUserService>();
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
                    // Add test roles and users
                    var roleManager = scope.ServiceProvider
                        .GetRequiredService<RoleManager<UserRoleEntity>>();
                    var userManager = scope.ServiceProvider
                        .GetRequiredService<UserManager<UserEntity>>();
                    AddTestUsers(roleManager, userManager).Wait();

                    var dbContext = scope.ServiceProvider.GetRequiredService<HotelApiContext>();
                    var dateLogicService = scope.ServiceProvider.GetRequiredService<IDateLogicService>();

                    // Add test data
                    SeedTestData(dbContext, dateLogicService);
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

            app.UseAuthentication();

            // The order here is very important
            // The response caching middle-ware has to handle the request before MVC
            app.UseResponseCaching();
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Something went wrong!");
            //});
        }

        private static async Task AddTestUsers(
            RoleManager<UserRoleEntity> roleManager,
            UserManager<UserEntity> userManager)
        {
            // Add a test role
            await roleManager.CreateAsync(new UserRoleEntity("Admin"));

            // Add a test user
            var user = new UserEntity
            {
                Email = "admin@landon.local",
                UserName = "admin@landon.local",
                FirstName = "Admin",
                LastName = "Testerman",
                CreatedAt = DateTimeOffset.UtcNow
            };

            await userManager.CreateAsync(user, "Supersecret123!!");

            // Put the user in the admin role
            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.UpdateAsync(user);
        }

        private static void SeedTestData(HotelApiContext context, IDateLogicService dateLogicService)
        {
            var oxford = context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("301df04d-8679-4b1b-ab92-0a586ae53d08"),
                Name = "Oxford Suite",
                Rate = 10119,
            }).Entity;

            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("ee2b83be-91db-4de5-8122-35a9e9195976"),
                Name = "Driscoll Suite",
                Rate = 23959
            });

            var today = DateTimeOffset.Now;
            var start = dateLogicService.AlignStartTime(today);
            var end = start.Add(dateLogicService.GetMinimumStay());

            context.Bookings.Add(new BookingEntity
            {
                Id = Guid.Parse("2eac8dea-2749-42b3-9d21-8eb2fc0fd6bd"),
                Room = oxford,
                CreatedAt = DateTimeOffset.UtcNow,
                StartAt = start,
                EndAt = end,
                Total = oxford.Rate,
            });

            context.SaveChanges();
        }
    }
}
