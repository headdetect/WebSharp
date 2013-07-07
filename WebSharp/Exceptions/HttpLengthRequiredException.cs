using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpLengthRequiredException : HttpException
    {
        public override int StatusCode
        {
            get { return 411; }
        }

        public HttpLengthRequiredException() : base()
        {
        }

        public HttpLengthRequiredException(string message) : base(message)
        {
        }
    }
}
