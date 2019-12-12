
using Autofac;
using DotNetFramework.Infrastructure.Services;

namespace DotNetFramework.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClientHelperService>().As<IHttpClientHelperService>().PropertiesAutowired();
        }
    }
}
