using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public abstract class HttpException : Exception
	{
	    public abstract int StatusCode { get; }

	    public HttpException()
	    {
	    }

	    public HttpException(string message) : base(message)
	    {
	    }
	}
}
