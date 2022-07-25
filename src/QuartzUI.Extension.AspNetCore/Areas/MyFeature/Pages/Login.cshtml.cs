using QuartzUI.Extension.AspNetCore.Model;
using QuartzUI.Extension.AspNetCore.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace QuartzUI.Extension.AspNetCore.Areas.MyFeature.Pages
{
    public class LoginModel : PageModel
    {
        private readonly string _token;
        private readonly string _superToken;

        public LoginModel(IConfiguration configuration)
        {
            _token = configuration["QuartzUI:Token"];
            _superToken = configuration["QuartzUI:SuperToken"];
        }

        [BindProperty]
        public string Token { get; set; }
        public string Msg { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(_token) && string.IsNullOrEmpty(_superToken))
            {
                Msg = "首次使用请在 QuartzUI 节点中配置管理员口令";
                return Page();
            }

            var loginToken = HttpContext.Session.GetString("QuartzUIToken");
            if (!string.IsNullOrEmpty(loginToken))
                return Redirect("/QuartzUI");

            return Page();
        }

        public IActionResult OnPost()
        {
            var data = new ResultQuartzData() { status = Token != null && (Token == _token || Token == _superToken) };
            if (data.status)
                HttpContext.Session.SetString("QuartzUIToken", Token);
            data.message = data.status ? "/QuartzUI" : "管理员口令不正确";
            return new JsonDataResult(data);
        }
    }
}
