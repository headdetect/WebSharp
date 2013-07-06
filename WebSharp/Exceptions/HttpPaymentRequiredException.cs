using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpPaymentRequiredException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 402; }
	    }

	    public HttpPaymentRequiredException() : base()
	    {
	    }

	    public HttpPaymentRequiredException(string message) : base(message)
	    {
	    }
	}
}
