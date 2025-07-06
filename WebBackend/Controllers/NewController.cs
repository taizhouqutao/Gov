using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebBackend.Controllers
{
    public class NewController : Controller
    {
        private BLL.BllCity bllcity = new BLL.BllCity();
        private BLL.BllNew bll = new BLL.BllNew();
        private BLL.BllComment bllcomment = new BLL.BllComment();
        private BLL.BllUser blluser = new BLL.BllUser();
        private BLL.BllNewType bllNewType = new BLL.BllNewType();

        public async Task<IActionResult> Index(int NewTypeId)
        {
            List<CityResDto> citys = new List<CityResDto>();
            if (HttpContext.Session.GetString("CityIds") != null)
            {
                var cityIds = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CityIds") ?? "[]");
                citys = await bllcity.GetCitysByIdAsync(cityIds);
            }
            var NewTypeRight = GetNewTypeRight(NewTypeId);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.ShowCode) && !CheckRightType(NewTypeRight.ShowCode))
            {
                return Redirect("/Home/Forbidden");
            }
            var NewType = await bllNewType.GetNewTypeByIdAsync(NewTypeId);
            var NewPage = new NewPage()
            {
                NewTypeId = NewType.Id,
                Title = NewType.NewTypeName,
                Citys = citys
            };
            return View(NewPage);
        }

        public async Task<IActionResult> Comment(int NewTypeId)
        {
            var NewTypeRight = GetNewTypeRight(NewTypeId);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.ShowComment) && !CheckRightType(NewTypeRight.ShowComment))
            {
                return Redirect("/Home/Forbidden");
            }
            NewPage? res = null;
            if (NewTypeId > 0)
            {
                var NewType = await bllNewType.GetNewTypeByIdAsync(NewTypeId);
                res = new NewPage()
                {
                    NewTypeId = NewType.Id,
                    Title = NewType.NewTypeName
                };
            }
            else
            {
                res = new NewPage()
                {
                    NewTypeId = 0,
                    Title = "意见收集"
                };
            }
            return View(res);
        }

        [HttpPost]
        public async Task<Response<PageList<CommentResDto>>> GetCommentsByPage([FromBody] PageReq<CommentReqDto> req)
        {
            try
            {
                if (HttpContext.Session.GetString("CityIds") != null)
                {
                    var cityIds = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CityIds") ?? "[]");
                    if (req.Query == null)
                    {
                        req.Query = new CommentReqDto();
                    }
                    req.Query.cityIds = cityIds;
                }
                var res = await bllcomment.GetCommentsByPageAsync(req);
                return new Response<PageList<CommentResDto>>
                {
                    IfSuccess = 1,
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<CommentResDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<PageList<NewListDto>>> GetNewsByPage([FromBody] PageReq<NewReqDto> req)
        {
            try
            {
                if (HttpContext.Session.GetString("CityIds") != null)
                {
                    var cityIds = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CityIds") ?? "[]");
                    if (req.Query == null)
                    {
                        req.Query = new NewReqDto();
                    }
                    req.Query.cityIds = cityIds;
                }
                var res = await bll.GetNewsByPageAsync(req);
                return new Response<PageList<NewListDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<NewListDto>()
                    {
                        recordsTotal = res.recordsTotal,
                        draw = res.draw,
                        recordsFiltered = res.recordsFiltered,
                        data = res.data.ConvertAll(j => new NewListDto()
                        {
                            CreateTime = j.CreateTime,
                            CreateUserId = j.CreateUserId,
                            Id = j.Id,
                            CityName = j.CityName,
                            IsPublic = j.IsPublic,
                            NewTitle = j.NewTitle,
                            PublicTime = j.PublicTime,
                            PublicUserId = j.PublicUserId,
                            UpdateTime = j.UpdateTime,
                            UpdateUserId = j.UpdateUserId
                        })
                    },
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<NewListDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<New>> GetNewById([FromBody] NewReqDto req)
        {
            try
            {
                var res = await bll.GetNewByIdAsync(Convert.ToInt32(req.id));
                if (res == null) throw new Exception("编码对应实体不存在");
                return new Response<New>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<New>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<New>> SaveNew([FromBody] NewReqDto req)
        {
            var NewTypeRight = GetNewTypeRight(req.newTypeId ?? 0);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.SaveCode) && !CheckRightType(NewTypeRight.SaveCode))
            {
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                return new Response<New>() { IfSuccess = 0 };
            }
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                New res = null;
                if (req.id != null)
                {
                    res = await bll.GetNewByIdAsync(Convert.ToInt32(req.id));
                    if (res == null) throw new Exception("编码对应实体不存在");
                    res.NewTitle = req.newTitle ?? "";
                    res.NewContent = req.newContent ?? "";
                    res.CityId = req.cityId;
                    res.UpdateTime = DateTime.Now;
                    res.UpdateUserId = UserId ?? 0;
                    await bll.UpdateNewAsync(res);
                }
                else
                {
                    res = new New()
                    {
                        NewTitle = req.newTitle ?? "",
                        IsPublic = 0,
                        NewContent = req.newContent ?? "",
                        IfDel = 0,
                        CreateTime = DateTime.Now,
                        CreateUserId = UserId ?? 0,
                        CityId = req.cityId,
                        NewTypeId = req.newTypeId,
                        CommentCount = 0,
                        ViewCount = 0
                    };
                    await bll.AddNewAsync(res);
                }
                return new Response<New>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<New>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response> DelNew([FromBody] NewReqDto req)
        {
            var NewTypeRight = GetNewTypeRight(req.newTypeId ?? 0);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.DelCode) && !CheckRightType(NewTypeRight.DelCode))
            {
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                return new Response() { IfSuccess = 0 };
            }
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await bll.DelNewAsync(req.ids, UserId ?? 0);
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
        public async Task<Response> SetNewPublic([FromBody] NewReqDto req)
        {
            var NewTypeRight = GetNewTypeRight(req.newTypeId ?? 0);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.OnDownCode) && !CheckRightType(NewTypeRight.OnDownCode))
            {
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                return new Response() { IfSuccess = 0 };
            }
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    var News = await bll.GetNewsByIdAsync(req.ids);
                    News.ForEach(i =>
                    {
                        i.IsPublic = (int)req.isPublic;
                        i.PublicTime = ((int)req.isPublic) == 1 ? DateTime.Now : null;
                        i.UpdateTime = DateTime.Now;
                        i.UpdateUserId = UserId ?? 0;
                    });
                    await bll.UpdateNewsAsync(News);
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
        public async Task<Response> DelComments([FromBody] CommentReqDto req)
        {
            var NewTypeRight = GetNewTypeRight(req.newTypeId ?? 0);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.DelComment) && !CheckRightType(NewTypeRight.DelComment))
            {
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                return new Response() { IfSuccess = 0 };
            }
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await bllcomment.DelCommentAsync(req.ids, UserId ?? 0);
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
        public async Task<Response> SetCommentShow([FromBody] CommentReqDto req)
        {
            var NewTypeRight = GetNewTypeRight(req.newTypeId ?? 0);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.OnDownComment) && !CheckRightType(NewTypeRight.OnDownComment))
            {
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                return new Response() { IfSuccess = 0 };
            }
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await bllcomment.SetCommentShowAsync(req, UserId ?? 0);
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
        public async Task<Response<CommentResDetailDto?>> GetCommentsById([FromBody] CommentReqDto req)
        {
            try
            {
                var res = await bllcomment.GetCommentDetailByIdAsync(Convert.ToInt32(req.id));
                if (res == null) throw new Exception("编码对应实体不存在");
                var SonComments = await bllcomment.GetCommentsByAsync(new CommentReqDto()
                {
                    fatherCommentId = res.Id
                });
                List<User> Users = new List<User>();
                if (SonComments.Count > 0)
                {
                    var UserIds = SonComments.Where(i => i.UserId != null && i.UserId > 0).Select(i => Convert.ToInt32(i.UserId)).ToList();
                    if (UserIds.Count > 0)
                    {
                        Users = await blluser.GetUsersByIdAsync(UserIds);
                    }
                }
                if (res.NewId > 0)
                {
                    var New = await bll.GetNewByIdAsync(Convert.ToInt32(res.NewId));
                    var NewContent = HtmlHelp.GetString(New.NewContent);
                    res.NewContent = NewContent.Length < 500 ? NewContent : NewContent.Substring(0, 500);
                }
                res.PersonHead = "/img/unperson.jpg";
                res.Deals = SonComments.ConvertAll(i =>
                {
                    var User = Users.FirstOrDefault(j => j.Id == (i.UserId == null ? 0 : Convert.ToInt32(i.UserId)));
                    return new CommentResDealDto()
                    {
                        Content = i.Content,
                        CreateTime = i.CreateTime,
                        CreateUserId = i.CreateUserId,
                        PersonName = i.PersonName,
                        RoleType = i.RoleType,
                        UserId = i.UserId,
                        PersonHead = (User == null || string.IsNullOrEmpty(User.UserHead)) ? "/img/unperson.jpg" : User.UserHead
                    };
                });
                return new Response<CommentResDetailDto?>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<CommentResDetailDto?>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<CommentResDealDto>> DealComment([FromBody] CommentReqDto req)
        {
            var NewTypeRight = GetNewTypeRight(req.newTypeId ?? 0);
            if (NewTypeRight != null && !string.IsNullOrEmpty(NewTypeRight.DalComment) && !CheckRightType(NewTypeRight.DalComment))
            {
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                return new Response<CommentResDealDto>() { IfSuccess = 0 };
            }
            try
            {
                var Comment = await bllcomment.GetCommentByIdAsync(Convert.ToInt32(req.id));
                var UserId = HttpContext.Session.GetInt32("UserId");
                var User = await blluser.GetUserByIdAsync(Convert.ToInt32(UserId));
                var NewComment = await bllcomment.AddCommentAsync(new DAL.Modles.Comment()
                {
                    NewId = Comment.NewId == null ? 0 : Convert.ToInt32(Comment.NewId),
                    Content = req.content ?? "",
                    CreateTime = DateTime.Now,
                    FatherCommentId = Comment.Id,
                    CreateUserId = User.Id,
                    IfDeal = 0,
                    IsShow = 1,
                    PersonCellphone = "",
                    PersonName = User.UserName ?? "",
                    IfDel = 0,
                    UserId = User.Id,
                    RoleType = 1
                });
                Comment.IfDeal = 1;
                Comment.IsShow = req.isShow ?? 0;
                Comment.UpdateTime = DateTime.Now;
                Comment.UpdateUserId = User.Id;
                await bllcomment.UpdateCommentAsync(Comment);
                return new Response<CommentResDealDto>
                {
                    IfSuccess = 1,
                    Data = new CommentResDealDto()
                    {
                        Content = NewComment.Content,
                        CreateTime = NewComment.CreateTime,
                        CreateUserId = NewComment.CreateUserId,
                        PersonName = User.UserName,
                        PersonHead = (User == null || string.IsNullOrEmpty(User.UserHead)) ? "/img/unperson.jpg" : User.UserHead,
                        RoleType = NewComment.RoleType,
                        UserId = User.Id,
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<CommentResDealDto>()
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
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "New"); // 存储路径

                // 3. 如果目录不存在则创建
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                var ResPath = "/Uploads/New/";
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
                return Json(new
                {
                    errno = false,
                    message = "上传成功",
                    data = new
                    {
                        url = $"{ResPath}{fileName}"
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { errno = true, message = "上传失败: " + ex.Message });
            }
        }

        public NewTypeRight? GetNewTypeRight(int NewTypeId)
        {
            List<NewTypeRight> rights = new List<NewTypeRight>();
            rights = new List<NewTypeRight>(){ new NewTypeRight()
            {
                NewTitye = "政策咨询管理",
                NewType = 1,
                ShowCode="003001",
                SaveCode="003001001",
                DelCode="003001003",
                OnDownCode="003001004",
                ShowComment="003002",
                DelComment="003002001",
                OnDownComment="003002002",
                DalComment="003002005"
            }, new NewTypeRight()
            {
                NewTitye = "公益讲座管理",
                NewType = 2,
                ShowCode="004001",
                SaveCode="004001001",
                DelCode="004001003",
                OnDownCode="004001004",
                ShowRep="004002"
            }, new NewTypeRight()
            {
                NewTitye = "法律援助管理",
                NewType = 3,
                ShowCode="005001",
                SaveCode="005001001",
                DelCode="005001003",
                OnDownCode="005001004",
                ShowComment="005002",
                DelComment="005002001",
                OnDownComment="005002002",
                DalComment="005002005"
            }, new NewTypeRight()
            {
                NewTitye = "禁毒宣传管理",
                NewType = 4,
                ShowCode="006001",
                SaveCode="006001001",
                DelCode="006001003",
                OnDownCode="006001004"
            }, new NewTypeRight()
            {
                NewTitye = "反诈宣传管理",
                NewType = 5,
                ShowCode="007001",
                SaveCode="007001001",
                DelCode="007001003",
                OnDownCode="007001004"
            }, new NewTypeRight()
            {
                NewTitye = "安全宣传管理",
                NewType = 6,
                ShowCode="008001",
                SaveCode="008001001",
                DelCode="008001003",
                OnDownCode="008001004"
            }, new NewTypeRight()
            {
                NewTitye = "政务办理导航管理",
                NewType = 7,
                ShowCode="010001",
                SaveCode="010001001",
                DelCode="010001003",
                OnDownCode="010001004",
                ShowRep="010002"
            }, new NewTypeRight()
            {
                NewTitye = "党建之窗管理",
                NewType = 8,
                ShowCode="011001",
                SaveCode="011001001",
                DelCode="011001003",
                OnDownCode="011001004"
            }, new NewTypeRight()
            {
                NewTitye = "廉政建设管理",
                NewType = 9,
                ShowCode="012001",
                SaveCode="012001001",
                DelCode="012001003",
                OnDownCode="012001004"
            }, new NewTypeRight()
            {
                NewTitye = "党务公开管理",
                NewType = 10,
                ShowCode="014001",
                SaveCode="014001001",
                DelCode="014001003",
                OnDownCode="014001004",
                ShowComment="014002",
                DelComment="014002001",
                OnDownComment="014002002"
            }, new NewTypeRight()
            {
                NewTitye = "党务要闻管理",
                NewType = 11,
                ShowCode="015001",
                SaveCode="015001001",
                DelCode="015001003",
                OnDownCode="015001004"
            }, new NewTypeRight()
            {
                NewTitye = "公告公示管理",
                NewType = 12,
                ShowCode="002001",
                SaveCode="002001001",
                DelCode="002001003",
                OnDownCode="002001004",
                ShowComment="002002",
                DelComment="002002003",
                OnDownComment="002002001"
            },new NewTypeRight()
            {
                NewTitye = "群众意见收集管理",
                NewType = 0,
                ShowComment="013001",
                DelComment="013001001",
                OnDownComment="013001002",
                DalComment="013001005"
            } };
            return rights.FirstOrDefault(i => i.NewType == NewTypeId);
        }

        public bool CheckRightType(string CkeckCode)
        {
            var httpContext = HttpContext;
            var rightIdStr = httpContext.Session?.GetString("RightId");
            List<string> rightIds = new List<string>();
            if (!string.IsNullOrEmpty(rightIdStr))
            {
                rightIds = System.Text.Json.JsonSerializer.Deserialize<List<string>>(rightIdStr) ?? new List<string>();
            }
            if (!rightIds.Exists(c => c == CkeckCode))
            {
                return false;
            }
            return true;
        }
        public class NewTypeRight
        {
            public required string NewTitye { get; set; }
            public required int NewType { get; set; }

            public string? ShowCode { get; set; }

            public string? SaveCode { get; set; }

            public string? DelCode { get; set; }

            public string? OnDownCode { get; set; }

            public string? ShowComment { get; set; }

            public string? DelComment { get; set; }

            public string? OnDownComment { get; set; }

            public string? DalComment { get; set; }

            public string? ShowRep { get; set; }
        }
    }
}