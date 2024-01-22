using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.DAL.Extensions
{
    public static class DALRegistrationExtension
    {
        public static void RegisterDALDependencies(this IServiceCollection services)
        {
            services.AddSingleton<B1TestTaskDBContext>();            
        }
    }
}
