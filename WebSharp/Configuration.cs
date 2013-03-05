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
            return config;
        }

        public string[] AutoImports { get; set; }
    }
}
