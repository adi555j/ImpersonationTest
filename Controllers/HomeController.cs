using ImpersonationTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImpersonationTest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRestAdapter _restAdapter;

        public HomeController(ILogger<HomeController> logger, IRestAdapter restAdapter)
        {
            _logger = logger;
            _restAdapter = restAdapter;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetSecureData()
        {
            List<WeatherForecast> meetingList = new List<WeatherForecast>();
            var requestUrl = "http://localhost:10707/weatherforecast";
            var response = await _restAdapter.GetJsonAsyncUsingAuthentication(requestUrl);
            meetingList = JsonConvert.DeserializeObject<List<WeatherForecast>>(response);// https://localhost:44301/Home/GetSecureData
            ViewData["t"] = meetingList;
            return View(true);
        }      
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
