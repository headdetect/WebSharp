using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharp
{
    internal class Configuration
    {
        public static Configuration GetDefaultConfiguration()
        {
            var config = new Configuration();
            config.AutoImports = new[]
            {
                "System",
                "System.Web",
                "System.Net",
                "System.IO"
            };
            config.SilentlyIgnoreDuplicateFiles = true;
            config.DependencyLocation = "dependencies";
            return config;
        }

        public string[] AutoImports { get; set; }
        public bool SilentlyIgnoreDuplicateFiles { get; set; }
        public string DependencyLocation { get; set; }
    }
}
