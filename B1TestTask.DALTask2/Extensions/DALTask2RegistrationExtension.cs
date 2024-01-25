using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.DALTask2.Extensions
{
    public static class DALTask2RegistrationExtension
    {
        public static void RegisterDALTask2Dependencies(this IServiceCollection services)
        {
            services.AddSingleton<B1TestTask2DBContext>();
            services.AddSingleton<IFileModelRepository, FileModelRepository>();
            services.AddSingleton<IClassModelRepository, ClassModelRepository>();
            services.AddSingleton<IAccountGroupModelRepository, AccountGroupModelRepository>();
            services.AddSingleton<IAccountModelRepository, AccountModelRepository>();
        }
    }
}
