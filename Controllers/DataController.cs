using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using RestApiForGeoData.Models;

namespace RestApiForGeoData.Controllers
{
    public class DataController : Controller
    {
        [HttpPost]
        public IActionResult Index(string country, string city, string street)
        {

            LogWriter logWriter;

            string log;

            dynamic dataDadata;

            dynamic dataOpenstreetmap = Openstreetmap(country, city, street);


            if (dataOpenstreetmap.Count > 0)
            {
                try
                {
                    dataDadata = Dadata(dataOpenstreetmap[0].lat, dataOpenstreetmap[0].lon);

                    ViewBag.lon = dataOpenstreetmap[0].lon;

                    ViewBag.lat = dataOpenstreetmap[0].lat;

                    logWriter = new LogWriter($"Успешный поиск по адресу: '{country}' '{city}' '{street}'");

                    return View(dataDadata);
                }
                catch (Exception ex)
                {
                    logWriter = new LogWriter($"Ошибка '{ex.Message}' при поиске по адресу: '{country}' '{city}' '{street}'");

                    return BadRequest(ex);
                }
            }
            else
            {
                string message = "Нет результатов";
                logWriter = new LogWriter($"Ошибка '{message}' при поиске по адресу: '{country}' '{city}' '{street}'");
                return BadRequest(message);
            }
            
        }


        public dynamic Dadata<T>(T lat, T lon)
        {
            string url = $"https://suggestions.dadata.ru/suggestions/api/4_1/rs/geolocate/address";
            
            RestRequest request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Token 48ecca72935291047a107f7becf0e2036ba98742");

            request.AddBody(JsonConvert.SerializeObject(new { lat = lat, lon = lon, count = 10 }));
            
            dynamic data = RestClient(url, request);

            return data;
        }


        public dynamic Openstreetmap(string country, string city, string street)
        {
            string url = $"https://nominatim.openstreetmap.org/search?country={country}&city={city}&street={street}&format=json&limit=2";
            
            RestRequest request = new RestRequest(url, Method.Get);

            dynamic data = RestClient(url, request);

            return data;


        }


        public dynamic RestClient(string url, RestRequest request)
        {
            RestClient client = new RestClient(url);

            var response = client.Execute(request);

            string stream = response.Content;

            var data = JsonConvert.DeserializeObject(stream);

            return data;
        }
    }
}
