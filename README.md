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
valuable, please drop them off as GitHub issues.

Mono support is planned. Believe me, no one wants it more than I do. I'll get to it soon.

## Dependencies

Lots of great dependencies make this project possible. Some are pulled from Nuget, others
are just submodules.

* [Roslyn](http://msdn.microsoft.com/en-us/vstudio/roslyn.aspx) for script-like C# stuff
* [RazorEngine](https://github.com/Antaris/RazorEngine) for razor templates
* [Json.NET](http://json.codeplex.com/) because JSON is awesome
* [Griffin.Networking](https://github.com/jgauffin/griffin.networking) because System.Net.HttpListener is horrible