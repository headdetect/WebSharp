using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpExpectationFailedException : HttpException
    {
        public override int StatusCode
        {
            get { return 417; }
        }

        public HttpExpectationFailedException() : base()
        {
        }

        public HttpExpectationFailedException(string message) : base(message)
        {
        }
    }
}
