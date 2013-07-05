using System;
using WebSharp.Routing;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.MVC
{
    public class MvcRouter : IRouteMapper
    {
        public MvcRouter()
        {
        }

        public bool Match(IRequest request)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}

