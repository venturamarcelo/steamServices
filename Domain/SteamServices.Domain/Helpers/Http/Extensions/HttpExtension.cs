using System.Net.Http;
using System.Text.Json;
using SteamServices.Domain.Helpers.Http.Entities;

namespace SteamServices.Domain.Helpers.Http.Extensions
{
    public static class HttpExtension
    {
        public static TEntity ToEntity<TEntity>(this HttpResponseMessage response, TEntity defaultValue = default, JsonSerializerOptions jsonSerializerOptions = null) 
        {
            if (response == null) return defaultValue;
            return JsonSerializer.Deserialize<TEntity>(response.Content.ReadAsStringAsync().Result, jsonSerializerOptions?? new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public static ResponseViewModel<TEntity> ToResponseEntity<TEntity>(this HttpResponseMessage response,
            ResponseViewModel<TEntity> defaultValue = default, JsonSerializerOptions jsonSerializerOptions = null)
        {
            if (response == null) return defaultValue;
            return JsonSerializer.Deserialize<ResponseViewModel<TEntity>>(response.Content.ReadAsStringAsync().Result, jsonSerializerOptions?? new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
