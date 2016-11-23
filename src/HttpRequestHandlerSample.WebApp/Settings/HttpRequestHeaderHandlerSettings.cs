using HttpRequestHandlerSample.WebApp.Middlewares;

namespace HttpRequestHandlerSample.WebApp.Settings
{
    /// <summary>
    /// This represents the settings entity for the HTTP request header handler.
    /// </summary>
    public class HttpRequestHeaderHandlerSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="HttpRequestHeaderHandlerMiddlewareOptions"/> object.
        /// </summary>
        public HttpRequestHeaderHandlerMiddlewareOptions Options { get; set; }
    }
}