using Microsoft.AspNetCore.Mvc;
using Common;
using BLL;
using DAL.Modles;
using Web.Lib;
namespace Web.Controllers
{
  public class ContactController : Controller
  {
    public IConfiguration configuration;
    BllContact bllContact = new BllContact();
    BllContactMessage bllContactMessage = new BllContactMessage();

    private readonly IHttpContextAccessor _httpContextAccessor;
    public ContactController(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
      configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }
    public async Task<IActionResult> Index()
    {
      var CityId = WebHelp.GetCityId(_httpContextAccessor);
      var res = await bllContact.GetContactsByAsync(new ContactReqDto()
      {
        cityIds = new List<int>() { CityId }
      });
      string Url = configuration["BackEndPoint:Url"];
      var contact = new ContactPageDto()
      {
        contactList = res.ConvertAll(i =>
        {
          var PersonHead = string.IsNullOrEmpty(i.PersonHead) ? "/img/unperson.jpg" : i.PersonHead;
          return new ContactPageItemDto()
          {
            id = i.Id,
            depent = i.Depent,
            personHead = $"{Url}{PersonHead}",
            personName = i.PersonName,
            post = i.Post
          };
        })
      };
      return View(contact);
    }

    public async Task<IActionResult> Resume(int id)
    {
      var res = await bllContact.GetContactByIdAsync(id);
      string Url = configuration["BackEndPoint:Url"];
      var PersonHead = string.IsNullOrEmpty(res.PersonHead) ? "/img/unperson.jpg" : res.PersonHead;
      var contactMsg = await bllContactMessage.GetContactMessagesByPageAsync(new PageReq<ContactMessageReqDto>()
      {
        start = 0,
        length = 1,
        Query = new ContactMessageReqDto()
        {
          contactId = id,
          fatherContactMessageId = 0
        }
      });
      var contact = new ContactPageDetailDto()
      {
        id = res.Id,
        depent = res.Depent,
        personHead = $"{Url}{PersonHead}",
        personName = res.PersonName,
        post = res.Post,
        desc = res.Desc,
        TotalCount = contactMsg.recordsFiltered
      };
      return View(contact);
    }

    public async Task<IActionResult> Write(int id)
    {
      var res = await bllContact.GetContactByIdAsync(id);
      string Url = configuration["BackEndPoint:Url"];
      var PersonHead = string.IsNullOrEmpty(res.PersonHead) ? "/img/unperson.jpg" : res.PersonHead;
      var contact = new ContactPageDetailDto()
      {
        id = res.Id,
        depent = res.Depent,
        personHead = $"{Url}{PersonHead}",
        personName = res.PersonName,
        post = res.Post,
        desc = res.Desc
      };
      return View(contact);
    }

    [HttpPost]
    public async Task<Response<PageList<ContactMessageDto>>> GetMessage([FromBody] PageReq<ContactMessageReqDto> req)
    {
      try
      {
        var res = await bllContactMessage.GetContactMessagesByPageAsync(new PageReq<ContactMessageReqDto>()
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
        var replys = await bllContactMessage.GetContactMessagesByAsync(new ContactMessageReqDto()
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
    public async Task<Response<ContactMessageDto>> AddMessage([FromBody] ContactMessageAddDto req)
    {
      try
      {
        ContactMessageDto res = null;
        var result = await bllContactMessage.AddContactMessageAsync(new ContactMessage()
        {
          ContactId = req.contactId,
          Content = req.content,
          CreateTime = DateTime.Now,
          CreateUserId = 0,
          FatherContactMessageId = 0,
          IfDeal = 0,
          PersonCellphone = req.personCellphone,
          PersonName = req.personName,
          RoleType = 0,
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