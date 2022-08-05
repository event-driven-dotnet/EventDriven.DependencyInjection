using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EventDriven.DependencyInjection
{
    /// <summary>
    /// Helper methods for configuring services with dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register app settings with dependency injection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="config">The application's <see cref="IConfiguration"/>.</param>
        /// <param name="lifetime">Service lifetime.</param>
        /// <typeparam name="TAppSettings">Strongly typed app settings class.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddAppSettings<TAppSettings>(
            this IServiceCollection services, IConfiguration config,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TAppSettings : class
        {
            var settingsName = typeof(TAppSettings).Name;
            var configSection = config.GetSection(settingsName);
            if (!configSection.Exists())
                throw new Exception($"Configuration section '{settingsName}' not present in app settings.");
            services.Configure<TAppSettings>(configSection);
            switch (lifetime)
            {
                case ServiceLifetime.Transient:
                    services.AddTransient(sp =>
                        sp.GetRequiredService<IOptions<TAppSettings>>().Value);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(sp =>
                        sp.GetRequiredService<IOptions<TAppSettings>>().Value);
                    break;
                default:
                    services.AddSingleton(sp =>
                        sp.GetRequiredService<IOptions<TAppSettings>>().Value);
                    break;
            }
            return services;
        }
    }
}