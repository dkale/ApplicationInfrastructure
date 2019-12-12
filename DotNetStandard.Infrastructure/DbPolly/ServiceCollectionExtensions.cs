using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStandard.Infrastructure.DbPolly
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPolicyDbClient(this IServiceCollection services, IAsyncPolicy asyncPolicy, ISyncPolicy syncPolicy)
        {
            services.TryAddTransient<IDbConnectionFactory, SqlConnectionFactory>();

            List<ServiceDescriptor> descriptors = services.Where(d => d.ServiceType == typeof(IDbConnectionFactory)).ToList();

            foreach (ServiceDescriptor descriptor in descriptors)
            {
                services.Remove(descriptor);
                services.Add(new ServiceDescriptor(
                    typeof(IDbConnectionFactory),
                    provider =>
                    {
                        object factory = GetInstance(provider, descriptor);
                        return new PolicyDbConnectionFactory((IDbConnectionFactory)factory, asyncPolicy, syncPolicy);
                    },
                    ServiceLifetime.Transient));
            }
        }

        private static object GetInstance(IServiceProvider provider, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationType != null)
            {
                return ActivatorUtilities.CreateInstance(provider, descriptor.ImplementationType);
            }

            return descriptor.ImplementationFactory(provider);
        }
    }
}