using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBackend.Models;
using Common;
using Microsoft.AspNetCore.Authorization;

namespace WebBackend.Controllers;

public class HomeController : Controller
{
    private BLL.BllUser blluser = new BLL.BllUser();

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Home()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult login()
    {
        return View();
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

    [HttpPost]
    [AllowAnonymous]
    public async Task<Response<LoginResDto>> Login([FromBody] LoginReqDto req)
    {
        try
        {
            var res = await blluser.LoginAsync(req);
            if (res == null) throw new Exception("用户名和密码错误");
            return new Response<LoginResDto>
            {
                IfSuccess = 1,
                Data =new LoginResDto(){
                    RealName=res.RealName,
                    UserName=res.UserName
                }
            };
        }
        catch (Exception ex)
        {
            return new Response<LoginResDto>()
            {
                Msg = ex.Message
            };
        }
    }
}
