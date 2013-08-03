using System;
using Griffin.Networking.Protocol.Http.Implementation;
using Griffin.Networking.Protocol.Http.Protocol;
using System.Dynamic;
using System.Diagnostics;

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


        public ActionResult ViewOk(string template, object model = null)
        {
            return new ViewResult(this, template, model);
        }

        public ActionResult JsonOk(object data)
        {
            return new JsonResult(Request, Response, data);
        }

        public ActionResult StringOk(string str)
        {
            return new StringResult(Request, Response, str);
        }

        public ActionResult Ok()
        {
            return StringOk(string.Empty);
        }


        public ActionResult ViewRedirect(string location, string template, object model = null)
        {
            if (Response != null)
            {
                Response.Redirect(location);
            }

            return StringOk(string.Empty);
        }

        public ActionResult JsonRedirect(string location, object data)
        {
            if (Response != null)
            {
                Response.Redirect(location);
            }
            return new JsonResult(Request, Response, data);
        }

        public ActionResult StringRedirect(string location, string str)
        {
            if (Response != null)
            {
                Response.Redirect(location);
            }
            return new StringResult(Request, Response, str);
        }

        public ActionResult Redirect(string location)
        {
            if (Response != null)
            {
                Response.Redirect(location);
            }
            return StringOk(string.Empty);
        }


        public ActionResult ViewBadRequest(string template, object model = null)
        {
            if (Response != null)
            {
                Response.StatusCode = 400;
            }
            return new ViewResult(this, template, model);
        }

        public ActionResult JsonBadRequest(object data)
        {
            if (Response != null)
            {
                Response.StatusCode = 400;
            }
            return new JsonResult(Request, Response, data);
        }

        public ActionResult StringBadRequest(string str)
        {
            if (Response != null)
            {
                Response.StatusCode = 400;
            }
            return new StringResult(Request, Response, str);
        }

        public ActionResult BadRequest()
        {
            if (Response != null)
            {
                Response.StatusCode = 400;
            }
            return StringOk(string.Empty);
        }
    }
}