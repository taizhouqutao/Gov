using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebBackend.Controllers
{
    public class RightController : Controller
    {
        private BLL.BllRight bllRight = new BLL.BllRight();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<Response<List<RightTreeDto>>> GetRightsByTree([FromBody] RightReqDto req)
        {
            try
            {
                var res = await bllRight.GetRightTreeAsync(req);
                return new Response<List<RightTreeDto>>
                {
                    IfSuccess = 1,
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new Response<List<RightTreeDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response> SaveRoleRights([FromBody] RightReqDto req)
        {
            try
            {
                req.UserId = HttpContext.Session.GetInt32("UserId");
                await bllRight.SaveRoleRights(req);
                return new Response
                {
                    IfSuccess = 1
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