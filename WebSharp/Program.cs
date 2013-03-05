using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace WebSharp
{
    class Program
    {
        static int Main(string[] args)
        {
            string baseFile = null;
            Configuration config = null;

            // Parse arguments
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.StartsWith("-"))
                {

                }
                else
                {
                    if (baseFile == null)
                        baseFile = arg;
                    else
                    {
                        Console.WriteLine("Incorrect usage. See WebSharp --help for more information.");
                        return 1;
                    }
                }
            }

            if (baseFile == null)
            {
                Console.WriteLine("Incorrect usage. See WebSharp --help for more information.");
                return 1;
            }

            if (config == null)
                config = Configuration.GetDefaultConfiguration();

            var engine = new ScriptEngine();
            engine.AddReference(Assembly.GetEntryAssembly().Location);
            engine.AddReference(typeof(Griffin.Networking.Protocol.Http.Protocol.IRequest).Assembly);
            engine.AddReference(typeof(RazorEngine.Razor).Assembly);
            foreach (var reference in config.AutoImports)
                engine.AddReference(reference);

            var session = engine.CreateSession();
            session.ImportNamespace("WebSharp");
            session.ImportNamespace("RazorEngine");
            session.ImportNamespace("Griffin.Networking.Protocol.Http.Protocol");
            foreach (var reference in config.AutoImports)
                session.ImportNamespace(reference);

            // TODO: Error handling
            session.ExecuteFile(args[0]);

            return 0;
        }
    }
}
