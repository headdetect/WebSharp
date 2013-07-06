using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpProxyAuthenticationRequiredException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 407; }
	    }

	    public HttpProxyAuthenticationRequiredException() : base()
	    {
	    }

	    public HttpProxyAuthenticationRequiredException(string message) : base(message)
	    {
	    }
	}
}
