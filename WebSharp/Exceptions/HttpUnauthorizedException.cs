using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpUnauthorizedException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 401; }
	    }

	    public HttpUnauthorizedException() : base()
	    {
	    }

	    public HttpUnauthorizedException(string message) : base(message)
	    {
	    }
	}
}
