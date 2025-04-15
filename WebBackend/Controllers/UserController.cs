using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }
    }
}
