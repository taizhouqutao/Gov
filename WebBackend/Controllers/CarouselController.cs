using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using Common;
using DAL.Modles;
using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers
{
  public class CarouselController : Controller
  {
    BllCarousel bllCarousel = new BllCarousel();
    public IActionResult Index()
    {
      return View();
    }
    [HttpPost]
    public async Task<Response<PageList<Carousel>>> GetCarouselsByPage([FromBody] PageReq<CarouselReqDto> req)
    {
      try
      {
        var res = await bllCarousel.GetCarouselsByPageAsync(req);
        return new Response<PageList<Carousel>>
        {
          IfSuccess = 1,
          Data = res
        };
      }
      catch (Exception ex)
      {
        return new Response<PageList<Carousel>>()
        {
          Msg = ex.Message
        };
      }
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
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Carousel"); // 存储路径

        // 3. 如果目录不存在则创建
        if (!Directory.Exists(savePath))
        {
          Directory.CreateDirectory(savePath);
        }
        var ResPath = "/Uploads/Carousel/";
        string fileName = string.Empty;
        // 4. 保存每个文件
        for (int i = 0; i < uploadedFiles.Count; i++)
        {
          var file = uploadedFiles[i];
          if (file.Length > 0)
          {
            fileName = Path.GetFileName(file.FileName);
            var fileExt = fileName.Split('.').LastOrDefault();
            // 生成随机数作为文件名
            var NewFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
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
        return Json(new { success = true, message = "上传成功", file = $"{ResPath}{fileName}" });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "上传失败: " + ex.Message });
      }
    }

    [HttpPost]
    public async Task<Response<Carousel>> SaveCarousel([FromBody] CarouselReqDto req)
    {
      try
      {
        var UserId = HttpContext.Session.GetInt32("UserId");
        Carousel? res = null;
        if (req.id != null)
        {
          res = await bllCarousel.GetCarouselByIdAsync(Convert.ToInt32(req.id));
          if (res == null) throw new Exception("编码对应实体不存在");
          res.ImageUrl = req.imageUrl ?? "";
          res.IsPublic = Convert.ToInt32(req.isPublic);
          res.LinkUrl = req.linkUrl;
          res.Title = req.title ?? "";
          res.PublicTime = Convert.ToInt32(req.isPublic) == 1 ? DateTime.Now : null;
          res.PublicUserId = Convert.ToInt32(req.isPublic) == 1 ? UserId : null;
          res.UpdateUserId = UserId ?? 0;
          res.UpdateTime = DateTime.Now;
          await bllCarousel.UpdateCarouselAsync(res);
        }
        else
        {
          res = new Carousel()
          {
            IfDel = 0,
            CreateTime = DateTime.Now,
            CreateUserId = UserId ?? 0,
            ImageUrl = req.imageUrl ?? "",
            IsPublic = Convert.ToInt32(req.isPublic),
            LinkUrl = req.linkUrl,
            Title = req.title ?? "",
            PublicTime = Convert.ToInt32(req.isPublic) == 1 ? DateTime.Now : null,
            PublicUserId = Convert.ToInt32(req.isPublic) == 1 ? UserId : null,
          };
          await bllCarousel.AddCarouselAsync(res);
        }
        return new Response<Carousel>
        {
          IfSuccess = 1,
          Data = res,
        };
      }
      catch (Exception ex)
      {
        return new Response<Carousel>()
        {
          Msg = ex.Message
        };
      }
    }

    [HttpPost]
    public async Task<Response> DelCarousel([FromBody] CarouselReqDto req)
    {
      try
      {
        var UserId = HttpContext.Session.GetInt32("UserId");
        if (req.ids != null && req.ids.Count > 0)
        {
          await bllCarousel.DelCarouselAsync(req.ids, UserId ?? 0);
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
    public async Task<Response<Carousel>> GetCarouselById([FromBody] CarouselReqDto req)
    {
      try
      {
        var res = await bllCarousel.GetCarouselByIdAsync(Convert.ToInt32(req.id));
        if (res == null) throw new Exception("编码对应实体不存在");
        return new Response<Carousel>
        {
          IfSuccess = 1,
          Data = res,
        };
      }
      catch (Exception ex)
      {
        return new Response<Carousel>()
        {
          Msg = ex.Message
        };
      }
    }

    [HttpPost]
    public async Task<Response> SetCarouselPublic([FromBody] CarouselReqDto req)
    {
      try
      {
        var UserId = HttpContext.Session.GetInt32("UserId");
        if (req.ids != null && req.ids.Count > 0)
        {
          var Carousels = await bllCarousel.GetCarouselsByIdAsync(req.ids);
          Carousels.ForEach(i =>
          {
            i.IsPublic = (int)req.isPublic;
            i.PublicTime = ((int)req.isPublic) == 1 ? DateTime.Now : null;
            i.UpdateTime = DateTime.Now;
            i.UpdateUserId = UserId ?? 0;
          });
          await bllCarousel.UpdateCarouselsAsync(Carousels);
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