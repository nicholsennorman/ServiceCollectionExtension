using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ServiceCollectionExtension.Configuration.Cache
{
    public static class DistributedCacheExtension
    {
        public static CustomServiceBuilder AddDistributedCache(this CustomServiceBuilder serviceBuilder)
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
        /// Adds the Distributed cache service for repository caching.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <returns>The updated service collection</returns>
        private static IServiceCollection AddDistributedCache(this IServiceCollection services)
        {
            return services.AddSingleton<ICache, DistributedCache>();
        }
    }
}
