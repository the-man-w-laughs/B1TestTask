using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace B1TestTask.BLLTask2.Extensions
{
    public static class AutoMapperRegistrationExtensions
    {
        public static void RegisterAutomapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
