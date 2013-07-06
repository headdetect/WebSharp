using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpNotAcceptableException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 406; }
	    }

	    public HttpNotAcceptableException() : base()
	    {
	    }

	    public HttpNotAcceptableException(string message) : base(message)
	    {
	    }
	}
}
