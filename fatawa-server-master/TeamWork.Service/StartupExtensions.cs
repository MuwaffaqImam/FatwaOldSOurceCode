using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TeamWork.Core.Services;
using TeamWork.Repository.DependencyInjection;
using TeamWork.Service.UnitOfWork;

namespace TeamWork.Service.DependencyInjection
{
    public static class StartupExtensions
    {
        public static void AddDependencyInjectionService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddDependencyInjectionRepository(configuration, hostEnvironment);
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
        }
    }
}