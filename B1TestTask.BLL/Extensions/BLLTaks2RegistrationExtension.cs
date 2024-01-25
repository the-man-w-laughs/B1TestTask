using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Services.Database;
using B1TestTask.BLLTask2.Services.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.BLLTask2.Extensions
{
    public static class BLLTaks2RegistrationExtension
    {
        public static void RegisterBLLTask2Dependencies(this IServiceCollection services)
        {
            services.AddSingleton<ITrialBalanceParser, TrialBalanceParser>();
            services.AddSingleton<IFileModelService, FileModelService>();
            services.RegisterAutomapperProfiles();
        }
    }
}
