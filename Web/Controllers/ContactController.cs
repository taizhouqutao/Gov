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
              contactList=res.ConvertAll(i=>new ContactPageItemDto(){
                id=i.Id,
                depent=i.Depent,
                personHead=$"{Url}{i.PersonHead}",
                personName=i.PersonName,
                post=i.Post
              })
            };
            return View(contact);
        }
    }
}