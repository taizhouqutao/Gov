using System.Diagnostics;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Common;
namespace Web.Controllers;

public class HomeController : Controller
{
    BllWeather bll=new BllWeather();
    private BLL.BllNew bllNew = new BLL.BllNew();
    private BLL.BllNewType bllNewType = new BLL.BllNewType();
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var NewType = await bllNewType.GetNewTypeByIdAsync(12);
        var res = await bllNew.GetNewsByPageAsync(new PageReq<NewReqDto>(){
            start=0,
            length=18,
            Query=new NewReqDto(){
                isPublic=1,
                newTypeId=12
            }
        });
        var homePageDto=new HomePageDto()
        {
            NewTypeName=NewType.NewTypeName.Replace("管理",""),
            News=res.data.ConvertAll(i=>new HomeNewItemDto(){
                Id=i.Id,
                NewContent=i.NewContent,
                NewTitle=i.NewTitle,
                PublicTime=i.PublicTime
            })
        };
        return View(homePageDto);
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
