using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeanBot.Utils
{
    public class ExampleClass
    {
        private LogUtils _log;
        private IConfiguration _config;
        public ExampleClass(LogUtils logger, IConfiguration config)
        {
            _config = config;
            _log = logger;
            var hehe = _config.GetValue<string>("tehe");
            Log.Information($"Hehe: {hehe}");
        }
    }
}
