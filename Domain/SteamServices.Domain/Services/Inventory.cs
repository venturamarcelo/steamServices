
using System;
using System.Collections.Generic;
using SteamServices.Domain.Helpers.Http;
using SteamServices.Domain.Helpers.Http.Configurations;
using SteamServices.Domain.Helpers.Http.Extensions;
using SteamServices.Domain.Models;

namespace SteamServices.Domain.Services
{
    public class Inventory
    {
        private readonly string _steamCommunityBaseUrl = "https://steamcommunity.com/";
        private readonly string _steamCommunityApiTemplate= "{0}/{1}/inventory/json/{2}/{3}/";
        public IReadOnlyList<dynamic> GetItems(string type, int gameId, int typeId, string steamProfile)
        {
            var formatedTemplate = string.Format(_steamCommunityApiTemplate, type, steamProfile, gameId, typeId);
            var request = RequestBuilder.Build<string>(new RequestConfiguration
            {
                BaseAddress = _steamCommunityBaseUrl,
                HttpMethod = "GET",
                RequestName = "GetInventoryItems",
                TimeoutSeconds = 30,
                Polly = new PollyConfiguration
                {
                    IntervalInSeconds = 30,
                    Name = "GetInventoryItemsPolly",
                    Retries = 3
                }
            });
            var result = request.Execute(formatedTemplate);
            var value = result.Content.ReadAsStringAsync().Result;
            var a = result.ToEntity<InventoryModel>();
            return null;
        }
    }
}
