using B1TestTask.Presentation.MVVM;
using B1TestTask.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.DALTask2.Extensions
{
    public static class PresentationRegistrationExtension
    {
        public static void RegisterPresentationDependencies(this IServiceCollection services)
        {
            services.AddSingleton<App>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MainWindow>();
        }
    }
}
