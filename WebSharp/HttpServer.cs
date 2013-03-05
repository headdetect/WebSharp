using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Messaging;
using Griffin.Networking.Protocol.Http;
using Griffin.Networking.Protocol.Http.Protocol;
using WebSharp.Exceptions;

namespace WebSharp
{
    public class HttpServer
    {
        internal MessagingServer Server { get; set; }

        public delegate void RequestHandler(IRequest request, IResponse response); // TODO: Use our own wrappers
        public RequestHandler Request;

        public HttpServer()
        {
            Server = new MessagingServer(new HttpServiceWrappper.ServiceFactory(ListenerCallback),
                new MessagingServerConfiguration(new HttpMessageFactory()));
        }

        public void Start(IPEndPoint endPoint)
        {
            Server.Start(endPoint);
        }

        private IResponse ListenerCallback(IRequest request)
        {
            var response = request.CreateResponse(HttpStatusCode.OK, "OK");
            response.Body = new MemoryStream();
            if (Request == null)
            {
                response.ContentType = "text/plain";
                var writer = new StreamWriter(response.Body);
                writer.WriteLine("No request handler is registered with this HttpServer instance. Set HttpServer.Request to a RequestHandler delegate.");
                writer.Flush();
            }
            else
            {
                try
                {
                    Request(request, response);
                }
                catch (Exception e)
                {
                    HandleException(e, response);
                }
            }
            response.Body.Position = 0;
            return response;
        }

        private void HandleException(Exception e, IResponse response)
        {
            // TODO: Custom exception handlers
            response.ContentType = "text/plain";
            var writer = new StreamWriter(response.Body);
            writer.Write("An unhandled exception occured while processing this request: " +
                Environment.NewLine + e.ToString());
            writer.Flush();
            if (e is HttpNotFoundException)
                response.StatusCode = 404;
            else
                response.StatusCode = 400;
        }

        static HttpServer()
        {
            ContentTypes = new Dictionary<string, string>();
            AddContentType("png", "image/png");
            AddContentType("jpeg", "image/jpeg");
            AddContentType("txt", "text/plain");
            AddContentType("js", "text/javascript");
            AddContentType("css", "text/css");
            AddContentType("html", "text/html");
        }

        public static void AddContentType(string extension, string type)
        {
            ContentTypes[extension.ToUpper()] = type;
        }

        private static Dictionary<string, string> ContentTypes { get; set; }
        public static string GetContentTypeForExtension(string extension)
        {
            if (!ContentTypes.ContainsKey(extension.ToUpper()))
                return null; // TODO: Should we do this?
            return ContentTypes[extension.ToUpper()];
        }
    }
}
