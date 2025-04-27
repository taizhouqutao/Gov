using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
    public class RightController: Controller
    {
        private BLL.BllRight bllRight = new BLL.BllRight();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<Response<PageList<RightTreeDto>>> GetRightsByTree([FromBody] PageReq<RightReqDto> req)
        {
            try
            {
                var res = await bllRight.GetRightTreeAsync(req.Query);
                return new Response<PageList<RightTreeDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<RightTreeDto>(){
                        data=res, 
                        draw=req.draw,
                        recordsFiltered=res.Count,
                        recordsTotal=res.Count
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<RightTreeDto>>()
                {
                    Msg = ex.Message
                };
            }
        }


    }
}