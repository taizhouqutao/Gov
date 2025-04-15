using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class NewController : Controller
    {
        private readonly ILogger<NewController> _logger;
        public NewController(ILogger<NewController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
