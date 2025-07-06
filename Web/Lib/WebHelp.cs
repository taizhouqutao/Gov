using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Common;
using BLL;
using DAL.Modles;
namespace Web.Lib
{
  public static class WebHelp
  {

    public static int GetCityId(IHttpContextAccessor httpContextAccessor)
    {
      // 从当前HTTP请求的Cookies中获取cityId的值
      var context = httpContextAccessor?.HttpContext;
      if (context != null && context.Request != null && context.Request.Cookies["cityId"] != null)
      {
        int cityId;
        if (int.TryParse(context.Request.Cookies["cityId"], out cityId))
        {
          return cityId;
        }
      }
      return 0;
    }

  }
}
