using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceCollectionExtension.Configuration
{
    public static class StartupExtension
    {
        public static IServiceCollection AddCustomService(this IServiceCollection services, Action<CustomServiceBuilder> options)
        {
            var serviceBuilder = new CustomServiceBuilder(services);
            options?.Invoke(serviceBuilder);
            return serviceBuilder.Services;
        }
    }
}
