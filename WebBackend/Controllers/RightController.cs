using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<Response<List<RightTreeDto>>> GetRightsByTree([FromBody] PageReq<RightReqDto> req)
        {
            try
            {
                var res = await bllRight.GetRightTreeAsync(req.Query);
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


    }
}