using Microsoft.Extensions.DependencyInjection;
using ST.SolutionHub.DataLayer.Repositories;
using ST.SolutionHub.DataLayer.Repositories.Abstracts;

namespace ST.SolutionHub.DataLayer
{
    public static class RepositoriesServicesExtension
    {
        public static IServiceCollection ConfigureRepositoriesServices(this IServiceCollection services)
        {
            services.AddScoped<IRefreshTokenRepositry, RefreshTokenRepositry>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();

            return services;
        }
    }
}
