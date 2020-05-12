using Microsoft.Extensions.DependencyInjection;

namespace ServiceCollectionExtension.Configuration
{
    public class CustomServiceBuilder
    {
        public readonly IServiceCollection Services;

        public CustomServiceBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
