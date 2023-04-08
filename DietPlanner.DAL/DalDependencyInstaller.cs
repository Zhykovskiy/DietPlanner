using Math.DAL.Abstract.Repository;
using Math.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DietPlanner.DAL;

public static class DalDependencyInjection
{
    public static void InstallRepositories(this IServiceCollection services)
    {
        services.AddTransient<IAppUserRepository, AppUserRepository>();
    
    }
}