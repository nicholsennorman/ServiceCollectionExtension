using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtension.Configuration.Cache;
using System;
using System.Linq;

namespace ServiceCollectionExtension.Configuration
{
    public static class InMemoryCacheExtension
    {
        public static CustomServiceBuilder AddInMemoryCache(this CustomServiceBuilder serviceBuilder)
        {

            // Check dependent services
            //https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1
            //For most apps, IMemoryCache is enabled. For example, calling AddMvc, AddControllersWithViews, AddRazorPages, AddMvcCore().AddRazorViewEngine, and many other Add{Service} methods in ConfigureServices, enables IMemoryCache. 
            //For apps that are not calling one of the preceding Add{Service} methods, it may be necessary to call AddMemoryCache in ConfigureServices before calling this method.
            //In this sample project, it has called AddRazorPages() in Startup.cs. Hence, it will have the IMemoryCache registered already.
            //Do not put any IMemoryCache service registration here to prevent tightly bound of IMemoryCache implementation to any concrete implementation
            if (!serviceBuilder.Services.Any(s => s.ServiceType == typeof(IMemoryCache)))
            {
                throw new NotSupportedException("You need to register a IMemoryCache service in order to use Memory Cache");
            }
            serviceBuilder.Services.AddInMemoryCache();

            return serviceBuilder;
        }

        /// <summary>
        /// Adds the memory cache service for repository caching.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            return services.AddSingleton<ICache, InMemoryCache>();
        }
    }
}
