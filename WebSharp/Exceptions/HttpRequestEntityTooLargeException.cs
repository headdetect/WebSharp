using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpRequestEntityTooLargeException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 413; }
	    }

	    public HttpRequestEntityTooLargeException() : base()
	    {
	    }

	    public HttpRequestEntityTooLargeException(string message) : base(message)
	    {
	    }
	}
}
