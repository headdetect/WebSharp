using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpUnsupportedMediaTypeException : HttpException
    {
        public override int StatusCode
        {
            get { return 415; }
        }

        public HttpUnsupportedMediaTypeException() : base()
        {
        }

        public HttpUnsupportedMediaTypeException(string message) : base(message)
        {
        }
    }
}
