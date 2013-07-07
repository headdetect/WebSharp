using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpMethodNotAllowedException : HttpException
    {
        public override int StatusCode
        {
            get { return 405; }
        }

        public HttpMethodNotAllowedException() : base()
        {
        }

        public HttpMethodNotAllowedException(string message) : base(message)
        {
        }
    }
}
