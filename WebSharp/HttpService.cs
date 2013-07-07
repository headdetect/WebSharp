using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Buffers;
using Griffin.Networking.Protocol.Http;
using Griffin.Networking.Protocol.Http.Protocol;
using Griffin.Networking.Servers;

namespace WebSharp
{
    internal class HttpServiceWrappper : HttpService
    {
        public class ServiceFactory : IServiceFactory
        {
            public RequestHandler Request;

            public ServiceFactory(RequestHandler request)
            {
                Request = request;
            }

            public INetworkService CreateClient(EndPoint remoteEndPoint)
            {
                return new HttpServiceWrappper(Request);
            }
        }

        private static readonly BufferSliceStack stack = new BufferSliceStack(50, 32000);

        public delegate IResponse RequestHandler(IRequest request);
        public RequestHandler Request;

        public HttpServiceWrappper(RequestHandler request) : base(stack)
        {
            Request = request;
        }

        public override void Dispose()
        {
        }

        public override void OnRequest(IRequest request)
        {
            Send(Request(request));
        }
    }
}
