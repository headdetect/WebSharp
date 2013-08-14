using Griffin.Networking.Protocol.Http.Protocol;
using Newtonsoft.Json;
using System.IO;

namespace WebSharp.MVC.Results
{
    public class JsonResult : ActionResult
    {
        public object Value { get; set; }
        public Formatting Formatting { get; set; }

        public JsonResult(object value, Formatting formatting = Formatting.None)
        {
            Value = value;
            Formatting = formatting;
        }

        public override void HandleRequest(IRequest request, IResponse response)
        {
            var writer = new StreamWriter(response.Body);
            response.ContentType = "application/json";
            writer.Write(JsonConvert.SerializeObject(Value));
            writer.Flush();
        }
    }
}