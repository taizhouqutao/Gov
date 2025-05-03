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
                var ResPath="/Uploads/Contact/";
                string fileName=string.Empty;
                // 4. 保存每个文件
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    var file = uploadedFiles[i];
                    if (file.Length > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        var fileExt=fileName.Split('.').LastOrDefault();
                        // 生成随机数作为文件名
                        var NewFileName=DateTime.Now.ToString("yyyyMMddHHmmssfff");
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
                return Json(new { success = true, message = "上传成功",file=$"{ResPath}{fileName}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "上传失败: " + ex.Message });
            }
        }
    }
}