using System;
using System.Linq;
using HtmlAgilityPack;

namespace WebSharp.MVC
{
    public static class HtmlHelper
    {
        public static void PopulateNode(HtmlNode node, object attributes)
        {
            if (attributes == null)
                return;
            var properties = attributes.GetType().GetProperties().Where(p => p.CanRead);
            foreach (var prop in properties)
                node.Attributes.Add(prop.Name.Replace('_', '-'), Convert.ToString(prop.GetValue(attributes, null)));
        }
    }
}

