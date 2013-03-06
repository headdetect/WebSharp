using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace WebSharp
{
    class Program
    {
        private static List<string> LoadedFiles;
        internal static Configuration Configuration;

        static int Main(string[] args)
        {
            string baseFile = null;

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

            if (Configuration == null)
                Configuration = Configuration.GetDefaultConfiguration();
            LoadedFiles = new List<string>();

            var engine = new ScriptEngine();
            engine.AddReference(Assembly.GetEntryAssembly().Location);
            engine.AddReference(typeof(Griffin.Networking.Protocol.Http.Protocol.IRequest).Assembly);
            engine.AddReference(typeof(RazorEngine.Razor).Assembly);
            foreach (var reference in Configuration.AutoImports)
                engine.AddReference(reference);

            var scriptBase = new ScriptBase();
            var session = engine.CreateSession(scriptBase, typeof(ScriptBase));
            scriptBase.Session = session;
            session.ImportNamespace("WebSharp");
            session.ImportNamespace("WebSharp.Routing");
            session.ImportNamespace("WebSharp.Handlers");
            session.ImportNamespace("RazorEngine");
            session.ImportNamespace("Griffin.Networking.Protocol.Http.Protocol");
            foreach (var reference in Configuration.AutoImports)
                session.ImportNamespace(reference);

            // TODO: Compiler error handling
            if (!Path.IsPathRooted(baseFile))
                baseFile = Path.GetFullPath(baseFile);
            ExecuteScript(scriptBase, baseFile);

            return 0;
        }

		static void ExecuteScript(ScriptBase scriptBase, string baseFile)
		{
            LoadedFiles.Add(baseFile);
		    var script = File.ReadAllText(baseFile);
		    var lines = script.Replace("\r", "").Split('\n');
		    for (int i = 0; i < lines.Length; i++)
		    {
		        if (lines[i].StartsWith("//"))
		        {
		            if (lines[i].StartsWith("// Include:"))
		            {
		                var include = lines[i].Substring(lines[i].IndexOf(':') + 1).Trim();
                        if (!include.Contains(Path.DirectorySeparatorChar))
                            include = Path.Combine(Directory.GetCurrentDirectory(), include);
                        var filter = Path.GetFileName(include);
                        var files = Directory.GetFiles(Path.GetDirectoryName(include), filter);
                        foreach (var file in files)
                        {
                            if (!LoadedFiles.Contains(file) || !Configuration.SilentlyIgnoreDuplicateFiles)
                            {
                                // TODO: Allow for custom loaders
                                if (Path.GetExtension(file) == ".cshtml")
                                {
                                    Razor.Compile(File.ReadAllText(file), Path.GetFileNameWithoutExtension(file));
                                    LoadedFiles.Add(file);
                                }
                                else
                                    ExecuteScript(scriptBase, file);
                            }
                        }
		            }
		        }
	            else
	               break;
		    }
		    scriptBase.Session.Execute(script);
		}
    }
}
