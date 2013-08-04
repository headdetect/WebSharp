using System;
using Griffin.Networking.Protocol.Http.Implementation;
using Griffin.Networking.Protocol.Http.Protocol;
using System.Dynamic;
using System.Diagnostics;
using WebSharp.Exceptions;

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


        public ActionResult View(string template, object model = null, HttpStatusCodes status = HttpStatusCodes.Ok)
        {
            if (Response != null)
                Response.StatusCode = (int)status;

            if(CanRender(status))
                return new ViewResult(this, template, model);

            //TODO: Exceptions. Lots of exceptions.

            return new StringResult(Request, Response, "Not really sure what to do here");
        }

        public ActionResult Json(object data, HttpStatusCodes status = HttpStatusCodes.Ok)
        {
            if (Response != null)
                Response.StatusCode = (int)status;

            if (CanRender(status))
                return new JsonResult(Request, Response, data);

            //TODO: Exceptions. Lots of exceptions.

            return new StringResult(Request, Response, "Not really sure what to do here");
            
        }

        public ActionResult Text(string str, HttpStatusCodes status = HttpStatusCodes.Ok)
        {
            if (Response != null)
                Response.StatusCode = (int)status;

            if (CanRender(status))
                return new StringResult(Request, Response, str);

            //TODO: Exceptions. Lots of exceptions.

            return new StringResult(Request, Response, "Not really sure what to do here");
            
        }


        protected static bool CanRender(HttpStatusCodes code)
        {
            //TODO: add all the rest of the status codes

            return code != HttpStatusCodes.MovedTemp &&
                   code != HttpStatusCodes.MovedPerm;
        }

    }
}