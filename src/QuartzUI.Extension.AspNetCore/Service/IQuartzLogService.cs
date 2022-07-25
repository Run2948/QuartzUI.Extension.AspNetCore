using QuartzUI.Extension.AspNetCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuartzUI.Extension.AspNetCore.Service
{
    public interface IQuartzLogService
    {
        Task<ResultData<tab_quarz_tasklog>> GetLogs(DateTime? startDate, DateTime? endDate, string taskName, string groupName, int page, int pageSize = 100);
        Task<tab_quarz_tasklog> Getlastlog(string taskName, string groupName);

        Task<ResultQuartzData> AddLog(tab_quarz_tasklog tab_Quarz_Tasklog);

        Task<ResultQuartzData> DeleteLogs(DateTime? startDate, DateTime? endDate, string taskName, string groupName);
    }
}
