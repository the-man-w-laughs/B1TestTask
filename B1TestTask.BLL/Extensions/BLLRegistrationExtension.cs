using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Services.Database;
using B1TestTask.BLLTask2.Services.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.BLLTask2.Extensions
{
    public static class BLLRegistrationExtension
    {
        public static void RegisterDALDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ITrialBalanceParser, TrialBalanceParser>();
            services.AddSingleton<IFileService, FileService>();
        }
    }
}
