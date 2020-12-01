using System;
using SteamServices.Domain.Services;

namespace SteamServices.Application.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameId = 730;
            var type = "profiles";
            var typeId = 2;
            var steamId = "76561197989013350";
            var inventoryServices = new Inventory();
            var items = inventoryServices.GetItems(type, gameId, typeId, steamId);
        }
    }
}
