// Include: ViewModels/*.csx
// Include: Controllers/*.csx
// Include: Views/*.cshtml
using System.Threading;

var httpd = new HttpServer();
var router = new HttpRouter();
ControllerRoute.DiscoverControllers();

router.AddRoute(new ControllerRoute("/{controller}/{action}/{name}"));

httpd.Request = router.Route;
httpd.Start(new IPEndPoint(IPAddress.Loopback, 8080));

Console.WriteLine("Press 'Ctrl+C' to exit.");
while (true) Thread.Yield();