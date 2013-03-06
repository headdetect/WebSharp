using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpConflictException : HttpException
    {
        public override int StatusCode
        {
            get { return 409; }
        }

        public HttpConflictException() : base()
        {
        }

        public HttpConflictException(string message) : base(message)
        {
        }
    }
}
