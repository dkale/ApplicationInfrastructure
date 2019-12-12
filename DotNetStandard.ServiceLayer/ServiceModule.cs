using DotNetStandard.Infrastructure.DbPolly;
using DotNetStandard.ServiceLayer.Models;
using DotNetStandard.ServiceLayer.NSwagClients;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NReco.PdfGenerator;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace DotNetStandard.ServiceLayer
{
    public static class ServiceModule
    {
        public static IServiceCollection AddServiceModule(this IServiceCollection services)
        {
            // retry policies
            PolicyBuilder policyBuilder = Policy.Handle<Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn);
            const int retryCount = 5;
            TimeSpan SleepDurationProvider(int retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2d, retryAttempt));
            services.AddPolicyDbClient(
                policyBuilder.WaitAndRetryAsync(retryCount, SleepDurationProvider),
                policyBuilder.WaitAndRetry(retryCount, SleepDurationProvider));

            services.AddTransient(CreateHtmlToPdfConverter);

            services.AddHttpClient("NSwag Client")
               .ConfigureHttpClient((provider, client) =>
               {
                   client.Timeout = TimeSpan.FromMinutes(5);
                   client.BaseAddress = provider.GetRequiredService<IOptions<WebApiUri>>().Value.WiresService;
               })
               //.AddHttpMessageHandler<CustomHttpMessageHandler>()
               .ConfigureHttpMessageHandlerBuilder(builder =>
               {
                   builder.PrimaryHandler = new HttpClientHandler
                   {
                       UseDefaultCredentials = true,
                       AllowAutoRedirect = false,
                       MaxAutomaticRedirections = 50,
                   };
               }).AddTypedClient<IGeneratedNSwagClient, GeneratedNSwagClient>()
               .SetHandlerLifetime(TimeSpan.FromMinutes(10))
               .AddPolicyHandler(GetRetryPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(retryCount: 6, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static HtmlToPdfConverter CreateHtmlToPdfConverter(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<NRecoLicenseOptions>>();
            var converter = new HtmlToPdfConverter();
            converter.License.SetLicenseKey(options.Value.LicenseOwner, options.Value.LicenseKey);
            converter.Orientation = PageOrientation.Portrait;
            converter.Size = PageSize.Letter;
            return converter;
        }
    }
}