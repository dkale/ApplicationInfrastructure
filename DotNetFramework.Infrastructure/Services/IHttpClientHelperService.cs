using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetFramework.Infrastructure.Services
{
    public interface IHttpClientHelperService
    {
        Task<TResponse> Get<TResponse>(string url, string path, NameValueCollection queryParams = null, int requestTimeoutInSeconds = 0);

        Task<TResponse> Post<TResponse>(string url, string path, object request, NameValueCollection queryParams = null, Dictionary<string, string> customHeaders = null);

        Task<HttpResponseMessage> SendAsync(string url, string path, object request, Dictionary<string, string> customHeaders = null, NameValueCollection queryParams = null, CancellationToken cancellationToken = default);

        Task<TResponse> SendAsync<TResponse>(string url, string path, object request, Dictionary<string, string> customHeaders = null, NameValueCollection queryParams = null, CancellationToken cancellationToken = default);
    }
}
