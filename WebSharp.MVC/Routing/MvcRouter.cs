using System;
using System.Linq;
using WebSharp.Routing;
using Griffin.Networking.Protocol.Http.Protocol;
using System.Collections.Generic;
using WebSharp.Exceptions;
using System.Reflection;

namespace WebSharp.MVC
{
    public class MvcRouter : IRouteMapper
    {
        public List<MvcRoute> Routes { get; set; }
        public List<Controller> Controllers { get; set; }
        public bool CaseInsensitive { get; set; }

        public MvcRouter()
        {
            Routes = new List<MvcRoute>();
        }

        public void AddRoute(string name, string route, object defaults = null)
        {
            Routes.Add(new MvcRoute(name, route, defaults));
        }

        public void RegisterController(Controller controller)
        {
            Controllers.Add(controller);
        }

        public bool Match(IRequest request)
        {
            Dictionary<string, string> values = null;
            var route = Routes.SingleOrDefault(r => (values = r.Match(request.Uri.LocalPath, CaseInsensitive)) != null);
            if (route == null)
                return false;
            if (!values.ContainsKey("controller") && !values.ContainsKey("action"))
                return false;
            var controller = Controllers.SingleOrDefault(c => c.Name.Equals(values["controller"],
                CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            if (controller == null)
                return false;
            var action = ResolveAction(controller, request, values);
            return action != null;
        }

        MethodInfo ResolveAction(Controller controller, IRequest request, Dictionary<string, string> values)
        {
            // TODO: ActionName attribute
            var methods = controller.GetType().GetMethods().Where(m =>
                m.IsPublic && m.Name.Equals(values ["action"], CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                int matches = 0;
                foreach (var parameter in parameters)
                {
                    var name = parameter.Name;
                    if (CaseInsensitive)
                    {
                        if (values.ContainsKey(name))
                        {
                            matches++;
                            continue;
                        }
                    }
                    else
                    {
                        if (values.ContainsKey(name.ToUpper()))
                        {
                            matches++;
                            continue;
                        }
                    }
                    if (request.QueryString.Any(q => q.Name.Equals(name, CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)))
                        matches++;
                }
                if (matches == parameters.Length)
                    return method;
            }
            return null;
        }

        public void Execute(IRequest request, IResponse response)
        {
        }

        public class MvcRoute
        {
            public string Name { get; set; }
            public string Route
            {
                get
                {
                    return string.Join("/", RouteParts);
                }
                set
                {
                    RouteParts = value.Split('/');
                }
            }
            public Dictionary<string, object> Defaults { get; set; }

            private string[] RouteParts { get; set; }

            public MvcRoute(string name, string route, object defaults)
            {
                Name = name;
                Route = route;
                Defaults = new Dictionary<string, object>();
                var properties = defaults.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.CanRead)
                        Defaults.Add(property.Name, property.GetValue(defaults, null));
                }
            }

            public Dictionary<string, string> Match(string localPath, bool caseInsensitive)
            {
                var parts = localPath.Split('/');
                Dictionary<string, string> values = new Dictionary<string, string>();
                for (int i = 0; i < RouteParts.Length; i++)
                {
                    if (RouteParts[i].StartsWith("{") && RouteParts[i].EndsWith("}"))
                    {
                        var key = RouteParts[i].Substring(1, RouteParts[i].Length - 2);
                        if (i >= parts.Length)
                        {
                            if (Defaults.ContainsKey(key))
                            {
                                if (!caseInsensitive)
                                    values[key] = Defaults[key].ToString();
                                else
                                    values[key] = Defaults[key].ToString().ToUpper();
                            }
                            else
                                return null;
                        }
                        else
                        {
                            if (!caseInsensitive)
                                values[key] = parts[i];
                            else
                                values[key] = parts[i].ToUpper();
                        }
                    }
                    else
                    {
                        if (i >= parts.Length || parts[i] != RouteParts[i])
                            return null;
                    }
                }
                return values;
            }
        }
    }
}

