using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpRequestedRangeNotSatisfiableException : HttpException
    {
        public override int StatusCode
        {
            get { return 416; }
        }

        public HttpRequestedRangeNotSatisfiableException() : base()
        {
        }

        public HttpRequestedRangeNotSatisfiableException(string message) : base(message)
        {
        }
    }
}
