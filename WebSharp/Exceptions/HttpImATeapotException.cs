using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpImATeapotException : HttpException
    {
        public override int StatusCode
        {
            get { return 418; }
        }

        public HttpImATeapotException() : base()
        {
        }

        public HttpImATeapotException(string message) : base(message)
        {
        }
    }
}
