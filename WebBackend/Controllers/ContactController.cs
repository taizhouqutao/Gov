using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
    public class ContactController: Controller
    {
        BllContact bllContact=new BllContact();
        public async Task<IActionResult> Index()
        {
            var Contacts = await bllContact.GetContactsByAsync(new ContactReqDto(){});
            var ContactPageDto=new ContactPageDto(){
             contactList=Contacts.ConvertAll(i=>new ContactPageItemDto(){
                depent=i.Depent,
                personName=i.PersonName,
                post=i.Post,
                id=i.Id,
                personHead= (string.IsNullOrEmpty(i.PersonHead))?"/img/unperson.jpg": i.PersonHead
             })
            };
            return View(ContactPageDto);
        }
    }
}