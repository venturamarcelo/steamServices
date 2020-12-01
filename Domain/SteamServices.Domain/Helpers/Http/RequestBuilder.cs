using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SteamServices.Domain.Helpers.Http.Configurations;
using SteamServices.Domain.Helpers.Http.Implementations;
using SteamServices.Domain.Helpers.Http.Interfaces;

namespace SteamServices.Domain.Helpers.Http
{
    public class RequestBuilder
    {
        private readonly RequestConfiguration _configuration;

        internal RequestBuilder(RequestConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Creates a request based on request configuration class.
        /// Its also possible to configure httpclient as you wish and pass.
        /// Futhermore you can create your on implementation for retries.  
        /// </summary>
        /// <typeparam name="TEntity">Parameters type</typeparam>
        /// <param name="configuration"> Configuration of request</param>
        /// <param name="httpClientConfigurations">Pass your own httpclient configuration</param>
        /// <param name="retryHandler">Case you want to create a custom retry implementation</param>
        /// <returns></returns>
        public static IHttpMethod<TEntity> Build<TEntity>(RequestConfiguration configuration, Action<HttpClient> httpClientConfigurations = null, IHttpRequestRetryHandler retryHandler = null, ILogger logger=null)
        {

            var requestBuilder = new RequestBuilder(configuration);
            var services = requestBuilder.ConfigureServices(httpClientConfigurations, retryHandler);
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = requestBuilder.GetService<IHttpClientFactory>(serviceProvider);
            return requestBuilder.Build<TEntity>(httpClientFactory,logger);
        }
        /// <summary>
        /// Creates a request based on request configuration class. Where parameters will be passed as string.<para />
        /// Its also possible to configure httpclient as you wish and pass.<para />
        /// Futhermore you can create your on implementation for retries.  <para />
        /// </summary>
        /// <param name="configuration"> Configuration of request</param>
        /// <param name="httpClientConfigurations">Pass your own httpclient configuration</param>
        /// <param name="retryHandler">Case you want to create a custom retry implementation</param>
        /// <returns>IHttpMethod</returns>
        public static IHttpMethod<string> Build(RequestConfiguration configuration,
            Action<HttpClient> httpClientConfigurations = null, IHttpRequestRetryHandler retryHandler = null)
        {
            return Build<string>(configuration, httpClientConfigurations, retryHandler);
        }

        private IHttpMethod<TEntity> Build<TEntity>(IHttpClientFactory httpClientFactory,ILogger logger)
        {
            switch (_configuration.HttpMethod.ToUpperInvariant())
            {
                case "POST":
                    return new HttpRequestPost<TEntity>(
                        client: CreateHttpClient(httpClientFactory),
                        addressSuffix: _configuration.RequestSufix,
                        logger:logger);
                case "GET":
                    return new HttpRequestGet(
                        client: CreateHttpClient(httpClientFactory),
                        addressSuffix: _configuration.RequestSufix,
                        logger:logger) as IHttpMethod<TEntity>;
                default:
                    return null;
            }
        }

        private HttpClient CreateHttpClient(IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(GetRequestName());
        }

        private TService GetService<TService>(IServiceProvider serviceProvider) where TService : class
        {
            return serviceProvider.GetService(typeof(TService)) as TService;
        }


        private IServiceCollection ConfigureServices(Action<HttpClient> httpClient = null, IHttpRequestRetryHandler retryHandler = null)
        {
            var services = new ServiceCollection();
            Action<HttpClient> httpClientAction;

            if (httpClient == null)
                httpClientAction = c =>
                {
                    c.BaseAddress = new Uri(_configuration.BaseAddress);
                    foreach (var (key, value) in _configuration.RequestHeaders)
                        c.DefaultRequestHeaders.Add(key, value);
                };
            else
                httpClientAction = httpClient;
            ConfigurePolly(services, retryHandler);

            services.AddHttpClient(GetRequestName(), httpClientAction)
                .AddPolicyHandlerFromRegistry(_configuration.Polly.Name);

            return services;
        }

        private void ConfigurePolly(IServiceCollection service, IHttpRequestRetryHandler retryHandler = null)
        {
            if (_configuration.Polly == null) return;


            AsyncRetryPolicy<HttpResponseMessage> policy;



            if (retryHandler == null)
            {
                policy = Policy
                   .Handle<Exception>()
                   .OrResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                   .WaitAndRetryAsync(
                       retryCount: _configuration.Polly.Retries,
                       sleepDurationProvider: retryAttempt =>
                           TimeSpan.FromSeconds(_configuration.Polly.IntervalInSeconds));

            }
            else
            {
                policy = Policy
                    .Handle<Exception>()
                    .OrResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                    .WaitAndRetryAsync(
                        retryCount: _configuration.Polly.Retries,
                        sleepDurationProvider: retryAttempt =>
                            TimeSpan.FromSeconds(_configuration.Polly.IntervalInSeconds),
                        onRetry: retryHandler.OnRetry);
            }

            service.AddPolicyRegistry().Add(_configuration.Polly.Name, policy);

        }


        private string GetRequestName()
        {
            return string.IsNullOrEmpty(_configuration.RequestName)
                ? $"DEFAULT_REQUEST"
                : _configuration.RequestName;
        }
    }
}
