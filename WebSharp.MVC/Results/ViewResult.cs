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

        public ViewResult(Controller controller, string view, object model = null)
            : base(controller.Request, controller.Response)
        {
            View = view;
            Model = model;
            Controller = controller;
        }

        public override string Render(object model = null)
        {
            Response.ContentType = "text/html";
            var path = ResolveView(View);
            if (path == null)
                throw new HttpNotFoundException(string.Format("Requested view not found. Looking for: {0}", Path.Combine(".", ViewBase, View)));

            return (string)Razor.ExecuteUrl(path.Replace('\\', '/'), Model, Controller.ViewBag, false, true).ToString();

        }

        private string ResolveView(string view)
        {
            return File.Exists(Path.Combine(".", ViewBase, view)) ? view : null;
        }
    }
}

