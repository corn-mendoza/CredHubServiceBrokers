using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Security.DataProtection.CredHubCore;
using Swashbuckle.AspNetCore.Swagger;

namespace MSSqlSecureBroker
{
    public class Startup
    {
        ILoggerFactory logFactory;
        public Startup(IConfiguration configuration, ILoggerFactory logFactory)
        {
            Configuration = configuration;
            this.logFactory = logFactory;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbString = Configuration.GetConnectionString("AttendeeContext");

            //services.AddDbContext<AttendeeContext>(options => options.UseSqlServer(dbString));

            services.AddMvc();
            services.AddCredHubClient(Configuration, logFactory);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = ".NET Service Broker for MSSQL", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Service Broker for MSSQL V1");
            });
            app.UseMvc();
        }
    }
}
