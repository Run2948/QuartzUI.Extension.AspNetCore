using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartzUI.Extension.AspNetCore.Enum;
using QuartzUI.Extension.AspNetCore.Model;
using QuartzUI.Extension.AspNetCore.Service;
using QuartzUI.Extension.AspNetCore.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace QuartzUI.Extension.AspNetCore.Areas.MyFeature.Pages
{
    public class MainModel : PageModel
    {
        private readonly IQuartzHandle _quartzHandle;
        private readonly IQuartzLogService _logService;
        private readonly bool _isDebug;
        private readonly string _token;
        private readonly string _superToken;

        public MainModel(IQuartzHandle quartzHandle, IQuartzLogService logService, IConfiguration configuration)
        {
            _quartzHandle = quartzHandle;
            _logService = logService;
            _isDebug = Convert.ToBoolean(configuration["QuartzUI:Debug"] ?? "False");
            _token = configuration["QuartzUI:Token"];
            _superToken = configuration["QuartzUI:SuperToken"];
        }

        [BindProperty]
        public tab_quarz_task Input { get; set; }
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetSelectJob()
        {
            var jobs = await _quartzHandle.GetJobs();
            return new JsonDataResult(jobs);
        }
        /// <summary>
        /// 新建任务
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddJob()
        {
            var data = await _quartzHandle.AddJob(Input);
            Input.Status = Convert.ToInt32(JobState.暂停);
            return new JsonDataResult(data);
        }
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostPauseJob()
        {
            var data = await _quartzHandle.Pause(Input);
            return new JsonDataResult(data);
        }
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostStartJob()
        {
            var data = await _quartzHandle.Start(Input);
            return new JsonDataResult(data);
        }
        /// <summary>
        /// 立即执行任务
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostRunJob()
        {
            var data = await _quartzHandle.Run(Input);
            return new JsonDataResult(data);
        }
        /// <summary>
        /// 修改任务
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUpdateJob()
        {
            var data = await _quartzHandle.Update(Input);
            return new JsonDataResult(data);
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteJob()
        {
            var data = await _quartzHandle.Remove(Input);
            return new JsonDataResult(data);
        }
        /// <summary>
        /// 获取任务执行记录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostJobRecord(DateTime? startDate, DateTime? endDate, string taskName, string groupName, int current, int size)
        {
            var data = await _logService.GetLogs(startDate, endDate, taskName, groupName, current, size);
            return new JsonDataResult(data);
        }
        /// <summary>
        /// 删除任务执行记录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteJobRecord(DateTime? startDate, DateTime? endDate, string taskName, string groupName)
        {
            var data = await _logService.DeleteLogs(startDate, endDate, taskName, groupName);
            return new JsonDataResult(data);
        }

        public IActionResult OnGet()
        {
            if (_isDebug)
                HttpContext.Session.SetString("QuartzUIToken", _superToken ?? _token);
            var loginToken = HttpContext.Session.GetString("QuartzUIToken");
            if (string.IsNullOrEmpty(loginToken))
                return Redirect("/QuartzLogin");
            return Page();
        }
    }
}
