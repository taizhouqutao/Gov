using System.Diagnostics;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Web.Models;
using Common;
using Web.Lib;
namespace Web.Controllers;

public class HomeController : Controller
{
    public IConfiguration configuration;
    BllWeather bll = new BllWeather();
    private BLL.BllNew bllNew = new BLL.BllNew();
    private BLL.BllNewType bllNewType = new BLL.BllNewType();

    private BLL.BllCity bllCity = new BLL.BllCity();

    private readonly ILogger<HomeController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    BllCarousel bllCarousel = new BllCarousel();
    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }

    public async Task<IActionResult> Index()
    {
        var NewType = await bllNewType.GetNewTypeByIdAsync(12);
        string Url = configuration["BackEndPoint:Url"];
        var CityId = WebHelp.GetCityId(_httpContextAccessor);
        var res = await bllNew.GetNewsByPageAsync(new PageReq<NewReqDto>()
        {
            start = 0,
            length = 18,
            Query = new NewReqDto()
            {
                isPublic = 1,
                newTypeId = 12,
                cityIds=new List<int>() { CityId },
            }
        });
        var Carousels = await bllCarousel.GetCarouselsByPageAsync(new PageReq<CarouselReqDto>()
        {
            start = 0,
            length = int.MaxValue,
            Query = new CarouselReqDto()
            {
                isPublic = 1,
                cityIds=new List<int>() { CityId },
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

    public async Task<IActionResult> SetCity()
    {
        var Citys = await bllCity.GetCitysAsync(new CityReqDto() { });
        return View(new SetCityPageDto()
        {
            Citys = Citys.ConvertAll(i => new CityResDto
            {
                cityid = i.Id,
                cityName = i.CityName,
                ifCheck = 0
            })
        });
    }


    [HttpPost]
    public async Task<Response> SetCityId([FromBody] SetCityDto req)
    {
        // 将cityId写入cookies，永不过期
        Response.Cookies.Append("cityId", req.cityId.ToString(), new CookieOptions
        {
            Expires = DateTimeOffset.MaxValue
        });
        return new Response() { IfSuccess = 1, Msg = "设置成功" };
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
