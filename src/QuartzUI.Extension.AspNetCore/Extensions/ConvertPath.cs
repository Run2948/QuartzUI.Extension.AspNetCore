using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QuartzUI.Extension.AspNetCore.Extensions
{
    public static class ConvertPath
    {
        public static bool _windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static string ReplacePath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            if (_windows)
                return path.Replace("/", "\\");
            return path.Replace("\\", "/");

        }
    }
}
