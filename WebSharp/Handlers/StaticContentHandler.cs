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
            if (!File.Exists(Path.Combine(BaseDirectory, request.Uri.LocalPath)))
                throw new HttpNotFoundException("The requested static content was not found.");
            response.Body = File.OpenRead(Path.Combine(BaseDirectory, request.Uri.LocalPath));
            response.ContentType = HttpServer.GetContentTypeForExtension(Path.GetExtension(request.Uri.LocalPath));
        }

        public void Serve(string path, IRequest request, IResponse response)
        {
            var parts = request.Uri.LocalPath.Split(Path.DirectorySeparatorChar);
            if (parts.Any(p => p == ".."))
                throw new HttpForbiddenException("Disallowed characters in path");
            if (!File.Exists(Path.Combine(BaseDirectory, path)))
                throw new HttpNotFoundException("The requested static content was not found.");
            response.Body = File.OpenRead(Path.Combine(BaseDirectory, path));
            response.ContentType = HttpServer.GetContentTypeForExtension(Path.GetExtension(path).Substring(1));
        }
    }
}
