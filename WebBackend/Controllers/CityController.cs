using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
  public class CityController : Controller
  {
    BllCity bllCity = new BllCity();

    [Authorize("001005")]
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public async Task<Response<PageList<City>>> GetCitysByPage([FromBody] PageReq<CityReqDto> req)
    {
      try
      {
        var res = await bllCity.GetCitysByPageAsync(req);
        return new Response<PageList<City>>
        {
          IfSuccess = 1,
          Data = res
        };
      }
      catch (Exception ex)
      {
        return new Response<PageList<City>>()
        {
          Msg = ex.Message
        };
      }
    }

    [HttpPost]
    public async Task<Response<City>> GetCityById([FromBody] CityReqDto req)
    {
      var res = await bllCity.GetCityByIdAsync(req.Id ?? 0);
      return new Response<City> { IfSuccess = 1, Data = res };
    }


    [Authorize("001005001")]
    [HttpPost]
    public async Task<Response<City>> SaveCity([FromBody] CityReqDto req)
    {
      try
      {
        var UserId = HttpContext.Session.GetInt32("UserId");
        City? res = null;
        if (req.Id != null)
        {
          res = await bllCity.GetCityByIdAsync(Convert.ToInt32(req.Id));
          if (res == null) throw new Exception("编码对应实体不存在");
          res.CityName = req.CityName ?? "";
          res.CitySort = Convert.ToInt32(req.CitySort);
          res.UpdateUserId = UserId ?? 0;
          res.UpdateTime = DateTime.Now;
          await bllCity.UpdateCityAsync(res);
        }
        else
        {
          res = new City()
          {
            IfDel = 0,
            CreateTime = DateTime.Now,
            CreateUserId = UserId ?? 0,
            CityName = req.CityName ?? "",
            CitySort = Convert.ToInt32(req.CitySort)
          };
          await bllCity.AddCityAsync(res);
        }
        return new Response<City>
        {
          IfSuccess = 1,
          Data = res,
        };
      }
      catch (Exception ex)
      {
        return new Response<City>()
        {
          Msg = ex.Message
        };
      }
    }

    [Authorize("001005003")]
    [HttpPost]
    public async Task<Response> DelCity([FromBody] CityReqDto req)
    {
      try
      {
        var UserId = HttpContext.Session.GetInt32("UserId");
        if (req.Ids != null && req.Ids.Count > 0)
        {
          await bllCity.DelCityAsync(req.Ids, UserId ?? 0);
        }
        return new Response
        {
          IfSuccess = 1,
        };
      }
      catch (Exception ex)
      {
        return new Response()
        {
          Msg = ex.Message
        };
      }
    }
  }
}