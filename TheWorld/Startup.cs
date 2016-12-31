namespace TheWorld
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using TheWorld.Services;

    public class Startup
    {
        private readonly IHostingEnvironment env;

        private IConfigurationRoot config;

        public Startup(IHostingEnvironment env)
        {
            this.env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(this.env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            this.config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.config);

            if (this.env.IsEnvironment("Development") || this.env.IsEnvironment("Testing"))
            {
                services.AddScoped<IMailService, DebugMailService>();
            }

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (this.env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(
                config =>
                    {
                        config.MapRoute(
                            name: "Default",
                            template: "{controller}/{action}/{id?}",
                            defaults: new { controller = "App", action = "Index" });
                    }); 
        }
    }
}
