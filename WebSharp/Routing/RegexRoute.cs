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
        public delegate void RegexRouteHandler(RegexRouteContext route, IRequest request, IResponse response);
        public RegexRouteHandler Handler;

        public Regex Expression { get; set; }

        public RegexRoute(string expression, HttpRouter.GenericRouteHandler handler) : this(expression, (a, b, c) => handler(b, c)) { }

        public RegexRoute(Regex expression, HttpRouter.GenericRouteHandler handler) : this(expression, (a, b, c) => handler(b, c)) { }

        public RegexRoute(string expression, RegexRouteHandler handler) : this(new Regex("^" + expression + "$", RegexOptions.Compiled), handler) { }

        public RegexRoute(Regex expression, RegexRouteHandler handler)
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
