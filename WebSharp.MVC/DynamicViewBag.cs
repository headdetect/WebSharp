using System;
using System.Linq;
using System.Dynamic;
using System.Collections.Generic;

namespace WebSharp.MVC
{
	public class DynamicViewBag : DynamicObject
	{
	    private Dictionary<string, object> Values { get; set; }

	    public DynamicViewBag()
	    {
	        Values = new Dictionary<string, object>();
	    }

	    public override IEnumerable<string> GetDynamicMemberNames()
	    {
	        return Values.Keys;
	    }

	    public override bool TrySetMember(SetMemberBinder binder, object value)
	    {
	        Values[binder.Name] = value;
	        return true;
	    }

	    public override bool TryGetMember(GetMemberBinder binder, out object result)
	    {
	        if (!Values.ContainsKey(binder.Name))
	            result = null;
	        else
	            result = Values[binder.Name];
	        return true;
	    }
	}
}

