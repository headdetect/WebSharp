using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpGoneException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 410; }
	    }

	    public HttpGoneException() : base()
	    {
	    }

	    public HttpGoneException(string message) : base(message)
	    {
	    }
	}
}
