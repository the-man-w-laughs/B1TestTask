using B1TestTask.DALTask1.Contracts;
using B1TestTask.DALTask1.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.DALTask1.Extensions
{
    public static class DALTask1RegistrationExtension
    {
        public static void RegisterDALTask1Dependencies(this IServiceCollection services)
        {
            services.AddSingleton<B1TestTask1DBContext>();
            services.AddSingleton<IGeneratedDataModelRepository, GeneratedDataModelRepository>();            
        }
    }
}
