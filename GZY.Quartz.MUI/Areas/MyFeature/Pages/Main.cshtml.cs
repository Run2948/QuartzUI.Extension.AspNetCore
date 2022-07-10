using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GZY.Quartz.MUI.Enum;
using GZY.Quartz.MUI.Model;
using GZY.Quartz.MUI.Service;
using GZY.Quartz.MUI.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GZY.Quartz.MUI.Areas.MyFeature.Pages
{
    public class MainModel : PageModel
    {
        private readonly IQuartzHandle _quartzHandle;
        private readonly IQuartzLogService _logService;
        public MainModel(IQuartzHandle quartzHandle, IQuartzLogService logService)
        {
            _quartzHandle = quartzHandle;
            _logService = logService;
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
        public async Task<IActionResult> OnPostJobRecord(string taskName, string groupName, int current, int size)
        {
            var data = await _logService.GetLogs(taskName, groupName, current, size);
            return new JsonDataResult(data);
        }

        public IActionResult OnGet()
        {
            var loginToken = HttpContext.Session.GetString("QuartzUIToken");
            if (string.IsNullOrEmpty(loginToken))
                return Redirect("/Login");
            return Page();
        }
    }
}
