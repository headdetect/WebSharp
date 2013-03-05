# WebSharp

The idea behind WebSharp is to provide a lightweight solution for web development in C#,
using Microsoft Roslyn and RazorEngine. The goal is to create something comparible to
node.js, but using C# with the power of .NET.

    var httpd = new HttpServer();
    httpd.Request += (request, response) =>
    {
        var stream = new StreamWriter(response.Stream);
        stream.Write("Hello, world!");
        stream.Close();
    };
    httpd.AddPrefix("http://+:8080");
    httpd.Start();
    
    Console.WriteLine("Press 'Ctrl+C' to exit.");
    while (true);

WebSharp is very young, and is still a work in progress. Ideas and feedback are immensely
valuable, please drop them off as GitHub issues.

Mono support is planned. Believe me, no one wants it more than I do. I'll get to it soon.