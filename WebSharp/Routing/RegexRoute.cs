using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.Routing
{
    public class RegexRoute : IRouteMapper
    {
        public delegate void RouteHandler(RegexRouteContext route, IRequest request, IResponse response);
        public RouteHandler Handler;

        public Regex Expression { get; set; }

        public RegexRoute(string expression, RouteHandler handler) : this(new Regex("^" + expression + "$"), handler) { }

        public RegexRoute(Regex expression, RouteHandler handler)
        {
            Handler = handler;
            Expression = expression;
        }

        public bool Match(IRequest request)
        {
            return Expression.IsMatch(request.Uri.LocalPath);
        }

        public void Execute(IRequest request, IResponse response)
        {
            Handler(new RegexRouteContext(request.Uri.LocalPath, Expression), request, response);
        }

        public class RegexRouteContext
        {
            private Match match { get; set; }

            public RegexRouteContext(string path, Regex regex)
            {
                match = regex.Match(path);
            }

            public string this[string key]
            {
                get
                {
                    return match.Groups[key].Value;
                }
            }
        }
    }
}
