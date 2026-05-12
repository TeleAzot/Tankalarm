using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Tankalarm.Data.API.DTO;
using Tankalarm.Data.API.Models;
using Tankalarm.Data.DB.Models;

namespace Tankalarm.Data.API.Services
{
    /// <summary>
    /// Provides methods to receive fuel prices in a certain area via the Tankerkoenig API.
    /// </summary>
    public class TankerkoenigService
    {
        readonly HttpClient _client;

        public TankerkoenigService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.TankerkoenigAPIBaseAddress);
        }

        public async Task<IEnumerable<FuelStation>> GetCheapestFuelPricesAsync(double lon, double lat, int radius, string fuelType)
        {
            var responseMessage = await _client.GetAsync($"list.php?lat={lat}&lng={lon}&rad={radius}&type={fuelType.ToLower()}&sort=price&apikey={Secrets.TankerkoenigAPIKey}");
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(await responseMessage.Content.ReadAsStringAsync());

            var jsonResponse = JsonSerializer.Deserialize<TankerkoenigResponse>(await responseMessage.Content.ReadAsStringAsync());

            if (!jsonResponse.ok)
                throw new Exception("Es ist ein unerwarteter Fehler beim Abrufen der günstigsten Spritpreise im Umkreis aufgetreten.");

            List<FuelStation> stations = new List<FuelStation>();
            foreach (var s in jsonResponse.stations)
            {
                stations.Add(new FuelStation
                {
                    Name = s.name,
                    Brand = s.brand,
                    Street = s.street,
                    City = s.place,
                    PostCode = s.postCode,
                    Distance = s.dist,
                    Price = s.price,
                    IsOpen = s.isOpen
                });
            }
            return stations;
        }
    }
}