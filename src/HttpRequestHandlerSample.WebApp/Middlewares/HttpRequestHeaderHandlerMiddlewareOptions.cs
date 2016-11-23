using System.Collections.Generic;

namespace HttpRequestHandlerSample.WebApp.Middlewares
{
    /// <summary>
    /// This represents the options entity for the <see cref="HttpRequestHeaderHandlerMiddleware"/> class.
    /// </summary>
    public class HttpRequestHeaderHandlerMiddlewareOptions
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HttpRequestHeaderHandlerMiddlewareOptions"/> class.
        /// </summary>
        public HttpRequestHeaderHandlerMiddlewareOptions()
        {
            this.Headers = new List<HttpRequestHeader>();
        }

        /// <summary>
        /// Gets or sets the list of<see cref= "HttpRequestHeader" /> objects.
        /// </summary>
        public List<HttpRequestHeader> Headers { get; set; }
    }

    /// <summary>
    /// This represents the option item entity for HTTP request header.
    /// </summary>
    public class HttpRequestHeader
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HttpRequestHeader"/> class.
        /// </summary>
        public HttpRequestHeader()
        {
            this.Prefixes = new List<string>();
            this.Suffixes = new List<string>();
            this.Headers = new List<HttpRequestHeaderItem>();
        }

        /// <summary>
        /// Gets or sets the URL to send the request.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the list of prefixes within the request paths.
        /// </summary>
        public List<string> Prefixes { get; set; }

        /// <summary>
        /// Gets or sets the list of suffixes within the request paths.
        /// </summary>
        public List<string> Suffixes { get; set; }

        /// <summary>
        /// Gets or sets the list of header key/value pairs.
        /// </summary>
        public List<HttpRequestHeaderItem> Headers { get; set; }
    }

    public class HttpRequestHeaderItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}