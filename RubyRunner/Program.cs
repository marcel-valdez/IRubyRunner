using System;
using System.IO;
using System.Linq;
using IronRuby;
using Microsoft.Scripting.Hosting;

namespace IRubyRunner
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      // 1. Get script file from args
      //  1.1. Give error if file does not exist.
      string scriptFile = GetScriptFile(args);
      string[] arguments = GetScriptArgs(args);

      // 2. Get ruby runner object
      // 3. Execute ruby object
      ExecuteScript(scriptFile, arguments);
    }

    public static void ExecuteScript(string scriptFile, string[] arguments)
    {
      SystemNumericsLoader.Init();
      ScriptEngine engine = Ruby.CreateEngine();
      ScriptScope scope = engine.CreateScope();

      foreach (string arg in arguments)
      {
        engine.Execute(string.Format("ARGV << '{0}'\n", arg), scope);
      }

      string scriptContent = String.Empty;
      using (var reader = new StreamReader(File.OpenRead(scriptFile)))
      {
        scriptContent = reader.ReadToEnd();
      }

      engine.Execute(scriptContent, scope);
    }

    public static string[] GetScriptArgs(string[] args)
    {
      return args.Length > 1 ? args.Skip(1).ToArray() : new string[] { };
    }

    public static string GetScriptFile(string[] args)
    {
      if (args.Length < 1)
        throw new ArgumentException("No filename provided.");

      if (!File.Exists(args[0]))
        throw new FileNotFoundException(string.Format("File: {0} does not exist.", args[0]));

      return args[0];
    }
  }
}
