using Griffin.Networking.Protocol.Http.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.MVC.Results
{
    public class RedirectResult : ActionResult
    {
        public string Url { get; set; }

        public RedirectResult(string url)
        {
            Url = url;
        }

        public override void HandleRequest(IRequest request, IResponse response)
        {
            var writer = new StreamWriter(response.Body);
            writer.Write("Location: " + Url);
            writer.Flush();
        }
    }
}
