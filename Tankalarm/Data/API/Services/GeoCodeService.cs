using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Tankalarm.Data.API.DTO;
using Tankalarm.Data.API.Models;

namespace Tankalarm.Data.API.Services
{
    /// <summary>
    /// Provides methods to determine the longitude and latitude for a certain location via the geocode API.
    /// </summary>
    public class GeoCodeService
    {
        readonly HttpClient _client;

        public GeoCodeService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.GeoCodeAPIBaseAddress);
        }

        public async Task<Coordinates> GetCoordinatesByPostcodeAsync(string postcode)
        {
            var responseMessage = await _client.GetAsync($"search?postcode={postcode}&limit=1&lang=de&format=json&apiKey={Secrets.GeocodeAPIKey}");
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(await responseMessage.Content.ReadAsStringAsync());

            var jsonResponse = JsonSerializer.Deserialize<GeoResponse>(await responseMessage.Content.ReadAsStringAsync());

            if (jsonResponse.results.Count == 0)
                throw new Exception("Zur angegebenen PLZ konnten keine geografischen Koordinaten ermittelt werden, daher kann die Umkreissuche nicht getätigt werden.");

            return new Coordinates
            {
                Longitude = jsonResponse.results.First().lon,
                Latitude = jsonResponse.results.First().lat
            };
        }
    }
}