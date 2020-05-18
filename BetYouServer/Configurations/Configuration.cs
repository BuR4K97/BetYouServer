using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BetYouServer.Controllers;

namespace BetYouServer.Configurations
{
    public static class Configuration
    {
        private static readonly IServiceProvider services = ConfigureServices();

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<ServerConfiguration>()
                .AddSingleton<DatabaseConfiguration>()
                .AddSingleton<DBConnectionController>()
                .AddSingleton<DatabaseController>()
                .AddSingleton<AccountController>()
                .BuildServiceProvider();
        }

        public static void Initialize() {}

        public static Type GetService<Type>()
        {
            return services.GetService<Type>();
        }

    }
}
