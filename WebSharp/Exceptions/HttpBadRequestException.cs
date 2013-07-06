using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp.Exceptions
{
	public class HttpBadRequestException : HttpException
	{
	    public override int StatusCode
	    {
	        get { return 400; }
	    }

	    public HttpBadRequestException() : base()
	    {
	    }

	    public HttpBadRequestException(string message) : base(message)
	    {
	    }
	}
}
