using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.Mvc
{
    public abstract class Controller
    {
        public IRequest Request { get; set; }
        public IResponse Response { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public ViewResult View(object model)
        {
            var view = new StackFrame(1, true).GetMethod().Name;
            return new ViewResult(view, model);
        }

        public ViewResult View(string view)
        {
            return new ViewResult(view);
        }

        public ViewResult View(string view, object model)
        {
            return new ViewResult(view, model);
        }
    }
}
