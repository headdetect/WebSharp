using System;
using RazorEngine.Templating;

namespace WebSharp
{
    public class RazorBase : TemplateBase
    {
        public void Test()
        {
            Write("Hello, world!");
        }
    }
}
