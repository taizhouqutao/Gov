using Microsoft.AspNetCore.Mvc;
using Common;
using Web.Lib;
namespace Web.Controllers
{
  public class SuggestController : Controller
  {
    private BLL.BllNewType bllNewType = new BLL.BllNewType();
    private BLL.BllComment bllComment = new BLL.BllComment();
    private readonly ILogger<SuggestController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SuggestController(ILogger<SuggestController> logger, IHttpContextAccessor httpContextAccessor)
    {
      _logger = logger;
      _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> Index()
    {
      var CityId = WebHelp.GetCityId(_httpContextAccessor);
      var CommentInfo = await bllComment.GetCommentsByPageAsync(new PageReq<CommentReqDto>()
      {
        start = 0,
        length = 1,
        Query = new CommentReqDto()
        {
          fatherCommentId = 0,
          newId = 0,
          isShow = 1,
          newTypeId = 0,
          cityIds = CityId == 0 ? null : new List<int>() { CityId }
        }
      });
      var newDetailPage = new NewDetailPage()
      {
        NewContent = "",
        NewTitle = "",
        TotalCount = CommentInfo.recordsFiltered
      };
      return View(newDetailPage);
    }


    [HttpPost]
    public async Task<Response<PageList<ContactMessageDto>>> GetComment([FromBody] PageReq<ContactMessageReqDto> req)
    {
      try
      {
        var CityId = WebHelp.GetCityId(_httpContextAccessor);
        var res = await bllComment.GetContactMessagesByPageAsync(new PageReq<ContactMessageReqDto>()
        {
          start = req.start,
          length = req.length,
          Query = new ContactMessageReqDto()
          {
            contactId = req.Query.contactId,
            fatherContactMessageId = 0,
            isShow = 1,
            cityIds = CityId == 0 ? null : new List<int>() { CityId }
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
        var CityId = WebHelp.GetCityId(_httpContextAccessor);
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
          CityId = CityId,
          IfDel = 0,
          IsShow = 0
        });
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