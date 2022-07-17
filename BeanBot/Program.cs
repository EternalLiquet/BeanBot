using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using System;
using BeanBot;
using BeanBot.Utils;
using Serilog;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BeanBot
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appSettings.json", false)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(config)
                .AddSingleton<LogUtils>()
                .AddSingleton<ExampleClass>()
                .BuildServiceProvider();
        }
    }
}
