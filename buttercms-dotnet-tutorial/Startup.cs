using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ButterCMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using buttercms_dotnet_tutorial.Configuration;

namespace buttercms_dotnet_tutorial
{
    public class Startup
    {
        public IWebHostEnvironment HostingEnvironment { get; }

        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Config = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddResponseCaching();
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("7days", new CacheProfile
                {
                    Duration = 604800,
                    Location = ResponseCacheLocation.Any,
                    
                });
                options.CacheProfiles.Add("2days", new CacheProfile
                {
                    Duration = 172800,
                    Location = ResponseCacheLocation.Any,
                    
                });
                options.CacheProfiles.Add("1days", new CacheProfile
                {
                    Duration = 86400,
                    Location = ResponseCacheLocation.Any,
                    
                });
            });

            services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));

            services.AddSingleton<IWebHostEnvironment>(HostingEnvironment);

            services.AddOptions();
            //services.Configure<UrlOptions>(Configuration.GetSection("UrlOptions"));
            //services.Configure<ButterCmsOptions>(Configuration.GetSection("ButterCMSOptions"));

            services.AddScoped<ButterCMSClient>(c =>
                new ButterCMSClient(c.GetRequiredService<IOptions<ButterCmsOptions>>().Value.ApiKey));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
