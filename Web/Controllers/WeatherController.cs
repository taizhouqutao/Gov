using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class WeatherController : Controller
    {
        private readonly ILogger<WeatherController> _logger;
        private BllWeather bllWeather = new BllWeather();
        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<Response<Weather>> GetTodayWeather()
        {
            try
            {
                var res = await bllWeather.GetTodayWeatherAsync();
                if (res == null) throw new Exception("编码对应实体不存在");
                return new Response<Weather>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<Weather>()
                {
                    Msg = ex.Message
                };
            }
        }
    }
}