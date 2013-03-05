using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpForbiddenException : Exception
    {
        public HttpForbiddenException() : base()
        {
        }

        public HttpForbiddenException(string message) : base(message)
        {
        }
    }
}
