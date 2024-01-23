using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.DALTask2.Extensions
{
    public static class DALRegistrationExtension
    {
        public static void RegisterDALDependencies(this IServiceCollection services)
        {
            services.AddSingleton<B1TestTask2DBContext>();
            services.AddSingleton<IFileRepository, FileRepository>();
        }
    }
}
