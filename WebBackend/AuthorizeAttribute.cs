using DAL.Modles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
  public string SessionKey { get; set; } = "UserId";
  public string RightKey { get; set; } = "RightId";
  private readonly string[] _permissions;

  public AuthorizeAttribute(params string[] permissions)
  {
    _permissions = permissions;
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var httpContext = context.HttpContext;
    if (httpContext.Session?.GetString(SessionKey) == null)
    {
      context.Result = new StatusCodeResult(401); // 未授权
      return;
    }

    // 如果需要，可以添加权限检查逻辑
    if (_permissions != null && !HasPermissions(_permissions, httpContext))
    {
      context.Result = new StatusCodeResult(403); // 禁止访问
      return;
    }
  }

  private bool HasPermissions(string[] permissions, HttpContext httpContext)
  {
    if (httpContext.Session?.GetString(RightKey) == null)
    {
      return false;
    }
    List<string> RightIds = new List<string>();
    string RightId = httpContext.Session.GetString(RightKey);
    if (!string.IsNullOrEmpty(RightId))
    {
      RightIds = JsonConvert.DeserializeObject<List<string>>(RightId);
    }
    return permissions.All(perm => RightIds.Exists(c => c == perm));
  }
}