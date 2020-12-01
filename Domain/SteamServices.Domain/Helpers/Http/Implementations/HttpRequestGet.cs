using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using SteamServices.Domain.Helpers.Http.Abstractions;
using SteamServices.Domain.Helpers.Http.Interfaces;

namespace SteamServices.Domain.Helpers.Http.Implementations
{
    internal class HttpRequestGet : BaseHttpRequestMethod<string>,
        IHttpMethod<string>
    {
        public HttpRequestGet(HttpClient client, string addressSuffix, ILogger logger) : base(client, addressSuffix,logger)
        {
        }

        public HttpResponseMessage Execute(string parameters, string suffixComplement = null)
        {
            try
            {
                var result = Client.GetAsync($"{AddressSuffix}/{parameters}").Result;
                return result;
            }
            catch (Exception ex)
            {

                return CreateDefaultExceptionResponseMessage(
                    httpMethod: HttpMethod.Get,
                    exception: ex,
                    statusCode: HttpStatusCode.BadGateway);
            }
        }
    }
}
