using System;
using System.Linq;
using System.Dynamic;
using Griffin.Networking.Protocol.Http.Protocol;
using System.IO;
using Xipton.Razor;
using System.Reflection;
using Xipton.Razor.Config;
using System.Web.Razor;
using WebSharp.Exceptions;
using Xipton.Razor.Core.ContentProvider;

namespace WebSharp.MVC
{
    public class ViewResult : ActionResult
    {
        static ViewResult()
        {
            // Set defaults //
            ViewBase = "Views";

            using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebSharp.MVC.DefaultRazorConfig.xml")))
                Razor = new RazorMachine(stream.ReadToEnd());
        }

        public static void RegisterLayout(string path)
        {

        }

        public static string ViewBase { get; set; }
        protected static RazorMachine Razor { get; set; }

        public string View { get; set; }
        public object Model { get; set; }
        public Controller Controller { get; set; }

        public ViewResult(string view, Controller controller, object model = null)
        {
            View = view;
            Model = model;
            Controller = controller;
        }

        public override void HandleRequest(IRequest request, IResponse response)
        {
            response.ContentType = "text/html";
            var writer = new StreamWriter(response.Body);
            var path = ResolveView(View);
            if (path == null)
                throw new HttpNotFoundException(string.Format("Requested view not found. Looking for: {0}", Path.Combine(Controller.Name, View)));
            string result;

            result = (string)Razor.ExecuteUrl(path, Model, Controller.ViewBag, false, true).ToString();

            writer.Write(result);
            writer.Flush();
        }

        private string ResolveView(string view)
        {
            if (File.Exists(Path.Combine(".", ViewBase, Controller.Name, view)))
                return Path.Combine(Controller.Name, view);
            return null;
        }
    }
}

