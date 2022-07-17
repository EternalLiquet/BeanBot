using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeanBot.Utils
{
    public class LogUtils
    {
        public LogUtils()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Logger instantiation complete");
        }
    }
}
