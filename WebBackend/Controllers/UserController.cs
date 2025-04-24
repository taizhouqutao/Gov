using System.Text.Json;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
    public class UserController : Controller
    {
        private BLL.BllRole bllrole = new BLL.BllRole();
        private BLL.BllUser blluser = new BLL.BllUser();
        private BLL.BllBizLog bllbizlog = new BLL.BllBizLog();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult Role()
        {
            return View();
        }
        public IActionResult BizLog()
        {
            return View();
        }

        [HttpPost]
        public async Task<Response<PageList<BizLog>>> GetBizLogsByPage([FromBody] PageReq<BizLogReqDto> req)
        {
            try
            {
                var res = await bllbizlog.GetBizLogsByPageAsync(req);
                return new Response<PageList<BizLog>>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<BizLog>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<BizLog>> GetBizLogById([FromBody] BizLogReqDto req)
        {
            try
            {
                var res = await bllbizlog.GetBizLogByIdAsync(Convert.ToInt32(req.id));
                if (res == null) throw new Exception("编码对应角色不存在");
                return new Response<BizLog>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<BizLog>()
                {
                    Msg = ex.Message
                };
            }
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

        [HttpPost]
        public async Task<Response<PageList<Role>>> GetRolesByPage([FromBody] PageReq<RoleReqDto> req)
        {
            try
            {
                var res = await bllrole.GetRolesByPageAsync(req);
                return new Response<PageList<Role>>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<Role>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<Role>> GetRolesById([FromBody] RoleReqDto req)
        {
            try
            {
                var res = await bllrole.GetRoleByIdAsync(Convert.ToInt32(req.id));
                if (res == null) throw new Exception("编码对应角色不存在");
                return new Response<Role>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<Role>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<Role>> SaveRole([FromBody] RoleReqDto req)
        {
            try
            {
                Role res = null;
                if (req.id != null)
                {
                    res = await bllrole.GetRoleByIdAsync(Convert.ToInt32(req.id));
                    if (res == null) throw new Exception("编码对应角色不存在");
                    res.RoleName = req.roleName ?? "";
                    await bllrole.UpdateRoleAsync(res);
                }
                else
                {
                    res = new Role()
                    {
                        RoleName = req.roleName ?? "",
                        IfDel = 0,
                        CreateTime = DateTime.Now,
                        CreateUserId = 1
                    };
                    await bllrole.AddRoleAsync(res);
                }
                return new Response<Role>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<Role>()
                {
                    Msg = ex.Message
                };
            }
        }
    
        [HttpPost]
        public async Task<Response> DelRole([FromBody] RoleReqDto req)
        {
            try
            {
                if (req.ids != null && req.ids.Count>0)
                {
                    await bllrole.DelRoleAsync(req.ids);
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
