using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TeamWork.Core.DependencyInjection;
using TeamWork.Core.Repository;
using TeamWork.Repository.DataContext;
using TeamWork.Repository.Repository;

namespace TeamWork.Repository.DependencyInjection
{
    public static class StartupExtensions
    {
        public static void AddDependencyInjectionRepository(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            IdentityBuilder builder = services.AddDependencyInjectionCore(configuration);
            builder.AddEntityFrameworkStores<TeamDataContext>();
            services.AddDbContext<TeamDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AppConnectionString"));

                if (hostEnvironment.IsDevelopment())
                    options.EnableSensitiveDataLogging(true);
            });
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IDbFactory), typeof(DbFactory));
        }
    }
}