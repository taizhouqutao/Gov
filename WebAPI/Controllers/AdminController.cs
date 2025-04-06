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
      [HttpPost]
      [Route("GetRolesByPage",Name = "GetRolesByPage")]
      public async Task<Response<PageList<Role>>> GetRolesByPageAsync(PageReq<RoleDto.Req> req)
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
    }
}