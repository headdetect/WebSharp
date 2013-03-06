class ExampleController : Controller
{
	public ActionResult Index(string name)
	{
		return View(new { Name = name });
	}
}