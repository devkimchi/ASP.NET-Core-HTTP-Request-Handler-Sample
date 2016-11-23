using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace HttpRequestHandlerSample.WebApp.Middlewares
{
    /// <summary>
    /// This represents the extension entity for the <see cref="HttpRequestHeaderHandlerMiddleware"/> class.
    /// </summary>
    public static class HttpRequestHeaderHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Registers the middleware into the HTTP request pipeline.
        /// </summary>
        /// <param name="builder"><see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="action"><see cref="Action{HttpRequestHeaderHandlerMiddlewareOptions}"/> delegate.</param>
        /// <returns>Returns the <see cref="IApplicationBuilder"/> instance that contains the middleware.</returns>
        public static IApplicationBuilder UseHttpRequestHeaderHandler(this IApplicationBuilder builder, Action<HttpRequestHeaderHandlerMiddlewareOptions> action = null)
        {
            if (builder == null)
            {
                return null;
            }

            HttpRequestHeaderHandlerMiddlewareOptions options;
            if (action == null)
            {
                options = new HttpRequestHeaderHandlerMiddlewareOptions()
                          {
                              Headers =
                              {
                                  new HttpRequestHeader()
                                  {
                                      Url = "http://localhost",
                                      Prefixes = { "/api" }
                                  }
                              }
                          };

                return UseHttpRequestHeaderHandler(builder, options);
            }

            options = new HttpRequestHeaderHandlerMiddlewareOptions();
            action.Invoke(options);

            return UseHttpRequestHeaderHandler(builder, options);
        }

        /// <summary>
        /// Registers the middleware into the HTTP request pipeline.
        /// </summary>
        /// <param name="builder"><see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="options"><see cref="HttpRequestHeaderHandlerMiddlewareOptions"/> instance.</param>
        /// <returns>Returns the <see cref="IApplicationBuilder"/> instance that contains the middleware.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseHttpRequestHeaderHandler(this IApplicationBuilder builder, HttpRequestHeaderHandlerMiddlewareOptions options)
        {
            if (builder == null)
            {
                return builder;
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.UseMiddleware<HttpRequestHeaderHandlerMiddleware>(Options.Create(options));
        }
    }
}