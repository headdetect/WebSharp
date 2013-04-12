# WebSharp

The idea behind WebSharp is to provide a lightweight solution for web development in C#, using
Microsoft Roslyn and RazorEngine for rapid web development. You can have a website up and running
in moments with WebSharp. It's built for customization - you control the web server, no more IIS.
If you're more comfortable with ASP.NET MVC, well, there's an MVC-esque framework waiting inside
if you want to go that route. Otherwise, there's a wide variety of ways to structure your website,
and WebSharp gives you the power to forge your own website design.

```csharp
var httpd = new HttpServer();
httpd.Request = (request, response) =>
{
    var stream = new StreamWriter(response.Body);
    stream.Write("Hello, world!");
    stream.Flush();
};
httpd.Start(new IPEndPoint(IPAddress.Loopback, 8080));

Console.WriteLine("Press 'Ctrl+C' to exit.");
while (true) System.Threading.Thread.Yield();
```

Say this file is saved to server.csx. You can run it with `WebSharp.Roslyn.exe server.csx`.

Here's another example of a simple server that only serves static content:

```csharp
// Defines a simple static content handler that serves content from the working directory
var content = new StaticContentHandler(Directory.GetCurrentDirectory());

var httpd = new HttpServer();
httpd.Request = content.Serve;
httpd.Start(new IPEndPoint(IPAddress.Any, 8080));
```

WebSharp is very young, and is still a work in progress. Ideas and feedback are immensely
valuable, please drop them off as GitHub issues. It is **extremely** likely that the structure
of the project and most of the API will change frequently in the early stages.

Mono support is planned. Believe me, no one wants it more than I do. I'll get to it soon.

## How it all works

WebSharp expects you to manage your own web server in the code, and as a result, it's extremely
customizable. Most of the time, though, your site will look something like this:

* A `HttpRouter` object for directing requests through your code
* A number of `IRouteMapper`s that direct requests to a number of "handlers"
* Several handlers, both custom and framework-provided, to handle requests

For instance, you might have several `RegexRoute`s that call `RazorHandler`s for displaying Razor
views, or they could call some `StaticContentHandler`s for serving static files, or they could
just call your own delegate handler that deals with it yourself.

If you choose to go with a system similar to ASP.NET MVC, you'll probably end up with several
`MvcControllerRoute`s that point to controller actions, which render and display Razor views. You
can also take out the "C" in MVC and just have handlers that wire directly into views, or any other
configuration you like. WebSharp lets you use any piece of the framework seperately from the rest.

## A more complex example

```csharp
using System.IO;
using System.Threading;
using WebSharp.Routing;
using WebSharp.Handlers;

var httpd = new HttpServer();
var router = new HttpRouter();
// We could also use httpd.Request = router.Route;
// Instead, we want to log activity to the console.
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
// This maps /greet/*, and says "Hello, *" on the page.
// Note the RegexRouteContext parameter in the lambda, this allows you to access
// named groups in the regex.
router.AddRoute(new RegexRoute("/greet/(?<name>[A-Za-z]+)", (context, request, response) =>
{
	var writer = new StreamWriter(response.Body);
	writer.WriteLine("Hello, " + context["name"]);
	writer.Flush();
}));
// We can also serve static content. This one serves up [working directory]/Test/static
// There are mime types defined WebSharp.HttpServer. You can add your own (or override
// existing ones) with HttpServer.SetContentType.
var content = new StaticContentHandler("Test/static");
router.AddRoute(new RegexRoute("/static/(?<path>[A-Za-z0-9_/\\.-]+)",
    (context, req, res) => content.Serve(context["path"], req, res)));
// A 404 handler on the router. You can also just go through httpd.RequestException and
// handle HttpNotFoundException.
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
```

## Dependencies

Lots of great dependencies make this project possible. Some are pulled from Nuget, others
are just submodules.

* [Roslyn](http://msdn.microsoft.com/en-us/vstudio/roslyn.aspx) for script-like C# stuff
* [RazorEngine](https://github.com/Antaris/RazorEngine) for razor templates
* [Json.NET](http://json.codeplex.com/) because JSON is awesome
* [Griffin.Networking](https://github.com/jgauffin/griffin.networking) because System.Net.HttpListener is horrible

## Building from Source

You'll .NET 4.5 installed. As such, the earliest Visual Studio version you can edit it with is 2012. You can also
use SharpDevelop, MonoDevelop, or just msbuild.