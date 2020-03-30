using System;
using System.Collections.Generic;
using System.Linq;
using InvestmentManager.Core;
using InvestmentManager.DataAccess.AdoNet;
using InvestmentManager.DataAccess.Dapper;
using InvestmentManager.DataAccess.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace InvestmentManager
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
            services.AddControllersWithViews();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IConfiguration>(this.Configuration);

            // Configure the data access layer
            var connectionString = this.Configuration.GetConnectionString("InvestmentDatabase");

            services.RegisterEfDataAccessClasses(connectionString);  // For Entity Framework
            //services.RegisterAdoNetDataAccessClasses(connectionString);           // For ADO.NET Repositories
            //services.RegisterDapperDataAccessClasses(connectionString);           // For Dapper Repositories

            // For Application Services
            String stockIndexServiceUrl = this.Configuration["StockIndexServiceUrl"];
            services.ConfigureStockIndexServiceHttpClientWithoutProfiler(stockIndexServiceUrl);
            services.ConfigureInvestmentManagerServices(stockIndexServiceUrl);
            services.AddApplicationInsightsTelemetry();
        }


        // Configures the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Home/Error");
            

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
