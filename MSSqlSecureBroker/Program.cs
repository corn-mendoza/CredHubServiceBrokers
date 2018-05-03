﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Logging;
using Steeltoe.Security.DataProtection.CredHubCore;

namespace MSSqlSecureBroker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((builderContext, loggingBuilder) =>
                 {
                     loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                     loggingBuilder.AddDynamicConsole();
                 })
                .UseCredHubInterpolation(new LoggerFactory().AddConsole())
                .Build();
    }
}
