# WebSharp

The idea behind WebSharp is to provide a lightweight solution for web development in C#,
using Microsoft Roslyn and RazorEngine. The goal is to create something comparible to
node.js, but using C# with the power of .NET.

    var httpd = new HttpServer();
    httpd.Request = (request, response) =>
    {
        response.ContentType = "text/plain";
        var stream = new StreamWriter(response.Body);
        stream.Write("Hello, world!");
        stream.Flush();
    };
    httpd.Start(new IPEndPoint(IPAddress.Loopback, 8080));

    Console.WriteLine("Press 'Ctrl+C' to exit.");
    while (true);

Say this file is saved to server.cs. You can run it with `WebSharp server.cs`.

WebSharp is very young, and is still a work in progress. Ideas and feedback are immensely
valuable, please drop them off as GitHub issues. It is **extremely** likely that the structure
of the project and most of the API will change frequently in the early stages.

Mono support is planned. Believe me, no one wants it more than I do. I'll get to it soon.

## A more complex example

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

## Dependencies

Lots of great dependencies make this project possible. Some are pulled from Nuget, others
are just submodules.

* [Roslyn](http://msdn.microsoft.com/en-us/vstudio/roslyn.aspx) for script-like C# stuff
* [RazorEngine](https://github.com/Antaris/RazorEngine) for razor templates
* [Json.NET](http://json.codeplex.com/) because JSON is awesome
* [Griffin.Networking](https://github.com/jgauffin/griffin.networking) because System.Net.HttpListener is horrible