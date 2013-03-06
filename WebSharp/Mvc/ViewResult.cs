using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;
using RazorEngine;

namespace WebSharp.Mvc
{
    public class ViewResult : ActionResult
    {
        public string View { get; set; }
        public object Model { get; set; }

        public ViewResult(string view)
        {
            View = view;
        }

        public ViewResult(string view, object model) : this(view)
        {
            Model = model;
        }

        public override void HandleRequest(IRequest request, IResponse response)
        {
            response.ContentType = "text/html";
            var writer = new StreamWriter(response.Body);
            writer.Write(Razor.Run(View, Model));
            writer.Flush();
        }
    }
}
