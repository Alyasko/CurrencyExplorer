using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Models;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Models.Entities.Database;
using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;

namespace CurrencyExplorer
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(appEnv.ApplicationBasePath)
                    .AddJsonFile("config.json")
                    .AddEnvironmentVariables();

                Configuration = builder.Build();
            }
            catch (FormatException e)
            {
                Debug.WriteLine($"Logged exception: {e.Message}");
                throw;
            }

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddMvc();

            string config = Configuration["Data:DefaultConnection:ConnectionString"];

            //services.AddEntityFramework()
            //    .AddSqlServer()
            //    .AddDbContext<CurrencyDataContext>(options =>
            //    {
            //        config = Configuration["Data:DefaultConnection:ConnectionString"];
            //        options.UseSqlServer(config);
            //    });


            services.AddScoped<CurrencyXplorer>();
            //services.AddScoped<ICachingProcessor, ApiDatabaseCachingProcessor>();
            //services.AddScoped<ICurrencyProvider, NationalBankCurrencyProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //var appSettings = app.ApplicationServices.GetService<IOptions<AppSettings>>();
            //string s = appSettings.Value.SiteTitle;

            app.UseStaticFiles();
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                    );
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
