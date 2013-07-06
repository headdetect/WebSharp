using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpRequestUriTooLongException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 414; }
	    }

	    public HttpRequestUriTooLongException() : base()
	    {
	    }

	    public HttpRequestUriTooLongException(string message) : base(message)
	    {
	    }
	}
}
