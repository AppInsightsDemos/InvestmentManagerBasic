using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using NLog.Web;

namespace InvestmentManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        }).ConfigureLogging(logging =>
        //        {
        //            logging.ClearProviders();
        //            logging.AddApplicationInsights();
        //            logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);

        //        })
        //        .UseNLog();


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfigurationRoot config = null;

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration(configBuilder =>
                {
                    config = configBuilder.Build();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();

                    string appInsightsKey = config["ApplicationInsights:InstrumentationKey"];
                    logging.AddApplicationInsights(appInsightsKey);
                    logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);

                    logging.AddConsole();
                })
                .UseNLog();

            return hostBuilder;
        }
    }
}
