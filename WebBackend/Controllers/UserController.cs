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
        public async Task<Response> SaveUser([FromBody] UserReqDto req)
        {
            try
            {
                User? res = null;
                if(string.IsNullOrEmpty(req.userName)) throw new Exception("用户名不能为空");
                if (req.id != null)
                {
                    var User = (await blluser.GetUsersAsync(new UserReqDto(){ userName=req.userName, idNot=req.id})).FirstOrDefault();
                    if(User!=null) throw new Exception("用户名已存在");
                    res = await blluser.GetUserByIdAsync(Convert.ToInt32(req.id));
                    if (res == null) throw new Exception("编码对应实体不存在");
                    res.UserName=req.userName??"";
                    res.RealName=req.realName??"";
                    res.PassWord=req.passWord??"";
                    res.UserEmail=req.userEmail??"";
                    res.UserPost=req.userPost??"";
                    await blluser.UpdateUserAsync(res);
                }
                else
                {
                    var User = (await blluser.GetUsersAsync(new UserReqDto(){ userName=req.userName})).FirstOrDefault();
                    if(User!=null) throw new Exception("用户名已存在");
                    res = new User()
                    {
                        CreateTime=DateTime.Now,
                        CreateUserId=1,
                        IfDel=0,
                        RealName=req.realName??"",
                        PassWord=req.passWord??"",
                        UserName=req.userName??"",
                        UserEmail=req.userEmail??"",
                        UserPost=req.userPost
                    };
                    await blluser.AddUserAsync(res);
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
                if (res == null) throw new Exception("编码对应日志不存在");
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
        public async Task<Response<UserResDto>> GetUserById([FromBody] UserReqDto req)
        {
            try
            {
                var res = await blluser.GetUserByIdAsync(Convert.ToInt32(req.id));
                if (res == null) throw new Exception("编码对应实体不存在");
                return new Response<UserResDto>
                {
                    IfSuccess = 1,
                    Data = new UserResDto(){
                        Id=res.Id, 
                        RealName=res.RealName,
                        UserName=res.UserName,
                        UserEmail=res.UserEmail,
                        UserHead=res.UserHead, 
                        UserPost=res.UserPost
                    },
                };
            }
            catch (Exception ex)
            {
                return new Response<UserResDto>()
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
    
        [HttpPost]
        public ActionResult FileUpload()
        {
            try
            {
                // 1. 检查是否有文件上传
                if (Request.Form.Files.Count == 0)
                {
                    return Json(new { success = false, message = "未接收到文件" });
                }

                // 2. 遍历所有上传的文件
                var uploadedFiles = Request.Form.Files;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "User"); // 存储路径

                // 3. 如果目录不存在则创建
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                // 4. 保存每个文件
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    var file = uploadedFiles[i];
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(savePath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                // 5. 返回成功信息
                return Json(new { success = true, message = "上传成功" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "上传失败: " + ex.Message });
            }
        }
    }
}
