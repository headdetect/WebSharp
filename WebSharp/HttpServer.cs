using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Griffin.Networking.Messaging;
using Griffin.Networking.Protocol.Http;
using Griffin.Networking.Protocol.Http.Protocol;
using HttpException = WebSharp.Exceptions.HttpException;
using Griffin.Networking.Protocol.Http.Services.BodyDecoders;

namespace WebSharp
{
    public class HttpServer
    {
        internal MessagingServer Server { get; set; }

        public delegate void RequestHandler(IRequest request, IResponse response);
        public RequestHandler Request;
        public bool LogRequests { get; set; }

        public HttpServer()
        {
            Server = new MessagingServer(new HttpServiceWrappper.ServiceFactory(ListenerCallback),
                new MessagingServerConfiguration(new HttpMessageFactory()));
            LogRequests = false;
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
                    if (request.Method == "POST" && !string.IsNullOrEmpty(request.ContentType))
                    {
                        var decoder = new CompositeBodyDecoder();
                        decoder.Decode(request);
                    }
                    if (LogRequests)
                        Log(request);
                    Request(request, response);
                    if (LogRequests)
                        Log(response);
                }
                catch (Exception e)
                {
                    HandleException(e, response);
                    if (LogRequests)
                        Console.Write(e);
                }
            }
            response.Body.Position = 0;
            return response;
        }

        static void Log(IRequest request)
        {
            Console.WriteLine(request.Method + " " + request.Uri + " [" + request.RemoteEndPoint + "]");
            foreach (var header in request.Headers)
                Console.WriteLine(" " + header.Name + ": " + header.Value);
        }

        static void Log(IResponse response)
        {
            Console.WriteLine("Response: " + response.StatusCode + ": " + response.StatusDescription);
            foreach (var header in response.Headers)
                Console.WriteLine(" " + header.Name + ": " + header.Value);
        }

        private void HandleException(Exception e, IResponse response)
        {
            // TODO: Custom exception handlers
            response.ContentType = "text/plain";
            var writer = new StreamWriter(response.Body);
            writer.Write("An unhandled exception occured while processing this request: " +
                Environment.NewLine + e.ToString());
            writer.Flush();
            if (e is HttpException)
                response.StatusCode = (e as HttpException).StatusCode;
            else
                response.StatusCode = 500;
        }

        static HttpServer()
        {
            ContentTypes = new Dictionary<string, string>();
            SetContentType("png", "image/png");
            SetContentType("jpeg", "image/jpeg");
            SetContentType("txt", "text/plain");
            SetContentType("js", "text/javascript");
            SetContentType("css", "text/css");
            SetContentType("html", "text/html");
            SetContentType("mkv", "video/x-matroska");
            SetContentType("mp4", "video/mp4");
            SetContentType("ogv", "video/ogg");
            SetContentType("webm", "video/webm");
            SetContentType("acc", "audio/acc");
            SetContentType("mp3", "audio/mpeg");
            SetContentType("oga", "audio/ogg");
            SetContentType("ogg", "audio/ogg");
        }

        public static void SetContentType(string extension, string type)
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
