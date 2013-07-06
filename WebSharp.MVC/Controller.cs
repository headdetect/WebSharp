using System;
using Griffin.Networking.Protocol.Http.Protocol;
using System.Dynamic;

namespace WebSharp.MVC
{
    public abstract class Controller
    {
        public IRequest Request { get; set; }
        public IResponse Response { get; set; }
        public dynamic ViewBag { get; set; }
        public string Name { get; set; }

        public Controller()
        {
            Name = GetType().Name;
            if (Name.EndsWith("Controller"))
                Name = Name.Remove(Name.Length - "Controller".Length);
        }

        public JsonResult Json(object data)
        {
            return new JsonResult(data);
        }

        public ViewResult View(string view, object model = null)
        {
            return new ViewResult(view, this, model);
        }
    }
}