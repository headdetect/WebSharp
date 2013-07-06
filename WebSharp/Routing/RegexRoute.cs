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
	    public Dictionary<string, string> DefaultGroups { get; set; }

	    public Regex Expression { get; set; }

	    public RegexRoute(string expression, HttpRouter.GenericRouteHandler handler) : this(expression, (a, b, c) => handler(b, c)) { }

	    public RegexRoute(string expression, HttpRouter.GenericRouteHandler handler, object defaults) : this(expression, (a, b, c) => handler(b, c), defaults) { }

	    public RegexRoute(Regex expression, HttpRouter.GenericRouteHandler handler) : this(expression, (a, b, c) => handler(b, c)) { }

	    public RegexRoute(Regex expression, HttpRouter.GenericRouteHandler handler, object defaults) : this(expression, (a, b, c) => handler(b, c), defaults) { }

	    public RegexRoute(string expression, RegexRouteHandler handler) : this(new Regex("^" + expression + "/?$", RegexOptions.Compiled), handler, null) { }

	    public RegexRoute(string expression, RegexRouteHandler handler, object defaults) : this(new Regex("^" + expression + "/?$", RegexOptions.Compiled), handler, defaults) { }

	    public RegexRoute(Regex expression, RegexRouteHandler handler) : this(expression, handler, null) { }

	    public RegexRoute(Regex expression, RegexRouteHandler handler, object defaults)
	    {
	        Handler = handler;
	        Expression = expression;
	        DefaultGroups = new Dictionary<string, string>();
	        if (defaults != null)
	        {
	            var properties = defaults.GetType().GetProperties();
	            foreach (var prop in properties)
	                DefaultGroups[prop.Name] = Convert.ToString(prop.GetValue(defaults, null));
	        }
	    }

	    public bool Match(IRequest request)
	    {
	        return Expression.IsMatch(request.Uri.LocalPath);
	    }

	    public void Execute(IRequest request, IResponse response)
	    {
	        Handler(new RegexRouteContext(request.Uri.LocalPath, Expression, DefaultGroups), request, response);
	    }

	    public class RegexRouteContext
	    {
	        public Regex Regex { get; set; }

	        private Match match { get; set; }
	        private Dictionary<string, string> DefaultGroups { get; set; }

	        public RegexRouteContext(string path, Regex regex, Dictionary<string, string> defaultGroups)
	        {
	            Regex = regex;
	            match = Regex.Match(path);
	            DefaultGroups = defaultGroups;
	        }

	        public string this[string key]
	        {
	            get
	            {
	                var group = match.Groups[key];
	                if (group.Success)
	                    return group.Value;
	                else
	                {
	                    if (DefaultGroups.ContainsKey(key))
	                        return DefaultGroups[key];
	                    throw new KeyNotFoundException();
	                }
	            }
	        }
	    }
	}
}
