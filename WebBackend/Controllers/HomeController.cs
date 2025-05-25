using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBackend.Models;
using Common;
using Microsoft.AspNetCore.Authorization;
using BLL;
using DAL.Modles;
using Newtonsoft.Json;
namespace WebBackend.Controllers;

public class HomeController : Controller
{
    private BLL.BllUser blluser = new BLL.BllUser();
    private BLL.BllContactMessage bllContactMessage = new BLL.BllContactMessage();
    private BLL.BllComment bllcomment = new BLL.BllComment();
    private BLL.BllViewLog bllviewlog = new BLL.BllViewLog();
    private BLL.BllNewType bllNewType = new BLL.BllNewType();
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Forbidden()
    {
        return View();
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Home()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult login()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> newReport(int NewTypeId)
    {
        var NewType = await bllNewType.GetNewTypeByIdAsync(NewTypeId);
        var NewPage = new NewPage()
        {
            NewTypeId = NewType.Id,
            Title = NewType.NewTypeName
        };
        return View(NewPage);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<Response<LoginResDto>> Login([FromBody] LoginReqDto req)
    {
        try
        {
            var res = await blluser.LoginAsync(req);
            if (res == null) throw new Exception("用户名和密码错误");
            HttpContext.Session.SetInt32("UserId", res.Id);
            List<string> Right = await blluser.GetRightCodeByUser(res.Id);
            HttpContext.Session.SetString("RightId", JsonConvert.SerializeObject(Right));
            return new Response<LoginResDto>
            {
                IfSuccess = 1,
                Data = new LoginResDto()
                {
                    RealName = res.RealName,
                    UserName = res.UserName
                }
            };
        }
        catch (Exception ex)
        {
            return new Response<LoginResDto>()
            {
                Msg = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<Response<ViewLogReportResDto>> GetViewLogReports([FromBody] ViewLogReportReqDto req)
    {
        try
        {
            var Now = DateTime.Now.Date;
            req.StartDate = Now.AddDays(-30);
            req.EndDate = Now;
            var res = await bllviewlog.GetViewLogReports(req);
            return new Response<ViewLogReportResDto>()
            {
                Data = res,
                IfSuccess = 1
            };
        }
        catch (Exception ex)
        {
            return new Response<ViewLogReportResDto>()
            {
                Msg = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<Response<ViewLogReportResDto>> GetViewLogDetailReports([FromBody] ViewLogReportReqDto req)
    {
        try
        {
            var Now = DateTime.Now.Date;
            req.StartDate = Now.AddDays(-30);
            req.EndDate = Now;
            var res = await bllviewlog.GetViewLogDetailReports(req);
            return new Response<ViewLogReportResDto>()
            {
                Data = res,
                IfSuccess = 1
            };
        }
        catch (Exception ex)
        {
            return new Response<ViewLogReportResDto>()
            {
                Msg = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<Response<ViewLogLineResDto>> GetViewLogLines([FromBody] ViewLogReportReqDto req)
    {
        try
        {
            var Now = DateTime.Now.Date;
            req.StartDate = Now.AddDays(-7);
            req.EndDate = Now;
            var res = await bllviewlog.GetViewLogLines(req);
            return new Response<ViewLogLineResDto>()
            {
                Data = res,
                IfSuccess = 1
            };
        }
        catch (Exception ex)
        {
            return new Response<ViewLogLineResDto>()
            {
                Msg = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<Response<List<CommentGroupResDto>>> GetCommentGroups([FromBody] CommentReqDto req)
    {
        try
        {
            req.newTypeIds = Common.HtmlHelp.GetShowCommentTypes();
            req.newTypeIds.Add(0);
            req.fatherCommentId = 0;
            req.ifDeal = 0;
            var res = await bllcomment.GetCommentGroupsByAsync(req);
            var part = await bllContactMessage.GetContactMessageGroupsByAsync(new ContactMessageReqDto()
            {
                ifDeal = 0,
                fatherContactMessageId = 0
            });
            res.AddRange(part);
            res = res.OrderByDescending(i => i.createTime).ToList();
            return new Response<List<CommentGroupResDto>>()
            {
                Data = res,
                IfSuccess = 1
            };
        }
        catch (Exception ex)
        {
            return new Response<List<CommentGroupResDto>>()
            {
                Msg = ex.Message
            };
        }
    }
}
