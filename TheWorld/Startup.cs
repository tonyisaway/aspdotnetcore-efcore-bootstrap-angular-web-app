﻿namespace TheWorld
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json.Serialization;

    using TheWorld.Models;
    using TheWorld.Services;
    using ViewModels;

    public class Startup
    {
        private readonly IHostingEnvironment env;

        private IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
            this.env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(this.env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            this.configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.configuration);

            if (this.env.IsEnvironment("Development") || this.env.IsEnvironment("Testing"))
            {
                services.AddScoped<IMailService, DebugMailService>();
            }

            services.AddDbContext<WorldContext>();
            services.AddScoped<IWorldRepository, WorldRepository>();

            services.AddTransient<GeoCoordinatesService>();
            services.AddTransient<WorldContextSeedData>();
            services.AddLogging();
            services.AddMvc(
                    config =>
                        {
                            if (this.env.IsProduction())
                            {
                                config.Filters.Add(new RequireHttpsAttribute());
                            }
                        })
                .AddJsonOptions(
                config =>
                    {
                        config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });

            services.AddIdentity<WorldUser, IdentityRole>(
                config =>
                    {
                        config.User.RequireUniqueEmail = true;
                        config.Password.RequiredLength = 8;
                        config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                        config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                                                                      {
                                                                          OnRedirectToLogin = async ctx =>
                                                                              {
                                                                                  if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                                                                                  {
                                                                                      ctx.Response.StatusCode = 401;
                                                                                  }
                                                                                  else
                                                                                  {
                                                                                      ctx.Response.Redirect(ctx.RedirectUri);
                                                                                  }
                                                                                  await Task.Yield();
                                                                              }
                                                                      };
                    }).AddEntityFrameworkStores<WorldContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            WorldContextSeedData seeder)
        {
            Mapper.Initialize(
                config =>
                    {
                        config.CreateMap<TripViewModel, Trip>().ReverseMap();
                        config.CreateMap<StopViewModel, Stop>().ReverseMap();
                    });

            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(
                config =>
                    {
                        config.MapRoute(
                            name: "Default",
                            template: "{controller}/{action}/{id?}",
                            defaults: new { controller = "App", action = "Index" });
                    });

            seeder.EnsureSeedData().Wait();
        }
    }
}
