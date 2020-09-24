using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCNMedia_Dev.CronJobs;
using MCNMedia_Dev.Models;
using MCNMedia_Dev.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;

namespace MCNMedia_Dev
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
            // For Google and Facebook Authentication
            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.ClientId = "";
            //        options.ClinetSecret = "";

//    })
//      .AddFacebook(options =>
//       {
//           options.AppId = "";
//           options.AppSecret = "";
//       });

#if DEBUG
            services.AddRazorPages().AddRazorRuntimeCompilation();
#endif
            //services.Add(new ServiceDescriptor(typeof(UserDataAccessLayer), new UserDataAccessLayer(Configuration.GetConnectionString("Default"))));
            //services.Add(new ServiceDescriptor(typeof(ChurchDataAccessLayer), new ChurchDataAccessLayer(Configuration.GetConnectionString("Default"))));
            //services.AddTransient<MySqlConnection>(_ => new MySqlConnection(Configuration["ConnectionStrings:Default"]));
            services.AddControllersWithViews();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);//You can set Time   
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCronJob<CronJobEveryMinute>(c =>
              {
                  c.TimeZoneInfo = TimeZoneInfo.Local;
                  c.CronExpression = @"* * * * *";
              });

            //services.AddCronJob<MyCronJob1>(c =>
            //{
            //    c.TimeZoneInfo = TimeZoneInfo.Local;
            //    c.CronExpression = @"*/5 * * * *";
            //});
            //services.AddCronJob<MyCronJob2>(c =>
            //{
            //    c.TimeZoneInfo = TimeZoneInfo.Local;
            //    c.CronExpression = @"* * * * *";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(env.ContentRootPath, "Uploads")),
                RequestPath = "/Uploads"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserLogin}/{action=UserLogin}/{id?}");
            });
        }
    }
}
