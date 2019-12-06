using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DotNetFramework.Infrastructure.Services
{
    public class HttpClientHelperService : IHttpClientHelperService
    {
        public ILog Log { get; set; }

        public async Task<TResponse> Get<TResponse>(string url, string path, NameValueCollection queryParams = null, int requestTimeoutInSeconds = 0)
        {
            using (var handler = new HttpClientHandler { UseDefaultCredentials = true })
            {
                using (var client = ResolveClient(handler, requestTimeoutInSeconds))
                {
                    var requestUri = ResolveUri(url, path, queryParams);

                    Log.Info($"GET Request URL: {requestUri.ToString()}");

                    var response = await client.GetAsync(requestUri);

                    return await ResolveResponse<TResponse>(response);
                }
            }
        }

        public async Task<TResponse> Post<TResponse>(string url, string path, object request, NameValueCollection queryParams = null, Dictionary<string, string> customHeaders = null)
        {
            using (var handler = new HttpClientHandler { UseDefaultCredentials = true })
            {
                using (var client = ResolveClient(handler))
                {
                    var requestUri = ResolveUri(url, path, queryParams);

                    var requestContent = new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8,
                        "application/json");

                    // Set Headers
                    if (customHeaders != null)
                    {
                        foreach (var _header in customHeaders)
                        {
                            if (client.DefaultRequestHeaders.Contains(_header.Key))
                            {
                                client.DefaultRequestHeaders.Remove(_header.Key);
                            }
                            client.DefaultRequestHeaders.TryAddWithoutValidation(_header.Key, _header.Value);
                        }
                    }
                    var response = await client.PostAsync(requestUri, requestContent);

                    return await ResolveResponse<TResponse>(response);
                }
            }
        }

        public async Task<HttpResponseMessage> SendAsync(string url, string path, object request, Dictionary<string, string> customHeaders = null, NameValueCollection queryParams = null, CancellationToken cancellationToken = default)
        {
            using (var handler = new HttpClientHandler { UseDefaultCredentials = true })
            {
                using (var client = ResolveClient(handler))
                {
                    var requestUri = ResolveUri(url, path, queryParams);

                    var requestContent = new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8,
                        "application/json");

                    var _httpRequest = new HttpRequestMessage
                    {
                        Method = new HttpMethod("POST"),
                        RequestUri = requestUri
                    };
                    // Set Headers
                    if (customHeaders != null)
                    {
                        foreach (var _header in customHeaders)
                        {
                            if (_httpRequest.Headers.Contains(_header.Key))
                            {
                                _httpRequest.Headers.Remove(_header.Key);
                            }
                            _httpRequest.Headers.TryAddWithoutValidation(_header.Key, _header.Value);
                        }
                    }
                    // Serialize Request
                    string _requestContent = null;
                    if (request != null)
                    {
                        _requestContent = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        _httpRequest.Content = new StringContent(_requestContent, Encoding.UTF8);
                        _httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    //_httpResponse = await client.SendAsync(_httpRequest, cancellationToken).ConfigureAwait(false);

                    //return await ResolveResponse<TResponse>(_httpResponse);

                    client.Timeout = Timeout.InfiniteTimeSpan;
                    return await client.SendAsync(_httpRequest, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public async Task<TResponse> SendAsync<TResponse>(string url, string path, object request, Dictionary<string, string> customHeaders = null, NameValueCollection queryParams = null, CancellationToken cancellationToken = default)
        {
            var httpResponse = await SendAsync(url, path, request, customHeaders, queryParams, cancellationToken);

            return await ResolveResponse<TResponse>(httpResponse);
        }

        private HttpClient ResolveClient(HttpMessageHandler handler, int timeoutInSecs = 0)
        {
            var client = new HttpClient(handler);

            if (timeoutInSecs != 0)
                client.Timeout = TimeSpan.FromSeconds(timeoutInSecs);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private Uri ResolveUri(string url, string path, NameValueCollection queryParams)
        {
            var collection = HttpUtility.ParseQueryString(string.Empty);
            if (queryParams != null)
            {
                foreach (var key in queryParams.Cast<string>())
                {
                    collection[key] = queryParams[key];
                }
            }
            var builder = new UriBuilder($@"{url}/{path}") { Query = collection.ToString() };
            return builder.Uri;
        }

        private async Task<TResponse> ResolveResponse<TResponse>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Exception in REST service call", new Exception(content));
            }

            return JsonConvert.DeserializeObject<TResponse>(content);
        }
    }
}
