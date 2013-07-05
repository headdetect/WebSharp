using System;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.MVC
{
    public abstract class Controller
    {
        public IRequest Request { get; set; }
        public IResponse Response { get; set; }
        public string Name { get; set; }

        public Controller()
        {
            Name = GetType().Name;
            if (Name.EndsWith("Controller"))
                Name = Name.Remove(Name.Length - "Controller".Length);
        }

        public ViewResult View(string view, object model = null)
        {
            return new ViewResult(view, model);
        }
    }
}