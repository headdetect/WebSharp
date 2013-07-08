using System;
using Griffin.Networking.Protocol.Http.Protocol;
using System.IO;

namespace WebSharp.MVC
{
    public class StringResult : ActionResult
    {
        public string Value { get; set; }
        public string ContentType { get; set; }

        public StringResult(string value, string contentType = "text/plain")
        {
            Value = value;
            ContentType = contentType;
        }

        public override void HandleRequest(IRequest request, IResponse response)
        {
            response.ContentType = ContentType;
            var writer = new StreamWriter(response.Body);
            writer.Write(Value);
            writer.Flush();
        }
    }
}

