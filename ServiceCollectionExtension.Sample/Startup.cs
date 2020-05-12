using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceCollectionExtension.Configuration;
using ServiceCollectionExtension.Configuration.Cache;

namespace ServiceCollectionExtension.Sample
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
            //Calling AddRazorPages() will register IMemoryCache concrete service implementation. 
            //Hence, there is no need to call services.AddMemoryCache(); separately.
            services.AddRazorPages();

            #region redis
            //Download Redis from https://github.com/ServiceStack/redis-windows and extract it.
            //Run redis-server.exe redis.windows.conf from the extracted folder location.
            //Uncomment below code if need to use Redis instead of in memory cache
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = "localhost";
            //    options.InstanceName = "RedisInstance";
            //});
            #endregion redis

            services.AddCustomService(options =>
            {
                options.UseInMemoryCache();
                //Uncomment below code if need to use Redis instead of in memory cache
                //options.UseDistributedCache();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICache cache)
        {
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

            app.UseRouting();

            app.UseAuthorization();

            cache.Set<DateTime>("DateTime", DateTime.Now);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
