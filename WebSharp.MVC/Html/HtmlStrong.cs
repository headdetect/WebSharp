using System;
using Xipton.Razor;
using HtmlAgilityPack;

namespace WebSharp.MVC
{
    public class HtmlStrong
    {
        public string Text { get; set; }
        public object HtmlAttributes { get; set; }

        public HtmlStrong(string text, object attributes = null)
        {
            Text = text;
            HtmlAttributes = attributes;
        }

        public LiteralString Render()
        {
            var node = HtmlNode.CreateNode("<strong>");
            node.InnerHtml = Text;
            HtmlHelper.PopulateNode(node, HtmlAttributes);
            return node.WriteTo();
        }
    }
}