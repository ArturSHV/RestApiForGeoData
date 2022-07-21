using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestApiForGeoData.Models;
using RestSharp;
using System.Diagnostics;

namespace RestApiForGeoData.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}