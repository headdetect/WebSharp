using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;
using WebSharp.Exceptions;

namespace WebSharp.Handlers
{
	public class StaticContentHandler
	{
	    public string BaseDirectory { get; set; }

	    public StaticContentHandler(string baseDir)
	    {
	        BaseDirectory = baseDir;
	    }

	    public void Serve(IRequest request, IResponse response)
	    {
	        var parts = request.Uri.LocalPath.Split(Path.DirectorySeparatorChar);
	        if (parts.Any(p => p == ".."))
	            throw new HttpForbiddenException("Disallowed characters in path");
	        var path = request.Uri.LocalPath;
	        if (Path.IsPathRooted(path)) path = path.Substring(path.IndexOf(Path.DirectorySeparatorChar) + 1);
	        if (!File.Exists(Path.Combine(BaseDirectory, path)))
	            throw new HttpNotFoundException("The requested static content was not found.");
	        response.Body = File.OpenRead(Path.Combine(BaseDirectory, path));
	        response.ContentType = HttpServer.GetContentTypeForExtension(Path.GetExtension(request.Uri.LocalPath).Substring(1));
	    }

	    public void Serve(string path, IRequest request, IResponse response)
	    {
	        var parts = request.Uri.LocalPath.Split(Path.DirectorySeparatorChar);
	        if (parts.Any(p => p == ".."))
	            throw new HttpForbiddenException("Disallowed characters in path");
	        if (Path.IsPathRooted(path)) path = path.Substring(path.IndexOf(Path.DirectorySeparatorChar) + 1);
	        if (!File.Exists(Path.Combine(BaseDirectory, path)))
	            throw new HttpNotFoundException("The requested static content was not found.");
	        response.Body = File.OpenRead(Path.Combine(BaseDirectory, path));
	        response.ContentType = HttpServer.GetContentTypeForExtension(Path.GetExtension(path).Substring(1));
	    }

	    public bool CanResolve(IRequest request)
	    {
	        var parts = request.Uri.LocalPath.Split(Path.DirectorySeparatorChar);
	        if (parts.Any(p => p == ".."))
	            return false;
	        var path = request.Uri.LocalPath;
	        if (Path.IsPathRooted(path)) path = path.Substring(path.IndexOf(Path.DirectorySeparatorChar) + 1);
	        if (!File.Exists(Path.Combine(BaseDirectory, path)))
	            return false;
	        return true;
	    }
	}
}
