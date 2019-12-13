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
    /// <summary>
    /// Foundation class for Test Classes to inherit from. 
    /// You could have another base class (ServiceTestBase.cs) ineheriting from this class.
    /// The ServiceTestBase.cs class shall serve as the base class for further service specific test classes.
    /// The ServiceTestBase.cs class will further register dependencies by either calling 
    /// other application specific service collections extensions (ex: .AddDataAccessModule(), .AddServiceModule())
    /// which will in turn register application dependencies.
    /// ServiceTestBase.cs will also register the DbContext with in-memory database (ex: serviceCollection.AddDbContext<EmployeeDbContext>(options => options.UseInMemoryDatabase("Employees_Db"))).
    /// 
    /// </summary>
    public abstract class AbstractTestBase
    {
        /// <summary>
        /// Exposes the IServiceCollection instance for derived classes to register their dependencies.
        /// </summary>
        public IServiceCollection ServiceCollection { get; private set; }

        /// <summary>
        /// ServiceProvider instance after the ServiceCollection is built.
        /// </summary>
        protected ServiceProvider ServiceProvider;
        protected IConfiguration Configuration;
        private ConfigurationBuilder _configurationBuilder;

        public AbstractTestBase()
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
