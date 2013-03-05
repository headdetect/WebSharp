using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.Routing
{
    public class HttpRouter
    {
        public delegate void MissingRouteHandler(IRequest request, IResponse response);
        public MissingRouteHandler MissingRoute;

        public List<IRouteMapper> Routes { get; set; }

        public HttpRouter()
        {
            Routes = new List<IRouteMapper>();
        }

        public void AddRoute(IRouteMapper route)
        {
            Routes.Add(route);
        }

        public void Route(IRequest request, IResponse response)
        {
            foreach (var route in Routes)
            {
                if (route.Match(request))
                {
                    route.Execute(request, response);
                    return;
                }
            }
            if (MissingRoute != null)
                MissingRoute(request, response);
            else
            {
                var writer = new StreamWriter(response.Body);
                writer.WriteLine("404: Not Found");
                writer.WriteLine("To use a custom 404 page, set HttpRouter.MissingRoute.");
                writer.Flush();
                response.StatusCode = 404;
                response.StatusDescription = "Not Found";
            }
        }
    }
}
