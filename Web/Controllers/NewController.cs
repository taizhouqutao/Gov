using Microsoft.AspNetCore.Mvc;
using Common;

namespace Web.Controllers
{
    public class NewController : Controller
    {
        private BLL.BllNewType bllNewType = new BLL.BllNewType();
        private BLL.BllNew bll = new BLL.BllNew();

        private readonly ILogger<NewController> _logger;
        public NewController(ILogger<NewController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int NewTypeId)
        {
            var NewType = await bllNewType.GetNewTypeByIdAsync(NewTypeId);
            var res = await bll.GetNewsByPageAsync(new PageReq<NewReqDto>(){
                start=1,
                length=1,
                Query=new NewReqDto(){
                    isPublic=1,
                    newTypeId=NewTypeId
                }
            });
            var newPage=new NewPage(){
                NewTypeId=NewType.Id,
                Title=NewType.NewTypeName,
                TotalCount=res.recordsTotal
            };
            return View(newPage);
        }

        [HttpPost]
        public async Task<Response<PageList<NewListDto>>> GetNewsByPage([FromBody] PageReq<NewReqDto> req)
        {
            try
            {
                var res = await bll.GetNewsByPageAsync(new PageReq<NewReqDto>(){
                    start=req.start,
                    length=req.length,
                    Query=new NewReqDto(){
                        isPublic=1,
                        newTypeId=req.Query?.newTypeId
                    }
                });
                return new Response<PageList<NewListDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<NewListDto>(){
                        recordsTotal=res.recordsTotal,
                        draw=res.draw,
                        recordsFiltered=res.recordsFiltered,
                        data=res.data.ConvertAll(j=>new NewListDto(){
                            CreateTime=j.CreateTime,
                            CreateUserId=j.CreateUserId,
                            Id=j.Id,
                            IsPublic=j.IsPublic,
                            NewTitle=j.NewTitle,
                            PublicTime=j.PublicTime,
                            PublicUserId=j.PublicUserId,
                            UpdateTime=j.UpdateTime,
                            UpdateUserId=j.UpdateUserId
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
            var newDetailPage=new NewDetailPage(){
                Id=NewId,
                NewContent=New.NewContent,
                NewTitle=New.NewTitle,
                PublicTime=New.PublicTime
            };
            return View(newDetailPage);
        }
    }
}
