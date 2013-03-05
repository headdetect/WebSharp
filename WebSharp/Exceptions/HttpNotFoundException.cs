using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
    public class HttpNotFoundException : Exception
    {
        public HttpNotFoundException() : base()
        {
        }

        public HttpNotFoundException(string message) : base(message)
        {
        }
    }
}
