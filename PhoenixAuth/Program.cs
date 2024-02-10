using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;

namespace PhoenixAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            //https://docs.sentry.io/platforms/dotnet/aspnetcore/#senddefaultpii
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Extensions.Http.DefaultHttpClientFactory", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.With(new MachineNameEnricher(), new ProcessIdEnricher(), new ThreadIdEnricher())
                .WriteTo.Console()
                .WriteTo.File("logs/log-.log", LogEventLevel.Information, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 60, buffered: true)
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
                Log.Information("Fun after Starting up");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.Fatal("Finally clean up");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true);//For deployment
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");//For deployment
                    webBuilder.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });
                    webBuilder.UseStartup<Startup>();

                });
    }
}
