using System;
using Griffin.Networking.Protocol.Http.Protocol;
using Newtonsoft.Json;
using System.IO;

namespace WebSharp.MVC
{
    public class JsonResult : ActionResult
    {
        public object Value { get; set; }
        public Formatting Formatting { get; set; }

        public JsonResult(IRequest request, IResponse response, object value, Formatting formatting = Formatting.None)
            : base(request, response)
        {
            Value = value;
            Formatting = formatting;
        }

        public override string Render(object model = null)
        {
            Response.ContentType = "application/json";
            return JsonConvert.SerializeObject(Value);
        }
    }
}

