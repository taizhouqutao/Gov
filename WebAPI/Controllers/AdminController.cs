using DAL.Modles;
using Microsoft.AspNetCore.Mvc;
using Common;
namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
      private BLL.BllRole bll=new BLL.BllRole();
      private BLL.BllUser blluser=new BLL.BllUser();
      
      [HttpPost]
      [Route("GetRolesByPage",Name = "GetRolesByPage")]
      public async Task<Response<PageList<Role>>> GetRolesByPageAsync(PageReq<RoleReqDto> req)
      {
        try{
          var res = await bll.GetRolesByPageAsync(req);
          return new Response<PageList<Role>>{  
            IfSuccess=1,
            Data=res,
          };
        }
        catch(Exception ex)
        {
          return new Response<PageList<Role>>(){
            Msg=ex.Message
          };
        }
      }

      [HttpPost]
      [Route("GetUsersByPage",Name = "GetUsersByPage")]
      public async Task<Response<PageList<User>>> GetUsersByPageAsync(PageReq<UserReqDto> req)
      {
        try{
          var res = await blluser.GetUsersByPageAsync(req);
          return new Response<PageList<User>>{  
            IfSuccess=1,
            Data=res,
          };
        }
        catch(Exception ex)
        {
          return new Response<PageList<User>>(){
            Msg=ex.Message
          };
        }
      }
    }
}