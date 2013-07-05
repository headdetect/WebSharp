using System;
using System.Dynamic;
using Griffin.Networking.Protocol.Http.Protocol;
using System.IO;
using Xipton.Razor;
using System.Reflection;
using Xipton.Razor.Config;
using System.Web.Razor;

namespace WebSharp.MVC
{
    public class ViewResult : ActionResult
    {
        static ViewResult()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebSharp.MVC.DefaultRazorConfig.xml");
            Razor = new RazorMachine();
        }

        /// <summary>
        /// When set to true, views will be compiled and cached. This will improve performance, but live updates will require an
        /// application restart.
        /// </summary>
        public static bool CompileViews = false;
        private static RazorMachine Razor { get; set; }

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
                Razor.RegisterTemplate(path, File.ReadAllText(path));
                result = Razor.ExecuteUrl(path, null, null, false, true).ToString();
            }
            else
                result = Razor.Execute(File.ReadAllText(path), Model, new ExpandoObject(), false, true).ToString();

            writer.Write(result);
            writer.Flush();
        }

        private string ResolveView(string view)
        {
            return view;
        }
    }
}

