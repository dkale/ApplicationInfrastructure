using Microsoft.Extensions.DependencyInjection;

namespace DotNetStandard.DataAccess
{
    public static class DataAccessModule
    {
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services)
        {

            return services;
        }
    }
}
