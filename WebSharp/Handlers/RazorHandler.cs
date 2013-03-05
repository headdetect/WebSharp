using System;
using System.IO;
using Griffin.Networking.Protocol.Http.Protocol;
using RazorEngine;
using RazorEngine.Templating;

namespace WebSharp.Handlers
{
    /// <summary>
    /// Handles requests by serving pages with Razor.
    /// </summary>
    public class RazorHandler
    {
        public void Serve(IRequest request, IResponse response, string template, object model)
        {
            response.ContentType = "text/html";
            string result = Razor.Run(template, model);
            var writer = new StreamWriter(response.Body);
            writer.WriteLine(result);
            writer.Flush();
        }
    }
}
