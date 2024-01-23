using B1TestTask.DALTask2.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace B1TestTask.Presentation
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            var app = ServiceProvider.GetService<App>();
            app?.Run();

        }

        public static IServiceProvider? ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
                .ConfigureServices((context, services) => {
                    services.RegisterPresentationDependencies();
                    services.RegisterDALDependencies();
                });
        }
    }
}
