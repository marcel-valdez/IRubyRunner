// -----------------------------------------------------------------------
// <copyright file="SystemNumericsLoader.cs" company="">
// Marcel Valdez Orozco. Copyright 2012.
// </copyright>
// -----------------------------------------------------------------------
using System.Reflection;

namespace IRubyRunner
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.IO;

  /// <summary>
  /// Inits System.Numerics v4 instead of the requires v2
  /// </summary>
  public class SystemNumericsLoader
  {
    static SystemNumericsLoader()
    {
      AppDomain.CurrentDomain.AssemblyResolve += ResolveSystemNumeric;
    }

    static Assembly ResolveSystemNumeric(object sender, ResolveEventArgs args)
    {
      if (args.Name.Contains("System.Numerics"))
      {
        object obj = System.Numerics.BigInteger.Zero;
        if (!AppDomain.CurrentDomain
                      .GetAssemblies()
                      .Any(a => a.FullName.Contains("System.Numerics")))
        {
          string currDir = AppDomain.CurrentDomain.BaseDirectory;
          string dllPath = Path.Combine(currDir, "System.Numerics.dll");
          return Assembly.LoadFile(dllPath);
        }
        else
        {
          return AppDomain.CurrentDomain.GetAssemblies()
                          .First(a => a.FullName.Contains("System.Numerics"));
        }
      }
      else
      {
        throw new DllNotFoundException(
          "File for assembly: " + args.Name + " not found.");
      }
    }

    public static void Init()
    {
    }
  }
}
