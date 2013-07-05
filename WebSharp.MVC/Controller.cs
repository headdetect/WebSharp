using System;

namespace WebSharp.MVC
{
    public class Controller
    {
        public Controller()
        {
        }

        public ViewResult View(string view, object model = null)
        {
            return new ViewResult(view, model);
        }
    }
}

