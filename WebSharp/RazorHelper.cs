using System;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Configuration;

namespace WebSharp
{
    public static class RazorHelper
    {
        static RazorHelper()
        {
            var config = new TemplateServiceConfiguration();
            config.BaseTemplateType = typeof(RazorBase);
            var service = new TemplateService(config);
            Razor.SetTemplateService(service);
        }
        
        public static void CompileAllViews(string path)
        {
            foreach (var item in Directory.GetFiles(path, "*.cshtml"))
                Razor.Compile(File.ReadAllText(item), Path.GetFileNameWithoutExtension(item));
        }
    }
}
