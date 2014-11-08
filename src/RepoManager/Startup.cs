using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Mvc;

namespace RepoManager
{
    public class Startup
    {
        IConfiguration Configuration;

        public Startup()
        {
            Configuration = new Configuration()
                .AddIniFile("configuration.ini")
                .AddEnvironmentVariables();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
    }
}
