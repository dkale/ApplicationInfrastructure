using DotNetStandard.DataAccess;
using DotNetStandard.Infrastructure.UnitTests;
using DotNetStandard.ServiceLayer;

namespace DotNetCore.UnitTest.Sample
{
    public class ServiceTestBase : AbstractTestBase
    {
        public ServiceTestBase()
        {
            ServiceCollection
                .AddServiceModule()
                .AddDataAccessModule();
        }
    }
}
