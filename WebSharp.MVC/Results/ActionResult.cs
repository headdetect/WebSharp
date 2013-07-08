using System;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.MVC
{
    public abstract class ActionResult
    {
        public abstract void HandleRequest(IRequest request, IResponse response);

        public static implicit operator ActionResult(string a)
        {
            return new StringResult(a);
        }
    }
}

