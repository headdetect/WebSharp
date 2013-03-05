var httpd = new HttpServer();
httpd.Request = (request, response) =>
{
    response.ContentType = "text/plain";
    var stream = new StreamWriter(response.Body);
    stream.Write("Hello, {0}!", request.Uri.LocalPath);
    stream.Flush();
};
httpd.Start(new IPEndPoint(IPAddress.Loopback, 8080));

Console.WriteLine("Press 'Ctrl+C' to exit.");
while (true);