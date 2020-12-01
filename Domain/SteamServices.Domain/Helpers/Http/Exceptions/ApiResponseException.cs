using System;
using System.Net;

namespace SteamServices.Domain.Helpers.Http.Exceptions
{
    public class ApiResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiResponseException() : base() { }
        public ApiResponseException(string message) : base(message) { }
        public ApiResponseException(string message, Exception exception) : base(message, exception) { }
    }
}
