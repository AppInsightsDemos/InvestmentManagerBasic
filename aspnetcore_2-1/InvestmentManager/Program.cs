using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using NLog.Extensions.Logging;
using NLog.Web;

namespace InvestmentManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseKestrel()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .ConfigureAppConfiguration((hostingEnv, configBuilder) =>
        //            configBuilder.SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //                .AddJsonFile($"appsettings.{hostingEnv.HostingEnvironment.EnvironmentName}.json", optional: true)
        //                .AddEnvironmentVariables().Build()
        //        )
        //        .ConfigureLogging(logging => {
        //            logging.ClearProviders();
        //            logging.AddApplicationInsights("instrumentation key")
        //            .AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
        //        })
        //        .UseSetting("detailedErrors", "true")
        //        .UseIISIntegration()
        //        .UseStartup<Startup>();



        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            IConfigurationRoot config = null;

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingEnv, configBuilder) =>
                {
                    config = configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingEnv.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables().Build();
                    
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    
                    string appInsightsKey = config["ApplicationInsights:InstrumentationKey"];
                    logging.AddApplicationInsights(appInsightsKey);
                    logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
                    logging.AddNLog();
                    logging.AddConsole();
                })
                .UseSetting("detailedErrors", "true")
                .UseIISIntegration()
                .UseStartup<Startup>();

            return webHostBuilder;
        }

    }
}
