using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;
using WebSharp.Exceptions;
using WebSharp.Routing;

namespace WebSharp.Mvc
{
    // TODO: Maybe WebSharp.Mvc should be a seperate project
    public class ControllerRoute : IRouteMapper
    {
        static ControllerRoute()
        {
            ControllerTypes = new List<Type>();
        }

        public static void RegisterController(Type type)
        {
            if (ControllerTypes.Contains(type))
                return;
            if (!typeof(Controller).IsAssignableFrom(type))
                throw new InvalidOperationException("Controller types must derive from WebSharp.Mvc.MvcController.");
            ControllerTypes.Add(type);
        }

        public static void DiscoverControllers()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => typeof(Controller).IsAssignableFrom(t) && !t.IsAbstract &&
                    t.GetConstructors().Any(c => c.GetParameters().Length == 0 && c.IsPublic));
                foreach (var type in types)
                    RegisterController(type);
            }
        }

        public static List<Type> ControllerTypes { get; set; }
        private const string PathPartRegex = "(?<$1>[A-Za-z0-9_-]+)";

        public ControllerRoute(string route)
        {
            // TODO: Improve this whole class
            route = new Regex("{([A-Za-z_][A-Za-z0-9_]*)}").Replace(route, PathPartRegex);
            Route = new Regex("^" + route + "/?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            DefaultValues = new Dictionary<string, object>();
        }

        public ControllerRoute(string route, object defaults) : this(route)
        {
            var properties = defaults.GetType().GetProperties();
            foreach (var prop in properties)
                DefaultValues[prop.Name] = prop.GetValue(defaults);
        }

        public Regex Route { get; set; }
        public Dictionary<string, object> DefaultValues { get; set; }

        public bool Match(IRequest request)
        {
            return Route.IsMatch(request.Uri.LocalPath);
        }

        public void Execute(IRequest request, IResponse response)
        {
            var match = Route.Match(request.Uri.LocalPath);
            var controllerType = ControllerTypes.FirstOrDefault(
                t => t.Name.ToUpper() == match.Groups["controller"].Value.ToUpper() ||
                     t.Name.ToUpper() == match.Groups["controller"].Value.ToUpper() + "CONTROLLER"); // TODO: Allow users to specify case insensitivity
            if (controllerType == null)
                throw new HttpNotFoundException();
            var actions = controllerType.GetMethods().Where(m => m.IsPublic && m.Name.ToUpper() == match.Groups["action"].Value.ToUpper() &&
                m.GetParameters().Length == match.Groups.Count - 3 && typeof(ActionResult).IsAssignableFrom(m.ReturnType));
            if (!actions.Any())
                throw new HttpNotFoundException();
            // Verify parameters
            var groups = Route.GetGroupNames().Where(g => g != "controller" && g != "action" && g != "0");
            foreach (var action in actions)
            {
                var parameters = action.GetParameters();
                object[] values = new object[parameters.Length];
                bool foundAction = true;
                foreach (var parameter in parameters)
                {
                    bool foundMatch = false;
                    foreach (var group in groups)
                    {
                        if (group == parameter.Name)
                        {
                            values[parameter.Position] = Convert.ChangeType(match.Groups[group].Value, parameter.ParameterType);
                            foundMatch = true;
                            break;
                        }
                    }
                    if (!foundMatch)
                    {
                        foundAction = false;
                        break;
                    }
                }
                if (foundAction)
                {
                    var controller = (Controller)Activator.CreateInstance(controllerType);
                    controller.Request = request;
                    controller.Response = response;
                    var result = (ActionResult)action.Invoke(controller, values);
                    result.HandleRequest(request, response);
                    return;
                }
            }
        }
    }
}
