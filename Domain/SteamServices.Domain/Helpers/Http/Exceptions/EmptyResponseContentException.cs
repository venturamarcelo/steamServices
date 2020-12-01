using System;

namespace SteamServices.Domain.Helpers.Http.Exceptions
{
    public class EmptyResponseContentException : ApiResponseException
    {
        public EmptyResponseContentException() : base() { }
        public EmptyResponseContentException(string message) : base(message) { }
        public EmptyResponseContentException(string message, Exception exception) : base(message, exception) { }
    }
}
