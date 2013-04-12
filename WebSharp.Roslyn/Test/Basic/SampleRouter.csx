class SampleRouter : IRouteMapper
{
    public bool Match(IRequest request)
    {
        return request.Uri.LocalPath == "/foobar";
    }
    
    public void Execute(IRequest request, IResponse response)
    {
        var writer = new StreamWriter(response.Body);
        writer.Write("Custom routing!");
        writer.Flush();
    }
}