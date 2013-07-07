using System;
using Xipton.Razor;
using HtmlAgilityPack;

namespace WebSharp.MVC
{
    public class HtmlLink
    {
        public string Text { get; set; }
        public string Link { get; set; }
        public object HtmlAttributes { get; set; }

        public HtmlLink(string text, string link, object attributes = null)
        {
            Text = text;
            Link = link;
            HtmlAttributes = attributes;
        }

        public LiteralString Render()
        {
            var node = HtmlNode.CreateNode("<a>");
            node.InnerHtml = Text;
            node.Attributes.Add("href", Link);
            HtmlHelper.PopulateNode(node, HtmlAttributes);
            return node.WriteTo();
        }
    }
}