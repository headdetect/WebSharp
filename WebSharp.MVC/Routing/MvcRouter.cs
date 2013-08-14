using System;
using System.IO;
using System.Linq;
using WebSharp.MVC.Results;
using WebSharp.Routing;
using Griffin.Networking.Protocol.Http.Protocol;
using System.Collections.Generic;
using WebSharp.Exceptions;
using System.Reflection;
using System.Dynamic;

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
            Controllers = new List<Controller>();
            CaseInsensitive = true;
        }

        public void AddRoute(string name, string route, object defaults = null)
        {
            Routes.Add(new MvcRoute(name, route, defaults));
        }

        public void RegisterController(Controller controller)
        {
            if (Controllers.IndexOf(controller) == -1)
                Controllers.Add(controller);
        }

        public bool Match(IRequest request)
        {
            Dictionary<string, string> values = null;
            var route = Routes.FirstOrDefault(r =>
            {
                var keyPair = r.Match(request.Uri.LocalPath, CaseInsensitive);
                if (keyPair == null) return false;
                values = keyPair;
                return true;
            });
            if (route == null)
                return false;
            if (!values.ContainsKey("controller") && !values.ContainsKey("action"))
                return false;
            var controller = Controllers.SingleOrDefault(c => c.Name.Equals(values["controller"],
                CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            if (controller == null)
                return false;
            object[] parameters;
            var action = ResolveAction(controller, request, values, out parameters);
            return action != null;
        }

        MethodInfo ResolveAction(Controller controller, IRequest request, Dictionary<string, string> values, out object[] actionParameters)
        {
            // TODO: ActionName attribute
            var methods = controller.GetType().GetMethods().Where(m =>
                m.IsPublic && m.Name.Equals(values["action"], CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture) &&
                typeof(ActionResult).IsAssignableFrom(m.ReturnType));
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                actionParameters = new object[parameters.Length];
                int matches = 0;
                foreach (var parameter in parameters)
                {
                    var name = parameter.Name;
                    if (!CaseInsensitive)
                    {
                        if (values.ContainsKey(name))
                        {
                            try
                            {
                                actionParameters[matches] = Convert.ChangeType(values[name], parameter.ParameterType);
                            }
                            catch
                            {
                                matches = -1;
                                break;
                            }
                            matches++;
                            continue;
                        }
                    }
                    else
                    {
                        if (values.ContainsKey(name.ToUpper()))
                        {
                            try
                            {
                                actionParameters[matches] = Convert.ChangeType(values[name.ToUpper()], parameter.ParameterType);
                            }
                            catch
                            {
                                matches = -1;
                                break;
                            }
                            matches++;
                            continue;
                        }
                    }
                    if (request.QueryString.Any(q => q.Name.Equals(name, CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)))
                    {
                        try
                        {
                            actionParameters[matches] = Convert.ChangeType(request.QueryString.First(
                                q => q.Name.Equals(name, CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)).Value,
                                parameter.ParameterType);
                        }
                        catch
                        {
                            matches = -1;
                            break;
                        }
                        matches++;
                    }
                    if (request.Method == "POST" && request.Form != null)
                    {
                        if (request.Form.Any(q => q.Name.Equals(name, CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)))
                        {
                            try
                            {
                                var value = request.Form.First(
                                    q => q.Name.Equals(name, CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)).Value;
                                if (parameter.ParameterType == typeof(bool))
                                {
                                    if (value.ToUpper() == "ON" || value.ToUpper() == "OFF")
                                        value = value.ToUpper() == "ON" ? "true" : "false";
                                }
                                actionParameters[matches] = Convert.ChangeType(value, parameter.ParameterType);
                            }
                            catch
                            {
                                matches = -1;
                                break;
                            }
                            matches++;
                        }
                    }
                }
                if (matches == parameters.Length)
                    return method;
            }
            actionParameters = new object[0];
            return null;
        }

        public void Execute(IRequest request, IResponse response)
        {
            Dictionary<string, string> values = null;
            var route = Routes.FirstOrDefault(r =>
            {
                var keyPair = r.Match(request.Uri.LocalPath, CaseInsensitive);
                if (keyPair == null) return false;
                values = keyPair;
                return true;
            });
            if (route == null)
                throw new HttpNotFoundException("Specified controller was not found.");
            if (!values.ContainsKey("controller") && !values.ContainsKey("action"))
                throw new HttpNotFoundException("Specified controller was not found.");
            var controller = Controllers.SingleOrDefault(c => c.Name.Equals(values["controller"],
                CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            if (controller == null)
                throw new HttpNotFoundException("Specified controller was not found.");
            object[] parameters;
            var action = ResolveAction(controller, request, values, out parameters);
            controller.Request = request; controller.Response = response;
            controller.ViewBag = new DynamicViewBag();
            var result = (ActionResult)action.Invoke(controller, parameters);

            if (result != null)
                result.HandleRequest(request, response);

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
                if (localPath.StartsWith("/"))
                    localPath = localPath.Substring(1);

                var parts = localPath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                var routeParts = Route.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

                Dictionary<string, string> values = new Dictionary<string, string>();

                // If localPath is not '/' or some variation ex '////////'
                if (parts.Length != 0 || routeParts.Length != 0)
                {
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
                                if (Defaults.ContainsKey(key) && Defaults[key].Equals(parts[i]))
                                {
                                    if (!caseInsensitive)
                                        values[key] = parts[i];
                                    else
                                        values[key] = parts[i].ToUpper();
                                }
                                else
                                    return null;

                            }
                        }
                        else
                        {
                            if (i >= parts.Length || parts[i] != RouteParts[i])
                                return null;
                        }
                    }
                }
                foreach (var item in Defaults.Where(item => !values.ContainsKey(item.Key)))
                {
                    values.Add(item.Key, Convert.ToString(item.Value));
                }
                return values;
            }
        }
    }
}

