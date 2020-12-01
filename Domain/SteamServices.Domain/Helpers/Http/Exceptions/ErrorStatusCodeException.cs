using System;

namespace SteamServices.Domain.Helpers.Http.Exceptions
{
    public class ErrorStatusCodeException : ApiResponseException
    {
        public ErrorStatusCodeException() : base() { }
        public ErrorStatusCodeException(string message) : base(message) { }
        public ErrorStatusCodeException(string message, Exception exception) : base(message, exception) { }
    }
}
