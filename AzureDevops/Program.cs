using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace AzureDevops
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Build()).Enrich.FromLogContext().WriteTo.Console().CreateLogger();
            Settings.ExcelFile = "d:\\devops\\data.xlsx";
            MembersToExcel();
            //GetProjectAdmins();

        }

        private static void GetProjectAdmins()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IAzureDevopsService, ProjectAdmins>();
                })
                .UseSerilog().Build();

            var svc = ActivatorUtilities.CreateInstance<ProjectAdmins>(host.Services);
            svc.Run();
        }

        private static void MembersToExcel()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IAzureDevopsService, MembersInDevops>();
                })
                .UseSerilog().Build();

            var svc = ActivatorUtilities.CreateInstance<MembersInDevops>(host.Services);
            svc.Run();
        }
        private static void GetStatistics()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IAzureDevopsService, StatisticsFromDevops>();
                })
                .UseSerilog().Build();

            var svc = ActivatorUtilities.CreateInstance<StatisticsFromDevops>(host.Services);
            svc.Run();
        }


        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional:true)
                .AddEnvironmentVariables();
        }



    }
}
