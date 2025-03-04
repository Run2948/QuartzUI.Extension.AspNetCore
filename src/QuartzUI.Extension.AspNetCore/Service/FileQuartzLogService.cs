﻿using QuartzUI.Extension.AspNetCore.Model;
using QuartzUI.Extension.AspNetCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzUI.Extension.AspNetCore.Service
{
    public class FileQuartzLogService : IQuartzLogService
    {

        private QuartzFileHelper _quartzFileHelper;
        public FileQuartzLogService(QuartzFileHelper quartzFileHelper)
        {
            _quartzFileHelper = quartzFileHelper;
        }

        public Task<ResultQuartzData> AddLog(tab_quarz_tasklog tab_Quarz_Tasklog)
        {
            return Task.Run(() =>
            {
                try
                {
                    _quartzFileHelper.WriteJobLogs(tab_Quarz_Tasklog);
                    return new ResultQuartzData { message = "日志数据保存成功!", status = true };
                }
                catch (Exception)
                {
                    return new ResultQuartzData { message = "日志数据保存失败!", status = false };
                }
            });
        }

        public Task<ResultQuartzData> DeleteLogs(DateTime? startDate, DateTime? endDate, string taskName, string groupName)
        {
            return Task.Run(() =>
            {
                try
                {
                    var count = _quartzFileHelper.DeleteJobsLog(a => a.TaskName == taskName && a.GroupName == groupName && (startDate == null || a.BeginDate >= startDate) && (endDate == null || a.BeginDate <= endDate));
                    return new ResultQuartzData { message = $"{count} 条日志数据删除成功!", status = true };
                }
                catch (Exception)
                {
                    return new ResultQuartzData { message = "日志数据删除失败!", status = false };
                }
            });
            
        }

        public Task<tab_quarz_tasklog> Getlastlog(string taskName, string groupName)
        {
            return Task.Run(() =>
            {
                var list = _quartzFileHelper.GetJobsLog();
                var date = list.Where(a => a.TaskName == taskName && a.GroupName == groupName).OrderByDescending(a => a.BeginDate).FirstOrDefault();
                return date;
            });
        }

        public Task<ResultData<tab_quarz_tasklog>> GetLogs(DateTime? startDate, DateTime? endDate, string taskName, string groupName, int page, int pageSize = 10)
        {
            return Task.Run(() =>
            {
                var list = _quartzFileHelper.GetJobsLog();
                int total = list.Where(a => a.TaskName == taskName && a.GroupName == groupName && (startDate == null || a.BeginDate >= startDate) && (endDate == null || a.BeginDate <= endDate)).Count();
                var data = list.Where(a => a.TaskName == taskName && a.GroupName == groupName && (startDate == null || a.BeginDate >= startDate) && (endDate == null || a.BeginDate <= endDate)).OrderByDescending(a => a.BeginDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ResultData<tab_quarz_tasklog> resultData = new ResultData<tab_quarz_tasklog>() { total = total, data = data };
                return resultData;
            });
        }
    }
}
