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
            var path = request.Uri.LocalPath;
            Serve(path, request, response);
        }

        public void Serve(string path, IRequest request, IResponse response)
        {
            if (request.Uri.Segments.Any(p => p == ".."))
                throw new HttpForbiddenException("Disallowed characters in path");
            if (Path.IsPathRooted(path)) path = path.Substring(path.IndexOfAny(new[] { Path.DirectorySeparatorChar, '/', '\\' }) + 1);
            if (!File.Exists(Path.Combine(BaseDirectory, path)))
                throw new HttpNotFoundException("The requested static content was not found.");

            var stream = File.OpenRead(Path.Combine(BaseDirectory, path));
            var extension = Path.GetExtension(path);
            if (extension != null)
                response.ContentType = HttpServer.GetContentTypeForExtension(extension.Substring(1));
            response.AddHeader("Accept-Ranges", "bytes");

            // Handle ranges
            long length = stream.Length;
            if (request.Headers.Any(h => h.Name == "Range"))
            {
                //response.StatusCode = 206; // Breaks things for some unknown reason, quite infuriating
                //response.StatusDescription = "Partial Content";
                var range = request.Headers["Range"].Value;
                var type = range.Remove(range.IndexOf("=", StringComparison.Ordinal));
                if (type != "bytes")
                {
                    response.StatusCode = 400;
                    response.StatusDescription = "Bad Request";
                    return;
                }
                range = range.Substring(range.IndexOf("=", StringComparison.Ordinal) + 1);
                var rangeParts = range.Split('-');
                long start = int.Parse(rangeParts[0]);
                long end;
                if (!long.TryParse(rangeParts[1], out end))
                    end = length;
                length = end - start;
                response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", start, end, length));
                stream.Seek(start, SeekOrigin.Begin);
            }
            //stream._Length = length;
            response.Body = new MemoryStream();
            stream.CopyTo(response.Body);
            stream.Close();
            response.Body.Seek(0, SeekOrigin.Begin);
        }

        public bool CanResolve(IRequest request)
        {
            if (request.Uri.Segments.Any(p => p == ".."))
                return false;
            var path = request.Uri.LocalPath;
            if (Path.IsPathRooted(path)) path = path.Substring(path.IndexOfAny(new[] { Path.DirectorySeparatorChar, '/', '\\' }) + 1);
            string file = Path.Combine(BaseDirectory, path);
            return File.Exists(file);
        }
    }
}
