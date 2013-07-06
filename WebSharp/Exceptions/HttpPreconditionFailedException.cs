using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpPreconditionFailedException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 412; }
	    }

	    public HttpPreconditionFailedException() : base()
	    {
	    }

	    public HttpPreconditionFailedException(string message) : base(message)
	    {
	    }
	}
}
