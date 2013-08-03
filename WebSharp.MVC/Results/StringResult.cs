using System;
using Griffin.Networking.Protocol.Http.Protocol;
using System.IO;

namespace WebSharp.MVC
{
    public class StringResult : ActionResult
    {
        public string Value { get; set; }
        public string ContentType { get; set; }

        public StringResult(IRequest request, IResponse response, string value, string contentType = "text/plain")
            : base(request, response)
        {
            Value = value;
            ContentType = contentType;
        }

        public override string Render(object model = null)
        {
            Response.ContentType = ContentType;
            return Value;
        }
    }
}

