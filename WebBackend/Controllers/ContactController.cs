using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebBackend.Controllers
{
    public class ContactController : Controller
    {
        BllContact bllContact = new BllContact();
        BllContactMessage bllContactMessage = new BllContactMessage();
        BllCity bllcity = new BllCity();
        BllUser blluser = new BllUser();
        BllDuty bllDuty = new BllDuty();

        [Authorize("009001")]
        public async Task<IActionResult> Index()
        {
            List<CityResDto> citys = new List<CityResDto>();
            List<int> cityIds = new List<int>();
            if (HttpContext.Session.GetString("CityIds") != null)
            {
                cityIds = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CityIds") ?? "[]");
                citys = await bllcity.GetCitysByIdAsync(cityIds);
            }
            var Contacts = await bllContact.GetContactsByAsync(new ContactReqDto() { cityIds = cityIds });
            var ContactPageDto = new ContactPageDto()
            {
                contactList = Contacts.ConvertAll(i => new ContactPageItemDto()
                {
                    depent = i.Depent,
                    personName = i.PersonName,
                    cityName = i.CityName,
                    post = i.Post,
                    id = i.Id,
                    personHead = (string.IsNullOrEmpty(i.PersonHead)) ? "/img/unperson.jpg" : i.PersonHead
                }),
                Citys = citys
            };
            return View(ContactPageDto);
        }

        [Authorize("009003")]
        public async Task<IActionResult> Message()
        {
            return View();
        }

        [Authorize("009002")]
        public IActionResult Duty()
        {
            return View();
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
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Contact"); // 存储路径

                // 3. 如果目录不存在则创建
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                var ResPath = "/Uploads/Contact/";
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

        [HttpPost]
        public async Task<Response<Contact>> GetContactById([FromBody] ContactReqDto req)
        {
            try
            {
                var res = await bllContact.GetContactByIdAsync(Convert.ToInt32(req.id));
                if (res == null) throw new Exception("编码对应实体不存在");
                return new Response<Contact>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<Contact>()
                {
                    Msg = ex.Message
                };
            }
        }

        [Authorize("009001003")]
        [HttpPost]
        public async Task<Response> DelContact([FromBody] RoleReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await bllContact.DelContactAsync(req.ids, UserId ?? 0);
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

        [Authorize("009001001")]
        [HttpPost]
        public async Task<Response<ContactPlusDto>> SaveContact([FromBody] ContactReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                Contact? res = null;
                string CityName = "全部";
                if (req.id != null)
                {
                    res = await bllContact.GetContactByIdAsync(Convert.ToInt32(req.id));
                    if (res == null) throw new Exception("编码对应实体不存在");
                    res.PersonName = req.personName ?? "";
                    res.Post = req.post ?? "";
                    res.CityId = req.cityId;
                    res.Depent = req.depent;
                    res.Desc = req.personDesc;
                    res.PersonHead = req.personHead;
                    res.UpdateTime = DateTime.Now;
                    res.UpdateUserId = UserId ?? 0;
                    res.Cellphone = req.cellphone ?? "";
                    await bllContact.UpdateContactAsync(res);
                }
                else
                {
                    res = new Contact()
                    {
                        IfDel = 0,
                        CreateTime = DateTime.Now,
                        CreateUserId = UserId ?? 0,
                        CityId = req.cityId,
                        PersonName = req.personName ?? "",
                        Post = req.post ?? "",
                        Depent = req.depent,
                        Desc = req.personDesc,
                        Cellphone = req.cellphone ?? "",
                        PersonHead = req.personHead,
                    };
                    await bllContact.AddContactAsync(res);
                }
                if ((res.CityId ?? 0) > 0)
                {
                    var City = await bllcity.GetCityByIdAsync(res.CityId ?? 0);
                    if (City != null)
                    {
                        CityName = City.CityName ?? "";
                    }
                }
                return new Response<ContactPlusDto>
                {
                    IfSuccess = 1,
                    Data = new ContactPlusDto()
                    {
                        Cellphone = res.Cellphone,
                        CreateTime = res.CreateTime,
                        CreateUserId = res.CreateUserId,
                        IfDel = res.IfDel,
                        PersonName = res.PersonName,
                        Post = res.Post,
                        CityId = res.CityId,
                        Depent = res.Depent,
                        Desc = res.Desc,
                        Id = res.Id,
                        CityName = CityName,
                        PersonHead = res.PersonHead,
                        UpdateTime = res.UpdateTime,
                        UpdateUserId = res.UpdateUserId
                    },
                };
            }
            catch (Exception ex)
            {
                return new Response<ContactPlusDto>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<List<DutyDetailDto>>> GetDutyList([FromBody] DutyReqDto req)
        {
            try
            {
                if (HttpContext.Session.GetString("CityIds") != null)
                {
                    var cityIds = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CityIds") ?? "[]");
                    req.cityIds = cityIds;
                }
                var res = await bllDuty.GetDutyDetailsByAsync(req);
                return new Response<List<DutyDetailDto>>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<List<DutyDetailDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<List<ContactPlusDto>>> GetContactList([FromBody] ContactReqDto req)
        {
            try
            {
                var res = await bllContact.GetContactsByAsync(req);
                if (res == null) throw new Exception("编码对应实体不存在");
                return new Response<List<ContactPlusDto>>
                {
                    IfSuccess = 1,
                    Data = res,
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ContactPlusDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<List<DutyContactDto>>> GetContactWithDutyList([FromBody] DutyReqDto req)
        {
            try
            {
                List<int> cityIds = new List<int>();
                if (HttpContext.Session.GetString("CityIds") != null)
                {
                    cityIds = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("CityIds") ?? "[]");
                }
                var res = await bllContact.GetContactsByAsync(new ContactReqDto() { cityIds = cityIds });
                List<DutyDetailDto> DutyDetails = new List<DutyDetailDto>();
                if (req != null && !string.IsNullOrEmpty(req.startDateStr))
                {
                    DutyDetails = await bllDuty.GetDutyDetailsByAsync(new DutyReqDto()
                    {
                        startDateStr = req.startDateStr,
                        endDateStr = req.startDateStr,
                        cityIds = cityIds
                    });
                }
                return new Response<List<DutyContactDto>>
                {
                    IfSuccess = 1,
                    Data = res.ConvertAll(i =>
                    {
                        var ifDuty = (DutyDetails.Exists(j => j.contactId == i.Id));
                        return new DutyContactDto
                        {
                            id = i.Id,
                            personName = i.PersonName,
                            ifDuty = ifDuty,
                            post = i.Post
                        };
                    }),
                };
            }
            catch (Exception ex)
            {
                return new Response<List<DutyContactDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [Authorize("009002001")]
        [HttpPost]
        public async Task<Response> SaveDuty([FromBody] DutyReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                await bllDuty.SaveDuty(req, UserId ?? 0);
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
        public async Task<Response<PageList<ContactMessageResDto>>> GetMessage([FromBody] PageReq<ContactMessageReqDto> req)
        {
            try
            {
                var res = await bllContactMessage.GetContactMessagesByPageAsync(new PageReq<ContactMessageReqDto>()
                {
                    start = req.start,
                    length = req.length,
                    Query = req.Query
                });
                var replys = await bllContactMessage.GetContactMessagesByAsync(new ContactMessageReqDto()
                {
                    fatherContactMessageIds = res.data.ConvertAll(j => j.ContactId)
                });
                var Contacts = await bllContact.GetContactsByAsync(new ContactReqDto()
                {
                    ids = res.data.ConvertAll(j => j.ContactId)
                });
                return new Response<PageList<ContactMessageResDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<ContactMessageResDto>()
                    {
                        recordsTotal = res.recordsTotal,
                        draw = res.draw,
                        recordsFiltered = res.recordsFiltered,
                        data = res.data.ConvertAll(i =>
                        {
                            var Contact = Contacts.FirstOrDefault(j => j.Id == i.ContactId);
                            return new ContactMessageResDto()
                            {
                                ContactId = i.ContactId,
                                Content = i.Content,
                                CreateTime = i.CreateTime,
                                CreateUserId = i.CreateUserId,
                                FatherContactMessageId = i.FatherContactMessageId,
                                IfDeal = i.IfDeal,
                                PersonName = i.PersonName,
                                RoleType = i.RoleType,
                                Id = i.Id,
                                CityName = i.CityName,
                                IsShow = i.IsShow,
                                PersonCellphone = i.PersonCellphone,
                                UpdateTime = i.UpdateTime,
                                UpdateUserId = i.UpdateUserId,
                                ContactName = Contact == null ? "" : Contact.PersonName,
                                UserId = i.UserId
                            };
                        })
                    },
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<ContactMessageResDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<CommentResDetailDto?>> GetMessageById([FromBody] ContactMessageReqDto req)
        {
            try
            {
                var res = await bllContactMessage.GetContactMessageDetailByIdAsync(Convert.ToInt32(req.id));
                if (res == null) throw new Exception("编码对应实体不存在");
                var SonComments = await bllContactMessage.GetContactMessagesByAsync(new ContactMessageReqDto()
                {
                    fatherContactMessageId = res.Id
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

        [Authorize("009003001")]
        [HttpPost]
        public async Task<Response> DelMessages([FromBody] CommentReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await bllContactMessage.DelMessagesAsync(req.ids, UserId ?? 0);
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

        [Authorize("009003002")]
        [HttpPost]
        public async Task<Response> SetMessageShow([FromBody] ContactMessageReqDto req)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (req.ids != null && req.ids.Count > 0)
                {
                    await bllContactMessage.SetMessageShow(req, UserId ?? 0);
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

        [Authorize("009003003")]
        [HttpPost]
        public async Task<Response<CommentResDealDto>> DealMessage([FromBody] CommentReqDto req)
        {
            try
            {
                var Comment = await bllContactMessage.GetContactMessageByIdAsync(Convert.ToInt32(req.id));
                var UserId = HttpContext.Session.GetInt32("UserId");
                var User = await blluser.GetUserByIdAsync(Convert.ToInt32(UserId));
                var NewComment = await bllContactMessage.AddContactMessageAsync(new DAL.Modles.ContactMessage()
                {
                    ContactId = Convert.ToInt32(Comment.ContactId),
                    Content = req.content ?? "",
                    CreateTime = DateTime.Now,
                    FatherContactMessageId = Comment.Id,
                    CreateUserId = User.Id,
                    IfDeal = 0,
                    IsShow = 1,
                    PersonCellphone = "",
                    PersonName = User.UserName ?? "",
                    IfDel = 0,
                    UserId = User.Id,
                    RoleType = 1,
                });
                Comment.IfDeal = 1;
                Comment.IsShow = req.isShow ?? 0;
                Comment.UpdateTime = DateTime.Now;
                Comment.UpdateUserId = UserId ?? 0;
                await bllContactMessage.UpdateContactMessageAsync(Comment);
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

    }
}