using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpForbiddenException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 403; }
	    }

	    public HttpForbiddenException() : base()
	    {
	    }

	    public HttpForbiddenException(string message) : base(message)
	    {
	    }
	}
}
