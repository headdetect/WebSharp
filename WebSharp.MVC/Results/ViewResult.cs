using System;
using Griffin.Networking.Protocol.Http.Protocol;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Configuration;

namespace WebSharp.MVC
{
    public class ViewResult : ActionResult
    {
        /// <summary>
        /// When set to true, views will be compiled and cached. This will improve performance, but live updates will require an
        /// application restart.
        /// </summary>
        public static bool CompileViews = false;

        public string View { get; set; }
        public object Model { get; set; }

        public ViewResult(string view, object model = null)
        {
        }

        public override void HandleRequest(IRequest request, IResponse response)
        {
            response.ContentType = "text/html";
            var writer = new StreamWriter(response.Body);
            var path = ResolveView(View);
            string result;

            if (CompileViews)
            {
                if (Razor.Resolve(path) == null)
                    Razor.Compile(File.ReadAllText(path), path);
                result = Razor.Run(path);
            }
            else
                result = Razor.Parse(path);

            writer.Write(result);
            writer.Flush();
        }

        private string ResolveView(string view)
        {
            return view;
        }
    }
}

