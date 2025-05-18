using System.Diagnostics;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Common;
namespace Web.Controllers;

public class HomeController : Controller
{
    public IConfiguration configuration;
    BllWeather bll = new BllWeather();
    private BLL.BllNew bllNew = new BLL.BllNew();
    private BLL.BllNewType bllNewType = new BLL.BllNewType();
    private readonly ILogger<HomeController> _logger;
    BllCarousel bllCarousel = new BllCarousel();
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }

    public async Task<IActionResult> Index()
    {
        var NewType = await bllNewType.GetNewTypeByIdAsync(12);
        string Url = configuration["BackEndPoint:Url"];
        var res = await bllNew.GetNewsByPageAsync(new PageReq<NewReqDto>()
        {
            start = 0,
            length = 18,
            Query = new NewReqDto()
            {
                isPublic = 1,
                newTypeId = 12
            }
        });
        var Carousels = await bllCarousel.GetCarouselsByPageAsync(new PageReq<CarouselReqDto>()
        {
            start = 0,
            length = int.MaxValue,
            Query = new CarouselReqDto()
            {
                isPublic = 1
            }
        });
        var homePageDto = new HomePageDto()
        {
            NewTypeName = NewType.NewTypeName.Replace("管理", ""),
            News = res.data.ConvertAll(i => new HomeNewItemDto()
            {
                Id = i.Id,
                NewContent = i.NewContent,
                NewTitle = i.NewTitle,
                PublicTime = i.PublicTime
            }),
            Carousels = Carousels.data.ConvertAll(i => new CarouselDto()
            {
                ImageUrl = $"{Url}{i.ImageUrl}",
                LinkUrl = String.IsNullOrEmpty(i.LinkUrl) ? "#" : i.LinkUrl,
                Title = i.Title
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
