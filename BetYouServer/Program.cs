using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BetYouServer.Configurations;

namespace BetYouServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configuration.Initialize();
            WebHost.CreateDefaultBuilder(args).UseStartup<ServerConfiguration>().Build().Run();
        }
    }
}
