using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuartzUI.Extension.AspNetCore.Areas.MyFeature.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("QuartzUIToken");
            HttpContext.Session.Clear();
            return Redirect("/QuartzLogin");
        }
    }
}
