using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetStandard.Infrastructure.UnitTests
{
    public class BaseService
    {
        protected readonly ServiceProvider ServiceProvider;
        protected readonly IConfiguration Configuration;
    }
}
