using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCollectionExtension.Configuration.Cache
{
    public static class DistributedCacheExtension
    {
        public static CustomServiceBuilder UseDistributedCache(this CustomServiceBuilder serviceBuilder, bool clone = false)
        {
            // Check dependent services
            if (!serviceBuilder.Services.Any(s => s.ServiceType == typeof(IDistributedCache)))
            {
                throw new NotSupportedException("You need to register a IDistributedCache service in order to use Distributed Cache");
            }
            serviceBuilder.Services.AddDistributedCache();

            return serviceBuilder;
        }

        /// <summary>
        /// Adds the memory cache service for repository caching.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <param name="clone">If returned objects should be cloned</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddDistributedCache(this IServiceCollection services)
        {
            return services.AddSingleton<ICache, DistributedCache>();
        }
    }
}
