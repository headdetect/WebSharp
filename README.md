# WebSharp

WebSharp is a .NET web framework that helps you make websites quickly and easily, while giving you complete
control over the stack. It's highly customizable and very lightweight, and it runs great on Windows, Linux,
and Mac.

## Features

* Built-in web server using Griffin.Networking
* Includes optional handlers for:
  * MVC-style site design
  * Static content
  * Routing based on regular expressions
* Razor templating
* JSON serialization

Each part of WebSharp operates almost entirely independently of the rest, and you can use just what you need,
or even provide your own code to handle different parts of the framework.

## Example

Here's some example code to run a simple website:

```csharp
var httpd = new HttpServer();
var router = new HttpRouter();
// Registers the router with the HttpServer
httpd.Request = router.Route; 
// Note that they operate independently of each other, and you could easily
// use your own router if you wanted to, or plug our router into your own
// web server.

// Says "Hello, user!" where "user" comes from GET /user
router.AddRoute(new RegexRoute("/(?<name>[A-Za-z ]+)/?", (context, request, response) =>
{
    var writer = new StreamWriter(response.Body);
    // You can use context["group"] to grab named groups out of regex routes
    writer.Write("Hello, " + context["name"] + "!");
    writer.Flush();
    response.ContentType = "text/plain";
}));

// WebSharp also includes static content
var staticContent = new StaticContentHandler(
    Path.Combine(Directory.GetCurrentDirectory(), "content"));
// This static route helps the static content coorperate with other routes. It'll only
// match on routes that go to files the static content handler is aware of. This way,
// you could have "/style.css" resolve to static content, and "/edit" resolve to
// something else, under the same "/" root directory.
var staticRoute = new StaticContentRoute(staticContent);
router.AddRoute(staticRoute);

httpd.Start(new IPEndPoint(IPAddress.Any, 80));
```

## Contributing

Please feel free to fork WebSharp and submit pull requests with your changes. Adhere to coding styles
already in use.

## Compiling

WebSharp compiles with Visual Studio, MonoDevelop, xbuild, msbuild, SharpDevelop, etc. Requires .NET 4.5.
