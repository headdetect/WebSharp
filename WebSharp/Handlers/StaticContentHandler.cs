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
            var parts = request.Uri.LocalPath.Split(Path.DirectorySeparatorChar);
            if (parts.Any(p => p == ".."))
                throw new HttpForbiddenException("Disallowed characters in path");
            if (Path.IsPathRooted(path)) path = path.Substring(path.IndexOf(Path.DirectorySeparatorChar) + 1);
            if (!File.Exists(Path.Combine(BaseDirectory, path)))
                throw new HttpNotFoundException("The requested static content was not found.");
            //var stream = new StreamWrapper(Path.Combine(BaseDirectory, path), FileMode.Open);
            var stream = File.OpenRead(Path.Combine(BaseDirectory, path));
            response.ContentType = HttpServer.GetContentTypeForExtension(Path.GetExtension(path).Substring(1));
            response.AddHeader("Accept-Ranges", "bytes");
            // Handle ranges
            long length = stream.Length;
            if (request.Headers.Any(h => h.Name == "Range"))
            {
                response.StatusCode = 206;
                response.StatusDescription = "Partial Content";
                var range = request.Headers["Range"].Value;
                var type = range.Remove(range.IndexOf("="));
                if (type != "bytes")
                {
                    response.StatusCode = 400;
                    response.StatusDescription = "Bad Request";
                    return;
                }
                range = range.Substring(range.IndexOf("=") + 1);
                var rangeParts = range.Split('-');
                long start = int.Parse(rangeParts[0]);
                long end;
                if (!long.TryParse(rangeParts[1], out end))
                    end = length;
                length = end - start;
                response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", start, end, stream.Length));
                stream.Seek(start, SeekOrigin.Begin);
                response.KeepAlive = false;
            }
            response.ContentLength = (int)length;
            stream.CopyTo(response.Body);
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

        // Used to limit the length of a stream for HTML5 audio/video playback
        private class StreamWrapper : FileStream
        {
            public int _Length { get; set; }

            public override long Length
            {
                get
                {
                    return _Length;
                }
            }

            public StreamWrapper(string path, FileMode mode) : base(path, mode)
            {
            }
        }
    }
}
