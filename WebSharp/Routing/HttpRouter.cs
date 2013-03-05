using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;
using WebSharp.Exceptions;

namespace WebSharp.Routing
{
    public class HttpRouter
    {
        public delegate void GenericRouteHandler(IRequest request, IResponse response);
        public GenericRouteHandler MissingRoute;

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
                throw new HttpNotFoundException("No suitable route was found to handle " + request.Uri);
        }
    }
}
