using AutoMapper;
using DotNetStandard.Infrastructure.Extensions;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;

namespace DotNetStandard.Infrastructure.UnitTests
{
    public abstract class ServiceTestBase
    {
        public IServiceCollection ServiceCollection { get; set; }
        protected ServiceProvider ServiceProvider;
        protected IConfiguration Configuration;
        private ConfigurationBuilder _configurationBuilder;

        public ServiceTestBase()
        {
            Initialize();
        }

        private void Initialize()
        {
            Configuration = _configurationBuilder.Build();
            ServiceCollection = new ServiceCollection();
            ServiceCollection.AddSingleton(Configuration);

            ServiceCollection.AddOData();
            ServiceCollection.AddODataQueryFilter();
            ServiceCollection.AddTransient<ODataUriResolver>();
            ServiceCollection.AddTransient<ODataQueryValidator>();
            ServiceCollection.AddTransient<TopQueryValidator>();

            ServiceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public static HttpRequest SetupMockOdataProvider(string uriString, IServiceProvider serviceProvider)
        {
            var uri = new Uri(uriString);

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            var httpRequest = new DefaultHttpRequest(httpContext)
            {
                Method = "GET",
                Host = new HostString(uri.Host, uri.Port),
                Path = uri.LocalPath,
                QueryString = new QueryString(uri.Query)
            };

            var routeBuilder = new RouteBuilder(new ApplicationBuilder(serviceProvider));
            //routeBuilder.EnableDependencyInjection();

            return httpRequest;
        }

        public void AddJsonTestDataFiles(List<string> testDataFiles)
        {
            _configurationBuilder = new ConfigurationBuilder();
            if (!testDataFiles.IsNullOrEmpty())
            {
                testDataFiles.ForEach(file => _configurationBuilder.AddJsonFile(file));
            }
        }
    }
}
