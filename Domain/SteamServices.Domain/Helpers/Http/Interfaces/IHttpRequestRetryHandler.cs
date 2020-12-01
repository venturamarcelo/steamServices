using System;
using System.Net.Http;
using Polly;

namespace SteamServices.Domain.Helpers.Http.Interfaces
{
    public interface IHttpRequestRetryHandler
    {

        void OnRetry(DelegateResult<HttpResponseMessage> result,
           TimeSpan interval,
           int retryCount,
           Context context);
    }
}
