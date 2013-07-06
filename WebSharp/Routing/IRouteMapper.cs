using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Griffin.Networking.Protocol.Http.Protocol;

namespace WebSharp.Routing
{
	public interface IRouteMapper
	{
	    bool Match(IRequest request);
	    void Execute(IRequest request, IResponse response);
	}
}
