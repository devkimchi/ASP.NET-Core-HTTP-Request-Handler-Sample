using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HttpRequestHandlerSample.WebApp.Middlewares
{
    /// <summary>
    /// This represents the middleware entity for HTTP request headers.
    /// </summary>
    public class HttpRequestHeaderHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpRequestHeaderHandlerMiddlewareOptions _options;
        private readonly List<string> _headersToExclude;
        private readonly List<string> _methodsRequireContent;

        /// <summary>
        /// Initialises a new instance of the <see cref="HttpRequestHeaderHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/> delegate.</param>
        /// <param name="options"><see cref="IOptions{HttpRequestHeaderHandlerMiddlewareOptions}"/> instance.</param>
        public HttpRequestHeaderHandlerMiddleware(RequestDelegate next, IOptions<HttpRequestHeaderHandlerMiddlewareOptions> options)
        {
            this._next = next;
            this._options = options.Value;

            this._headersToExclude = new List<string>() { "Content-Length", "Content-Type", "HeaderHost" };
            this._methodsRequireContent = new List<string>() { "POST", "PUT", "PATCH" };
        }

        /// <summary>
        /// Processes the request header handling.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/> instance.</param>
        public async Task Invoke(HttpContext context)
        {
            string url;
            if (!this.IsHandleable(context, out url))
            {
                await this._next.Invoke(context).ConfigureAwait(false);
                return;
            }

            await this.ProcessRequestAsync(url, context).ConfigureAwait(false);
        }

        private static string BuildUri(string url, HttpContext context)
        {
            var path = context.Request.Path.ToString().TrimStart('/');
            var querystring = string.IsNullOrWhiteSpace(context.Request.QueryString.ToString())
                                  ? string.Empty
                                  : $"?{context.Request.QueryString}";

            var uri = $"{url.TrimEnd('/')}/{path}{querystring}";

            return uri;
        }

        private bool IsHandleable(HttpContext context, out string url)
        {
            var path = context.Request.Path.ToString().Trim('/');

            url = null;
            var isHandleable = false;

            foreach (var header in this._options.Headers)
            {
                var prefixed = !header.Prefixes.Any() || header.Prefixes
                                                               .Select(p => p.TrimStart('/'))
                                                               .Any(p => path.StartsWith(p, StringComparison.CurrentCultureIgnoreCase));
                var suffixed = !header.Suffixes.Any() || header.Suffixes
                                                               .Select(p => p.TrimEnd('/'))
                                                               .Any(p => path.EndsWith(p, StringComparison.CurrentCultureIgnoreCase));

                if (!prefixed || !suffixed)
                {
                    continue;
                }

                url = header.Url;
                isHandleable = true;
            }

            return isHandleable;
        }

        private async Task ProcessRequestAsync(string url, HttpContext context)
        {
            var uri = BuildUri(url, context);

            using (var client = new HttpClient())
            {
                foreach (var requestHeader in context.Request.Headers.Where(p => !this._headersToExclude.Contains(p.Key)))
                {
                    client.DefaultRequestHeaders.Add(requestHeader.Key, requestHeader.Value.ToArray());
                }

                foreach (var headerItem in this._options.Headers.Single(p => p.Url.Equals(url, StringComparison.CurrentCultureIgnoreCase)).Headers)
                {
                    client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                }

                var result = await this.SendAsync(uri, client, context).ConfigureAwait(false);

                context.Response.StatusCode = (int)result.StatusCode;
                if (result.Content.Headers.ContentType != null)
                {
                    context.Response.ContentType = result.Content.Headers.ContentType.MediaType;
                }

                var response = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                await context.Response.WriteAsync(response, Encoding.UTF8).ConfigureAwait(false);
            }
        }

        private async Task<HttpResponseMessage> SendAsync(string uri, HttpClient client, HttpContext context)
        {
            using (var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), uri))
            {
                StreamContent content = null;
                if (this.RequireContent(context))
                {
                    content = new StreamContent(context.Request.Body)
                              {
                                  Headers = { ContentType = new MediaTypeHeaderValue(context.Request.ContentType) }
                              };

                    request.Content = content;
                }

                var response = await client.SendAsync(request).ConfigureAwait(false);

                content?.Dispose();

                return response;
            }
        }

        private bool RequireContent(HttpContext context)
        {
            var method = context.Request.Method;
            var required = this._methodsRequireContent.Exists(p => p.Equals(method, StringComparison.CurrentCultureIgnoreCase));

            return required;
        }
    }
}