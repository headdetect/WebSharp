using System;
using Xipton.Razor;
using HtmlAgilityPack;

namespace WebSharp.MVC
{
    public class HtmlEmphasize
    {
        public string Text { get; set; }
        public object HtmlAttributes { get; set; }

        public HtmlEmphasize(string text, object attributes = null)
        {
            Text = text;
            HtmlAttributes = attributes;
        }

        public LiteralString Render()
        {
            var node = HtmlNode.CreateNode("<em>");
            node.InnerHtml = Text;
            HtmlHelper.PopulateNode(node, HtmlAttributes);
            return node.WriteTo();
        }
    }
}