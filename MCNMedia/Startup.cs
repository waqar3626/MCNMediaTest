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
using Microsoft.AspNetCore.Localization;
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
            services.Configure<StripeSetting>(Configuration.GetSection("Stripe"));
            services.AddControllersWithViews();
            //services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time 
                //options.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential = true;
            });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-PK");
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCronJob<CronJobEveryMinute>(c =>
              {
                  c.TimeZoneInfo = TimeZoneInfo.Local;
                  c.CronExpression = @"* * * * *";
              });

            services.AddCronJob<CronJobEveryFiveMinute>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"*/5 * * * *";
            });

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
            app.UseRequestLocalization();
            app.UseRouting();
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthorization();

            // app.UseStaticFiles(new StaticFileOptions
            // {
            //     FileProvider = new PhysicalFileProvider(
            //Path.Combine(env.ContentRootPath, "Uploads")),
            //     RequestPath = "/Uploads"
            // });

            app.UseEndpoints(endpoints =>
            {
                // Website
                endpoints.MapControllerRoute(
                    name: "Home",
                    pattern: "Home",
                    defaults: new { controller = "Website", Action = "Home" });
                endpoints.MapControllerRoute(
                    name: "churches",
                    pattern: "churches",
                    defaults: new { controller = "Website", Action = "Churches" });
                endpoints.MapControllerRoute(
                    name: "Cathedrals",
                    pattern: "Cathedrals",
                    defaults: new { controller = "Website", Action = "Cathedrals" });
                endpoints.MapControllerRoute(
                    name: "FuneralHomes",
                    pattern: "FuneralHomes",
                    defaults: new { controller = "Website", Action = "FuneralHomes" });
                endpoints.MapControllerRoute(
                    name: "Schedules",
                    pattern: "Schedules",
                    defaults: new { controller = "Website", Action = "Schedules" });
                endpoints.MapControllerRoute(
                    name: "ContactUs",
                    pattern: "ContactUs",
                    defaults: new { controller = "Website", Action = "ContactUs" });
                endpoints.MapControllerRoute(
                    name: "TermsAndCondition",
                    pattern: "Terms",
                    defaults: new { controller = "Website", Action = "Terms" });
                endpoints.MapControllerRoute(
                    name: "PrivacyPolicy",
                    pattern: "Privacy",
                    defaults: new { controller = "Website", Action = "Privacy" });
                endpoints.MapControllerRoute(
                    name: "UserLogin",
                    pattern: "UserLogin",
                    defaults: new { controller = "UserLogin", Action = "UserLogin" });
                endpoints.MapControllerRoute(
                    name: "Publish",
                    pattern: "Publish/Recording",
                    defaults: new { controller = "Recording", Action = "PublishEvent" });
                endpoints.MapControllerRoute(
                    name: "Profile",
                    pattern: "Camera/{id?}",
                    defaults: new { controller = "Website", Action = "Profile" });
                //Admin
                endpoints.MapControllerRoute(
                    name: "DashBoard",
                    pattern: "Admin/DashBoard",
                    defaults: new { controller = "DashBoard", Action = "DashBoard" });
                endpoints.MapControllerRoute(
                    name: "AdminChurchDetails",
                    pattern: "Admin/Church/{id?}",
                    defaults: new { controller = "Church", Action = "ChurchDetails" });
                endpoints.MapControllerRoute(
                    name: "AdminChurches",
                    pattern: "Admin/Churches",
                    defaults: new { controller = "Church", Action = "Listchurch" });
                endpoints.MapControllerRoute(
                    name: "AddChurch",
                    pattern: "Admin/Churches/Add",
                    defaults: new { controller = "Church", Action = "AddChurch" });
                endpoints.MapControllerRoute(
                    name: "AdminChurchPreview",
                    pattern: "Admin/Preview/{id?}",
                    defaults: new { controller = "Preview", Action = "Preview" });
                endpoints.MapControllerRoute(
                    name: "Users",
                    pattern: "Admin/Users",
                    defaults: new { controller = "User", Action = "ListUser" });
                endpoints.MapControllerRoute(
                    name: "AdminSchedules",
                    pattern: "Admin/Schedule",
                    defaults: new { controller = "Schedule", Action = "ListSchedule" });

                endpoints.MapControllerRoute(
                    name: "AdminRecordings",
                    pattern: "Admin/Recordings",
                    defaults: new { controller = "Recording", Action = "ListRecording" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Website}/{action=Home}/{id?}",
                defaults: new { controller = "Website", Action = "Home" });
            });
        }
    }
}
