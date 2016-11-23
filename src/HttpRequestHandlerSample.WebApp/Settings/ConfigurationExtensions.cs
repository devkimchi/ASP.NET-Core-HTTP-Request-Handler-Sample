using Microsoft.Extensions.Configuration;

namespace HttpRequestHandlerSample.WebApp.Settings
{
    /// <summary>
    /// This represents the extensions entity for the <see cref="IConfiguration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets the deserialised instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="config"><see cref="IConfiguration"/> instance.</param>
        /// <param name="key">Key to look for <c>appsettings.json</c>.</param>
        /// <returns>Returns the deserialised instance of type <typeparamref name="T"/>.</returns>
        public static T Get<T>(this IConfiguration config, string key) where T : new()
        {
            var instance = new T();

            config.GetSection(key).Bind(instance);

            return instance;
        }
    }
}
