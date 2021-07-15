using bzUploaderAndNlog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NLog;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bzUploaderAndNlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSyncfusionBlazor();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region 宣告 NLog 要使用到的變數內容
            var logRootPath = Configuration["CustomNLog:LogRootPath"];
            var allLogMessagesFilename = Configuration["CustomNLog:AllLogMessagesFilename"];
            var allWebDetailsLogMessagesFilename = Configuration["CustomNLog:AllWebDetailsLogMessagesFilename"];
            LogManager.Configuration.Variables["LogRootPath"] = logRootPath;
            LogManager.Configuration.Variables["AllLogMessagesFilename"] = allLogMessagesFilename;
            LogManager.Configuration.Variables["AllWebDetailsLogMessagesFilename"] = allWebDetailsLogMessagesFilename;
            #endregion

            #region Syncfusion License Registration
            string key = Configuration["Syncfusion:License:19"];
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            #region 靜態檔案服務
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "temp")),
                RequestPath = "/temp"
            });
            #endregion

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
