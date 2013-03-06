using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.Mvc
{
    public abstract class ActionResult
    {
        public abstract void HandleRequest(IRequest request, IResponse response);
    }
}
