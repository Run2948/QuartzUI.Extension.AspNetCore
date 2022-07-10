using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzUI.Extension.AspNetCore.Service
{
    public class ResultQuartzData
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class ResultData<T> where T :class
    {
        public int total { get; set; }
        public List<T> data { get; set; }
    }
}
