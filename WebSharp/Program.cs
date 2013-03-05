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

            var scriptBase = new ScriptBase();
            var session = engine.CreateSession(scriptBase, typeof(ScriptBase));
            scriptBase.Session = session;
            session.ImportNamespace("WebSharp");
            session.ImportNamespace("WebSharp.Routing");
            session.ImportNamespace("WebSharp.Handlers");
            session.ImportNamespace("RazorEngine");
            session.ImportNamespace("Griffin.Networking.Protocol.Http.Protocol");
            foreach (var reference in config.AutoImports)
                session.ImportNamespace(reference);

            // TODO: Error handling
            ExecuteScript(scriptBase, baseFile);

            return 0;
        }
        
		static void ExecuteScript(ScriptBase scriptBase, string baseFile)
		{
		    var script = File.ReadAllText(baseFile);
		    var lines = script.Replace("\r", "").Split('\n');
		    for (int i = 0; i < lines.Length; i++)
		    {
		        if (lines[i].StartsWith("//"))
		        {
		            if (lines[i].StartsWith("// Include:"))
		            {
		                var include = lines[i].Substring(lines[i].IndexOf(':') + 1).Trim();
		                ExecuteScript(scriptBase, include);
		            }
		        }
	            else
	               break;
		    }
		    scriptBase.Session.Execute(script);
		}
    }
}
