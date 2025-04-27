using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class WeatherController : Controller
    {
        private readonly ILogger<WeatherController> _logger;
        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}