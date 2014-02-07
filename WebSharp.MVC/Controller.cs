using System;
using Griffin.Networking.Protocol.Http.Implementation;
using Griffin.Networking.Protocol.Http.Protocol;
using System.Dynamic;
using System.Diagnostics;
using WebSharp.Exceptions;
using WebSharp.MVC.Results;

namespace WebSharp.MVC
{
    public abstract class Controller
    {
        public IRequest Request { get; set; }
        public IResponse Response { get; set; }
        public dynamic ViewBag { get; set; }
        public string Name { get; set; }

        protected Controller()
        {
            Name = GetType().Name;
            if (Name.EndsWith("Controller"))
                Name = Name.Remove(Name.Length - "Controller".Length);
        }


        /// <summary>
        /// Renders the view
        /// </summary>
        /// <param name="template">The template to render.</param>
        /// <param name="model">The model to pass to the template.</param>
        /// <param name="status">The status of the view.</param>
        /// <param name="resolveExact">if set to <c>true</c>, path will be resolved by calling object; else it will be resolved automatically.</param>
        /// <returns>The result of the action</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public ActionResult View(string template = null, object model = null, HttpStatusCode status = HttpStatusCode.OK)
        {
            if (template == null)
                template = new StackFrame(1, true).GetMethod().Name + ".cshtml";

            if (Response != null)
            {
                Response.StatusCode = (int) status;
                Response.StatusDescription = status.ToString();
            }

            if(CanRender(status))
                return new ViewResult(template, this, model);

            //TODO: Exceptions. Lots of exceptions.

            throw new InvalidOperationException(string.Format("Unable to render a body for status code {0}", status));
        }

        public ActionResult Json(object data, HttpStatusCode status = HttpStatusCode.OK)
        {
            if (Response != null)
            {
                Response.StatusCode = (int)status;
                Response.StatusDescription = status.ToString();
            }

            if (CanRender(status))
                return new JsonResult(data);

            //TODO: Exceptions. Lots of exceptions.

            throw new InvalidOperationException(string.Format("Unable to render a body for status code {0}", status));
            
        }

        public ActionResult Text(string str = "", HttpStatusCode status = HttpStatusCode.OK)
        {
            if (Response != null)
            {
                Response.StatusCode = (int)status;
                Response.StatusDescription = status.ToString();
            }

            if (CanRender(status))
                return str;

            //TODO: Exceptions. Lots of exceptions.

            throw new InvalidOperationException(string.Format("Unable to render a body for status code {0}", status));

        }

        public RedirectResult Redirect(string location)
        {
            if (Response != null)
            {
                Uri uri = new Uri(Request.Uri, location);

                Response.Redirect(uri.AbsoluteUri);
                Response.KeepAlive = false;
                Response.StatusCode = (int)HttpStatusCode.MovedTemp;
                Response.StatusDescription = HttpStatusCode.MovedTemp.ToString();
            }

            return new RedirectResult(location);
        }

        public RedirectResult RedirectPermanent(string location)
        {
            if (Response != null)
            {
                Uri uri = new Uri(Request.Uri, location);

                Response.Redirect(uri.AbsoluteUri);
                Response.KeepAlive = false;
                Response.StatusCode = (int)HttpStatusCode.MovedPerm;
                Response.StatusDescription = HttpStatusCode.MovedPerm.ToString();
            }

            return new RedirectResult(location);
        }


        protected static bool CanRender(HttpStatusCode code)
        {
            //TODO: add all the rest of the status codes

            return code != HttpStatusCode.MovedTemp &&
                   code != HttpStatusCode.MovedPerm;
        }

    }
}