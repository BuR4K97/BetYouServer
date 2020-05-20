using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BetYouServer.Configurations
{
    public class ServerConfiguration
    {
        public const int IdleTimeout = 10;
        
        public IConfiguration Configuration { get; }

        public ServerConfiguration(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(IdleTimeout);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }

    public class RequestControllerConfiguration
    {
        public const string Route = "RequestController/";

        public class Request
        {
            public const string Hello           = "Hello";
            public const string Register        = "Register";
            public const string Login           = "Login";
            public const string UpdateAccount   = "UpdateAccount";
            public const string Logout          = "Logout";
        }
        
        public class AdminRequest
        {
            public const string CreateAdmin     = "CreateAdmin";
        }
    }
}
