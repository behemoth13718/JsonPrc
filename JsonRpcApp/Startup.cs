using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SphaeraJsonRpc.Extensions;
using SphaeraJsonRpc.Protocol;

namespace JsonRpcApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddJsonRpc();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "1",
                        Title = "",
                        Description = $"Сервис для работы с пространственными объектами.",
                        TermsOfService = null,
                        Contact = new OpenApiContact { Name = "АО «СФЕРА»", Email = "info@sphaera.ru" },
                        License = new OpenApiLicense { Name = "GNU GPL v3.0", Url = new Uri("https://choosealicense.com/licenses/gpl-3.0/") }
                    });
            });

            services.AddJsonRpcClient<IRpcService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            //
            app.UseJsonRpc<RpcServerService>("/rpc1srv");
            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSwagger(x => x.RouteTemplate = "help/{documentName}/swagger.json");
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = "help";
                x.SwaggerEndpoint("v1/swagger.json", $"");
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}