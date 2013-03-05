using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Messaging;
using Griffin.Networking.Protocol.Http;
using Griffin.Networking.Protocol.Http.Protocol;

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
                    // TODO: Custom exception handlers
                    WriteExceptionResponse(e, response);
                }
            }
            response.Body.Position = 0;
            return response;
        }

        private void WriteExceptionResponse(Exception e, IResponse response)
        {
            response.ContentType = "text/plain";
            var writer = new StreamWriter(response.Body);
            writer.Write("An unhandled exception occured while processing this request: " +
                Environment.NewLine + e.ToString());
            writer.Flush();
        }
    }
}
