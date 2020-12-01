using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using SteamServices.Domain.Helpers.Http.Abstractions;
using SteamServices.Domain.Helpers.Http.Interfaces;

namespace SteamServices.Domain.Helpers.Http.Implementations
{
    internal class HttpRequestPost<TEntity> :
        BaseHttpRequestMethod<TEntity>,
        IHttpMethod<TEntity>
    {

        public HttpRequestPost(HttpClient client, string addressSuffix, ILogger logger) : base(client, addressSuffix,logger)
        {
        }
        public HttpResponseMessage Execute(TEntity parameters, string suffixComplement = null)
        {
            try
            {
                var result = Client.PostAsync($"{AddressSuffix}{suffixComplement}", CreateJsonObjectContent(parameters)).Result;
                return result;
            }
            catch (Exception ex)
            {

                return CreateDefaultExceptionResponseMessage(
                    httpMethod: HttpMethod.Post,
                    exception: ex,
                    statusCode: HttpStatusCode.BadRequest);
            }
        }




    }
}
