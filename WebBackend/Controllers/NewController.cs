using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
    public class NewController: Controller
    {
        private BLL.BllNew bll = new BLL.BllNew();
        private BLL.BllComment bllcomment = new BLL.BllComment();
        private BLL.BllNewType bllNewType = new BLL.BllNewType();

        public async Task<IActionResult> Index(int NewTypeId)
        {
            var NewType = await bllNewType.GetNewTypeByIdAsync(NewTypeId);
            var NewPage=new NewPage(){
                NewTypeId=NewType.Id,
                Title=NewType.NewTypeName
            };
            return View(NewPage);
        }

        public async Task<IActionResult> Comment(int NewTypeId)
        {
            var NewType = await bllNewType.GetNewTypeByIdAsync(NewTypeId);
            var NewPage=new NewPage(){
                NewTypeId=NewType.Id,
                Title=NewType.NewTypeName
            };
            return View(NewPage);
        }

        [HttpPost]
        public async Task<Response<PageList<CommentResDto>>> GetCommentsByPage([FromBody] PageReq<CommentReqDto> req)
        {
            try
            {
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
                var res = await bll.GetNewsByPageAsync(req);
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
            try
            {
                New res = null;
                if (req.id != null)
                {
                    res = await bll.GetNewByIdAsync(Convert.ToInt32(req.id));
                    if (res == null) throw new Exception("编码对应实体不存在");
                    res.NewTitle = req.newTitle ?? "";
                    res.NewContent=req.newContent??"";
                    await bll.UpdateNewAsync(res);
                }
                else
                {
                    res = new New()
                    {
                        NewTitle = req.newTitle ?? "",
                        IsPublic=0,
                        NewContent=req.newContent??"",
                        IfDel = 0,
                        CreateTime = DateTime.Now,
                        CreateUserId = 1,
                        NewTypeId=req.newTypeId
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
            try
            {
                if (req.ids != null && req.ids.Count>0)
                {
                    await bll.DelNewAsync(req.ids);
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
            try
            {
                if (req.ids != null && req.ids.Count>0)
                {
                    var News = await bll.GetNewsByIdAsync(req.ids);
                    News.ForEach(i=>{
                      i.IsPublic=(int) req.isPublic;
                      i.PublicTime=((int) req.isPublic)==1?DateTime.Now:null;
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
            try
            {
                if (req.ids != null && req.ids.Count>0)
                {
                    await bllcomment.DelCommentAsync(req.ids);
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
            try
            {
                if (req.ids != null && req.ids.Count>0)
                {
                    await bllcomment.SetCommentShowAsync(req);
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