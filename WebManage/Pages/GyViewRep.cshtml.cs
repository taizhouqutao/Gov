using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebManage.Pages;

public class GyViewRepModel : PageModel
{
    private readonly ILogger<GyViewRepModel> _logger;

    public GyViewRepModel(ILogger<GyViewRepModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
