using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBackend.Models;
using Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebBackend.Controllers;
public class CheckSessionAttribute : ActionFilterAttribute
{
    // 可配置的Session键名，默认为"UserId"
    public string SessionKey { get; set; } = "UserId";
    
    // 可配置的登录页面路径，默认为"/Account/Login"
    public string LoginPath { get; set; } = "/Home/login";

    private readonly string[] _excludePaths = 
    { 
        "/Home/login"
    };

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var httpContext = filterContext.HttpContext;
        var request = httpContext.Request;
        var path = request.Path.Value?.ToLower();
        //return; // 直接放行
        if (_excludePaths.Any(p => path?.StartsWith(p.ToLower()) ?? false))
        {
            return; // 直接放行
        }
        // 检查Session是否存在且包含指定键
        if (httpContext.Session?.GetString(SessionKey) == null)
        {
            // 处理Ajax请求
            if (httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult(new 
                {
                    ifSuccess = false,
                    msg = "会话已过期，请重新登录。"
                });
                httpContext.Response.StatusCode = 401; // Unauthorized
            }
            else
            {
                if(path=="/home"||path=="/"||path=="/home/index")
                {
                    // 普通请求跳转登录页
                    filterContext.Result = new RedirectResult(LoginPath);
                }
                else
                {
                    httpContext.Response.StatusCode = 401; // Unauthorized
                }
            }
        }

        base.OnActionExecuting(filterContext);
    }
}