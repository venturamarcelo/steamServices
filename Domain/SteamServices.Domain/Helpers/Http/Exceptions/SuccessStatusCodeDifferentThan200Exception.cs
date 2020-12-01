using System;

namespace SteamServices.Domain.Helpers.Http.Exceptions
{
    public class SuccessStatusCodeDifferentThan200Exception : ApiResponseException
    {
        public SuccessStatusCodeDifferentThan200Exception() : base() { }
        public SuccessStatusCodeDifferentThan200Exception(string message) : base(message) { }
        public SuccessStatusCodeDifferentThan200Exception(string message, Exception exception) : base(message, exception) { }
    }
}
