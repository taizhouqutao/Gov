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

        public async Task<IActionResult> AdminEditPwd()
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            var user = await blluser.GetUserByIdAsync(Convert.ToInt32(UserId));
            var Res = new UserDto()
            {
                userHead = user.UserHead
            };
            return View(Res);
        }

        public IActionResult AdminLoginOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Admin()
        {
            var Roles = await bllrole.GetRolesByUser(new RoleReqDto() { userId = 0 });
            return View(new AdminDto()
            {
                Roles = Roles
            });
        }

        [Authorize("001001")]
        public IActionResult Role()
        {
            return View();
        }
        public IActionResult BizLog()
        {
            return View();
        }
        [HttpPost]
        public async Task<Response> SaveMyHead([FromBody] UserReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                var user = await blluser.GetUserByIdAsync(Convert.ToInt32(UserId));
                if (user == null) throw new Exception("用户名不存在");
                user.UserHead = string.IsNullOrEmpty(req.userHead) ? "/img/unperson.jpg" : req.userHead;
                user.UpdateTime = DateTime.Now;
                user.UpdateUserId = UserId ?? 0;
                await blluser.UpdateUserAsync(user);
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
        public async Task<Response> SaveUser([FromBody] UserReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                User? res = null;
                if (string.IsNullOrEmpty(req.userName)) throw new Exception("用户名不能为空");
                if (req.id != null)
                {
                    var User = (await blluser.GetUsersAsync(new UserReqDto() { userName = req.userName, idNot = req.id })).FirstOrDefault();
                    if (User != null) throw new Exception("用户名已存在");
                    res = await blluser.GetUserByIdAsync(Convert.ToInt32(req.id));
                    if (res == null) throw new Exception("编码对应实体不存在");
                    res.UserName = req.userName ?? "";
                    res.RealName = req.realName ?? "";
                    res.UserEmail = req.userEmail ?? "";
                    res.UserPost = req.userPost ?? "";
                    res.UserHead = string.IsNullOrEmpty(req.userHead) ? "/img/unperson.jpg" : req.userHead;
                    res.UpdateTime = DateTime.Now;
                    res.UpdateUserId = UserId ?? 0;
                    await blluser.UpdateUserAsync(res);
                }
                else
                {
                    var User = (await blluser.GetUsersAsync(new UserReqDto() { userName = req.userName })).FirstOrDefault();
                    if (User != null) throw new Exception("用户名已存在");
                    res = new User()
                    {
                        CreateTime = DateTime.Now,
                        CreateUserId = UserId ?? 0,
                        IfDel = 0,
                        RealName = req.realName ?? "",
                        PassWord = req.passWord ?? "",
                        UserName = req.userName ?? "",
                        UserEmail = req.userEmail ?? "",
                        UserPost = req.userPost,
                        UserHead = string.IsNullOrEmpty(req.userHead) ? "/img/unperson.jpg" : req.userHead,
                        Enable = 1
                    };
                    res = await blluser.AddUserAsync(res);
                }
                if (req.roles != null)
                {
                    await blluser.SaveUserRole(res, req.roles);
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
                var roles = await bllrole.GetRolesByUser(new RoleReqDto() { userId = req.id });
                return new Response<UserResDto>
                {
                    IfSuccess = 1,
                    Data = new UserResDto()
                    {
                        Id = res.Id,
                        RealName = res.RealName,
                        UserName = res.UserName,
                        UserEmail = res.UserEmail,
                        UserHead = res.UserHead,
                        UserPost = res.UserPost,
                        Enable = res.Enable,
                        CreateTime = res.CreateTime,
                        CreateUserId = res.CreateUserId,
                        UpdateTime = res.UpdateTime,
                        UpdateUserId = res.UpdateUserId,
                        roles = roles.Where(i => i.ifCheck == 1).ToList().ConvertAll(i => i.roleId)
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
        public async Task<Response<PageList<UserResDto>>> GetUsersByPage([FromBody] PageReq<UserReqDto> req)
        {
            try
            {
                var res = await blluser.GetUsersByPageAsync(req);
                return new Response<PageList<UserResDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<UserResDto>()
                    {
                        recordsFiltered = res.recordsFiltered,
                        recordsTotal = res.recordsTotal,
                        draw = res.draw,
                        data = res.data.ConvertAll(i => new UserResDto()
                        {
                            Id = i.Id,
                            RealName = i.RealName,
                            UserName = i.UserName,
                            UserEmail = i.UserEmail,
                            UserHead = i.UserHead,
                            UserPost = i.UserPost,
                            Enable = i.Enable,
                            CreateTime = i.CreateTime,
                            CreateUserId = i.CreateUserId,
                            UpdateTime = i.UpdateTime,
                            UpdateUserId = i.UpdateUserId
                        })
                    },
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<UserResDto>>()
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
                var UserId = HttpContext.Session.GetInt32("UserId");
                Role res = null;
                if (req.id != null)
                {
                    res = await bllrole.GetRoleByIdAsync(Convert.ToInt32(req.id));
                    if (res == null) throw new Exception("编码对应角色不存在");
                    res.RoleName = req.roleName ?? "";
                    res.UpdateTime = DateTime.Now;
                    res.UpdateUserId = UserId ?? 0;
                    await bllrole.UpdateRoleAsync(res);
                }
                else
                {
                    res = new Role()
                    {
                        RoleName = req.roleName ?? "",
                        IfDel = 0,
                        CreateTime = DateTime.Now,
                        CreateUserId = UserId ?? 0
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
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await bllrole.DelRoleAsync(req.ids, UserId ?? 0);
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
        public async Task<Response> DelUser([FromBody] RoleReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await blluser.DelUserAsync(req.ids, UserId ?? 0);
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
        public async Task<Response> SetEnableUser([FromBody] UserReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    var Users = await blluser.GetUsersByIdAsync(req.ids);
                    Users.ForEach(i =>
                    {
                        i.Enable = (int)req.enable;
                        i.UpdateTime = DateTime.Now;
                        i.UpdateUserId = UserId ?? 0;
                    });
                    await blluser.UpdateUsersAsync(Users);
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
                var ResPath = "/Uploads/User/";
                string fileName = string.Empty;
                // 4. 保存每个文件
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    var file = uploadedFiles[i];
                    if (file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        var fileExt = fileName.Split('.').LastOrDefault();
                        // 生成随机数作为文件名
                        var NewFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        var random = new Random();
                        var randomNum = random.Next(1000, 9999);
                        fileName = $"{NewFileName}{randomNum}.{fileExt}";

                        var filePath = Path.Combine(savePath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        break;
                    }
                }

                // 5. 返回成功信息
                return Json(new { success = true, message = "上传成功", file = $"{ResPath}{fileName}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "上传失败: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<Response> ReSetPwd([FromBody] UserReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (!string.IsNullOrEmpty(req.passWord))
                {
                    var user = await blluser.GetUserByIdAsync(Convert.ToInt32(UserId));
                    if (user != null)
                    {
                        user.PassWord = req.passWord;
                        user.UpdateTime = DateTime.Now;
                        user.UpdateUserId = UserId ?? 0;
                        await blluser.UpdateUserAsync(user);
                    }
                    else
                    {
                        throw new Exception("当前用户不存在");
                    }
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
        public async Task<Response<UserResDto>> GetMyInfo()
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                var res = await blluser.GetUserByIdAsync(Convert.ToInt32(UserId));
                if (res == null) throw new Exception("编码对应实体不存在");
                return new Response<UserResDto>
                {
                    IfSuccess = 1,
                    Data = new UserResDto()
                    {
                        Id = res.Id,
                        RealName = res.RealName,
                        UserName = res.UserName,
                        UserEmail = res.UserEmail,
                        UserHead = res.UserHead,
                        UserPost = res.UserPost,
                        Enable = res.Enable,
                        CreateTime = res.CreateTime,
                        CreateUserId = res.CreateUserId,
                        UpdateTime = res.UpdateTime,
                        UpdateUserId = res.UpdateUserId
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
    }
}
