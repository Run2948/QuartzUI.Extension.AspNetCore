using QuartzUI.Extension.AspNetCore.EFContext;
using QuartzUI.Extension.AspNetCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzUI.Extension.AspNetCore.Service
{
    public class EFQuartzLogService : IQuartzLogService
    {
        private QuarzEFContext _quarzEFContext;
        private readonly DbContextOptions<QuarzEFContext> _options;

        public EFQuartzLogService(QuarzEFContext quarzEFContext, DbContextOptions<QuarzEFContext> options)
        {
            _quarzEFContext = quarzEFContext;
            _options = options;
        }

        public async Task<ResultQuartzData> AddLog(tab_quarz_tasklog tab_Quarz_Tasklog)
        {
            using var context = new QuarzEFContext(_options);
            context.tab_quarz_tasklog.Add(tab_Quarz_Tasklog);
            var data = await context.SaveChangesAsync();

            var result = new ResultQuartzData { status = false, message = "" };
            if (data > 0)
            {
                result.status = true;
                result.message = "数据库任务日志添加成功!";
            }
            return result;
        }


        public async Task<ResultQuartzData> DeleteLogs(DateTime? startDate, DateTime? endDate, string taskName, string groupName)
        {
            using var context = new QuarzEFContext(_options);
            var removeLogs = context.tab_quarz_tasklog.Where(a => a.TaskName == taskName && a.GroupName == groupName && (startDate == null || a.BeginDate >= startDate) && (endDate == null || a.BeginDate <= endDate)).ToArray();
            context.tab_quarz_tasklog.RemoveRange(removeLogs);
            var data = await context.SaveChangesAsync();

            var result = new ResultQuartzData { status = false, message = "" };
            if (data > 0)
            {
                result.status = true;
                result.message = "数据库任务日志删除成功!";
            }
            return result;
        }

        public async Task<tab_quarz_tasklog> Getlastlog(string taskName, string groupName)
        {
            var data = await _quarzEFContext.tab_quarz_tasklog.Where(a => a.TaskName == taskName && a.GroupName == groupName).OrderByDescending(a => a.BeginDate).FirstOrDefaultAsync();
            return data;
        }

        public async Task<ResultData<tab_quarz_tasklog>> GetLogs(DateTime? startDate, DateTime? endDate, string taskName, string groupName, int page, int pageSize = 100)
        {
            int total = _quarzEFContext.tab_quarz_tasklog.Where(a => a.TaskName == taskName && a.GroupName == groupName && (startDate == null || a.BeginDate >= startDate) && (endDate == null || a.BeginDate <= endDate)).Count();
            var pagem = page - 1;
            var data = await _quarzEFContext.tab_quarz_tasklog.Where(a => a.TaskName == taskName && a.GroupName == groupName && (startDate == null || a.BeginDate >= startDate) && (endDate == null || a.BeginDate <= endDate)).OrderByDescending(a => a.BeginDate).Skip(pagem * pageSize).Take(pageSize).ToListAsync();
            ResultData<tab_quarz_tasklog> resultData = new ResultData<tab_quarz_tasklog>() { total = total, data = data };
            return resultData;
        }
    }
}
