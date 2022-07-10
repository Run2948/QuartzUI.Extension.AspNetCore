using QuartzUI.Extension.AspNetCore.BaseService;

namespace QuartzUIDemo.ScheduleJobs
{
    public class TestJob : IJobService
    {
        public string ExecuteService(string parameter)
        {
            return $"Test Job: {parameter}";
        }
    }
}
