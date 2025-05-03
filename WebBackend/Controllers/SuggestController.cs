using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
    public class SuggestController: Controller
    {
        public IActionResult Index()
        {
            var res=new NewPage(){
                NewTypeId=0,
                Title="意见收集"
            };
            return View(res);
        }
    }
}