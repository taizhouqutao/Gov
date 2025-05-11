using Microsoft.AspNetCore.Mvc;
using Common;
using BLL;
using DAL.Modles;
namespace Web.Controllers
{
    public class ContactController: Controller
    {
        public IConfiguration configuration;
        BllContact bllContact=new BllContact();
        BllContactMessage bllContactMessage=new BllContactMessage();
        public ContactController()
        {
          configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
        public async Task<IActionResult> Index()
        {
            var res = await bllContact.GetContactsByAsync(new ContactReqDto(){});
            string Url = configuration["BackEndPoint:Url"];
            var contact=new ContactPageDto()
            {
              contactList=res.ConvertAll(i=>{
                var PersonHead=string.IsNullOrEmpty(i.PersonHead)?"/img/unperson.jpg":i.PersonHead;
                return new ContactPageItemDto(){
                  id=i.Id,
                  depent=i.Depent,
                  personHead=$"{Url}{PersonHead}",
                  personName=i.PersonName,
                  post=i.Post
                };
              })
            };
            return View(contact);
        }

        public async Task<IActionResult> Resume(int id)
        {
            var res = await bllContact.GetContactByIdAsync(id);
            string Url = configuration["BackEndPoint:Url"];
            var PersonHead=string.IsNullOrEmpty(res.PersonHead)?"/img/unperson.jpg":res.PersonHead;
            var contact=new ContactPageDetailDto()
            {
                id=res.Id,
                depent=res.Depent,
                personHead=$"{Url}{PersonHead}",
                personName=res.PersonName,
                post=res.Post,
                desc=res.Desc
            };
            return View(contact);
        }

        public async Task<IActionResult> Write(int id)
        {
            var res = await bllContact.GetContactByIdAsync(id);
            string Url = configuration["BackEndPoint:Url"];
            var PersonHead=string.IsNullOrEmpty(res.PersonHead)?"/img/unperson.jpg":res.PersonHead;
            var contact=new ContactPageDetailDto()
            {
                id=res.Id,
                depent=res.Depent,
                personHead=$"{Url}{PersonHead}",
                personName=res.PersonName,
                post=res.Post,
                desc=res.Desc
            };
            return View(contact);
        }
    
        [HttpPost]
        public async Task<Response<PageList<ContactMessageDto>>> GetMessage([FromBody] PageReq<ContactMessageReqDto> req)
        {
            try
            {
                var res = await bllContactMessage.GetContactMessagesByPageAsync(new PageReq<ContactMessageReqDto>(){
                    start=req.start,
                    length=req.length,
                    Query=req.Query
                });
                return new Response<PageList<ContactMessageDto>>
                {
                    IfSuccess = 1,
                    Data = new PageList<ContactMessageDto>(){
                        recordsTotal=res.recordsTotal,
                        draw=res.draw,
                        recordsFiltered=res.recordsFiltered,
                        data=res.data.ConvertAll(j=>new ContactMessageDto(){
                            content=j.Content,
                            createTime=j.CreateTime.ToString("yyyy-MM-dd"),
                            visitorName="",
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
    }
}