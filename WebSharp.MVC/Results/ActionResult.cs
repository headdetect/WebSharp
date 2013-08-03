using System;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.MVC
{
    public abstract class ActionResult
    {
        protected IRequest Request { get; set; }
        protected IResponse Response { get; set; }

        protected ActionResult(IRequest request, IResponse response)
        {
            Request = request;
            Response = response;
        }

        public abstract string Render(object model = null);

    }
}

