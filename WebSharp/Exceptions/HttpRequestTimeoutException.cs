using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpRequestTimeoutException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 408; }
	    }

	    public HttpRequestTimeoutException() : base()
	    {
	    }

	    public HttpRequestTimeoutException(string message) : base(message)
	    {
	    }
	}
}
