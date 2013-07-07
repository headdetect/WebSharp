using System;
using Griffin.Networking.Protocol.Http.Protocol;
using WebSharp.Handlers;

namespace WebSharp.Routing
{
    public class StaticContentRoute : IRouteMapper
    {
        public StaticContentHandler Handler { get; set; }

        public StaticContentRoute(StaticContentHandler handler)
        {
            Handler = handler;
        }

        public bool Match(IRequest request)
        {
            return Handler.CanResolve(request);
        }

        public void Execute(IRequest request, IResponse response)
        {
            Handler.Serve(request, response);
        }
    }
}

