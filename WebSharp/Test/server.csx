using System.IO;
using System.Threading;
using WebSharp.Routing;
using WebSharp.Handlers;

// Compile Razor templates
RazorHelper.CompileAllViews("Test/views");

var httpd = new HttpServer();
var router = new HttpRouter();
httpd.Request = (request, response) =>
{
    Console.WriteLine("Serving {0}", request.Uri.LocalPath);
    router.Route(request, response);
};

// Set routes
router.AddRoute(new RegexRoute("/", (request, response) =>
{
    var writer = new StreamWriter(response.Body);
    writer.WriteLine("Index page!");
    writer.Flush();
}));
router.AddRoute(new RegexRoute("/greet/(?<name>[A-Za-z]+)", (context, request, response) =>
{
    var writer = new StreamWriter(response.Body);
    writer.WriteLine("Hello, " + context["name"]);
    writer.Flush();
}));

var content = new StaticContentHandler("Test/static");
router.AddRoute(new RegexRoute("/static/(?<path>[A-Za-z0-9_/\\.-]+)", (c, req, res) => content.Serve(c["path"], req, res)));

var razor = new RazorHandler();
router.AddRoute(new RegexRoute("/example/(?<name>[A-Za-z]*)", (c, req, res) => razor.Serve(req, res, "example", new { Name = c["name"] })));

router.MissingRoute = (request, response) =>
{
    response.StatusCode = 404;
    var writer = new StreamWriter(response.Body);
    writer.WriteLine("404: Not Found (custom page)");
    writer.Flush();
};

httpd.Start(new IPEndPoint(IPAddress.Any, 8080));

Console.WriteLine("Press 'Ctrl+C' to exit.");
while (true) Thread.Yield();