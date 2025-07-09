using Microsoft.AspNetCore.Mvc;
using Common;
using Web.Lib;
namespace Web.Controllers
{
    public class NewController : Controller
    {
        public IConfiguration configuration;
        private BLL.BllNewType bllNewType = new BLL.BllNewType();
        private BLL.BllComment bllComment = new BLL.BllComment();
        private BLL.BllNew bll = new BLL.BllNew();

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<NewController> _logger;
        public NewController(ILogger<NewController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }

        public async Task<IActionResult> Index(int NewTypeId)
        {
            var NewType = await bllNewType.GetNewTypeByIdAsync(NewTypeId);
            var CityId = WebHelp.GetCityId(_httpContextAccessor);
            var res = await bll.GetNewsByPageAsync(new PageReq<NewReqDto>()
            {
                start = 0,
                length = 1,
                Query = new NewReqDto()
                {
                    isPublic = 1,
                    newTypeId = NewTypeId,
                    cityIds = CityId == 0 ? null : new List<int>() { CityId }
                }
            });
            var newPage = new NewPage()
            {
                NewTypeId = NewType.Id,
                Title = NewType.NewTypeName.Replace("管理", ""),
                TotalCount = res.recordsFiltered
            };
            return View(newPage);
        }

        [HttpPost]
        public async Task<Response<PageList<NewListDto>>> GetNewsByPage([FromBody] PageReq<NewReqDto> req)
        {
            try
            {
                var CityId = WebHelp.GetCityId(_httpContextAccessor);
                var res = await bll.GetNewsByPageAsync(new PageReq<NewReqDto>()
                {
                    start = req.start,
                    length = req.length,
                    Query = new NewReqDto()
                    {
                        isPublic = 1,
                        newTypeId = req.Query?.newTypeId,
                        cityIds = CityId == 0 ? null : new List<int>() { CityId }
                    }
                });
                return new Response<PageList<NewListDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<NewListDto>()
                    {
                        recordsTotal = res.recordsFiltered,
                        draw = res.draw,
                        recordsFiltered = res.recordsFiltered,
                        data = res.data.ConvertAll(j => new NewListDto()
                        {
                            CreateTime = j.CreateTime,
                            CreateUserId = j.CreateUserId,
                            Id = j.Id,
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

        public async Task<IActionResult> Detail(int NewId)
        {
            var New = await bll.GetNewByIdAsync(NewId);
            var NewContent = New.NewContent;
            string Url = configuration["BackEndPoint:Url"];
            var CommentInfo = await bllComment.GetCommentsByPageAsync(new PageReq<CommentReqDto>()
            {
                start = 0,
                length = 1,
                Query = new CommentReqDto()
                {
                    fatherCommentId = 0,
                    newId = NewId,
                    isShow = 1
                }
            });
            if (!string.IsNullOrEmpty(NewContent))
            {
                NewContent = System.Text.RegularExpressions.Regex.Replace(
                    NewContent,
                    @"src=""(?!http)([^""]+)""",
                    $@"src=""{Url}$1""",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );
            }
            await bll.AddNewViewCountAsync(NewId);
            var ifHiddenComment = !HtmlHelp.GetShowCommentTypes().Contains(New.NewTypeId ?? 0);
            var newDetailPage = new NewDetailPage()
            {
                Id = NewId,
                NewContent = NewContent,
                NewTitle = New.NewTitle,
                PublicTime = New.PublicTime,
                TotalCount = CommentInfo.recordsFiltered,
                IfHiddenComment = ifHiddenComment
            };
            return View(newDetailPage);
        }

        [HttpPost]
        public async Task<Response<PageList<ContactMessageDto>>> GetComment([FromBody] PageReq<ContactMessageReqDto> req)
        {
            try
            {
                var res = await bllComment.GetContactMessagesByPageAsync(new PageReq<ContactMessageReqDto>()
                {
                    start = req.start,
                    length = req.length,
                    Query = new ContactMessageReqDto()
                    {
                        contactId = req.Query.contactId,
                        fatherContactMessageId = 0,
                        isShow = 1
                    }
                });
                var replys = await bllComment.GetContactMessagesByAsync(new ContactMessageReqDto()
                {
                    fatherContactMessageIds = res.data.ConvertAll(j => j.Id)
                });
                return new Response<PageList<ContactMessageDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<ContactMessageDto>()
                    {
                        recordsTotal = res.recordsFiltered,
                        draw = res.draw,
                        recordsFiltered = res.recordsFiltered,
                        data = res.data.ConvertAll(j => new ContactMessageDto()
                        {
                            content = j.Content,
                            createTime = j.CreateTime.ToString("yyyy-MM-dd"),
                            personName = HtmlHelp.MaskChineseName(j.PersonName),
                            replys = replys.Where(k => k.FatherContactMessageId == j.Id).ToList().ConvertAll(k => new ContactMessageReplyDto()
                            {
                                content = k.Content,
                                createTime = k.CreateTime.ToString("yyyy-MM-dd"),
                                personName = HtmlHelp.MaskChineseName(k.PersonName)
                            })
                        })
                    },
                };
            }
            catch (Exception ex)
            {
                return new Response<PageList<ContactMessageDto>>()
                {
                    Msg = ex.Message
                };
            }
        }

        [HttpPost]
        public async Task<Response<ContactMessageDto>> AddComment([FromBody] ContactMessageAddDto req)
        {
            try
            {
                ContactMessageDto res = null;
                var result = await bllComment.AddCommentAsync(new DAL.Modles.Comment()
                {
                    NewId = req.contactId,
                    Content = req.content,
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    FatherCommentId = 0,
                    IfDeal = 0,
                    PersonCellphone = req.personCellphone,
                    PersonName = req.personName,
                    RoleType = 0,
                    IfDel = 0,
                    IsShow = 0
                });
                await bll.AddNewCommentCountAsync(req.contactId);
                return new Response<ContactMessageDto>
                {
                    IfSuccess = 1,
                    Data = new ContactMessageDto()
                    {
                        content = result.Content,
                        createTime = result.CreateTime.ToString("yyyy-MM-dd"),
                        personName = result.PersonName,
                    },
                };
            }
            catch (Exception ex)
            {
                return new Response<ContactMessageDto>()
                {
                    Msg = ex.Message
                };
            }
        }
    }
}
