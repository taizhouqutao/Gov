using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
    public class UserController : Controller
    {
        private BLL.BllRole bll = new BLL.BllRole();
        private BLL.BllUser blluser = new BLL.BllUser();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public async Task<Response<PageList<User>>> GetUsersByPage([FromBody] PageReq<UserReqDto> req)
        {
            try
            {
                var res = await blluser.GetUsersByPageAsync(req);
                return new Response<PageList<User>>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<User>>()
                {
                    Msg = ex.Message
                };
            }
        }
    }
}
