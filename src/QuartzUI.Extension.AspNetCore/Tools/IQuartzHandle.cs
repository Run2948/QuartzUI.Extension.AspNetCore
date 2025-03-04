﻿using QuartzUI.Extension.AspNetCore.Model;
using QuartzUI.Extension.AspNetCore.Service;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartzUI.Extension.AspNetCore.Tools
{
    public interface IQuartzHandle
    {
        Task<ResultQuartzData> AddJob(tab_quarz_task taskOptions);
        Task<List<tab_quarz_task>> GetJobs();
        void InitJobs();
        Task<ResultQuartzData> IsQuartzJob(string taskName, string groupName);
        ResultQuartzData IsValidExpression(string cronExpression);
        Task<ResultQuartzData> Pause(tab_quarz_task taskOptions);
        Task<ResultQuartzData> Remove(tab_quarz_task taskOptions);
        Task<ResultQuartzData> Run(tab_quarz_task taskOptions);
        Task<ResultQuartzData> Start(tab_quarz_task taskOptions);
        Task<ResultQuartzData> Update(tab_quarz_task taskOptions);
    }
}