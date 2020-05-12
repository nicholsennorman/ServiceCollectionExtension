using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtension.Configuration.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCollectionExtension.Configuration
{
    public static class InMemoryCacheExtension
    {
        public static CustomServiceBuilder UseInMemoryCache(this CustomServiceBuilder serviceBuilder)
        {
            // Check dependent services
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
        /// <param name="clone">If returned objects should be cloned</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            return services.AddSingleton<ICache, InMemoryCache>();
        }
    }
}
