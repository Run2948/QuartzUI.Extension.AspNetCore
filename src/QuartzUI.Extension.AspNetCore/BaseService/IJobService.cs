using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzUI.Extension.AspNetCore.BaseService
{
    public interface IJobService
    {
        string ExecuteService( string parameter);
    }
}
