using B1TestTask.BLLTask1.Contracts;
using B1TestTask.BLLTask1.Services;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.BLLTask1.Extensions
{
    public static class BLLTaks1RegistrationExtension
    {
        public static void RegisterBLLTask1Dependencies(this IServiceCollection services)
        {
            services.AddSingleton<ITask1FileService, Task1FileService>();            
        }
    }
}
