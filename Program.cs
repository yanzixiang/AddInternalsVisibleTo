using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Mono.Cecil;

namespace AddInternalsVisibleTo
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      Debugger.Launch();
      string asmFile = args[0];
      string todll = args[1];
      string publickye = "";
      if (args.Length > 2)
      {
        publickye = args[2];
      }
      Console.WriteLine("Making '{0}' InternalsVisibleTo", asmFile);
      Console.WriteLine("{0}", args);

      AssemblyDefinition sourceAssembly = AssemblyDefinition.ReadAssembly(asmFile, new ReaderParameters
      {
        ReadSymbols = false
      });

      if (!sourceAssembly.IsVisibleTo(todll))
      {
        sourceAssembly.AddInternalsVisibleTo(todll,publickye);
      }

      if(asmFile == "")
      {
        return;
      }

      sourceAssembly.Write(asmFile, new WriterParameters
      {
        WriteSymbols = false
      });
    }

    public static void AddInternalsVisibleTo(this AssemblyDefinition asm, 
      string name,
      string publickey="")
    {
      Type t = typeof(InternalsVisibleToAttribute);
      string path = t.Assembly.CodeBase.Remove(0,8);

      MethodReference mr = asm.MainModule.Import(
        t.GetConstructor(new Type[] { typeof(string) }));

      CustomAttribute ca = new CustomAttribute(mr);
      ModuleDefinition tmr = ModuleDefinition.ReadModule(path);
      TypeReference tr = tmr.GetType(
        "System",
        "String");
      CustomAttributeArgument caa = new CustomAttributeArgument(tr, name);

      if(publickey.Length > 0)
      {
        string nameAndPublickey = string.Format("{0},PublicKey={1}",
          name, publickey);
        caa = new CustomAttributeArgument(tr, nameAndPublickey);
      }

      ca.ConstructorArguments.Add(caa);
      asm.CustomAttributes.Add(ca);
    }

    public static bool IsVisibleTo(this AssemblyDefinition asm,string name)
    {
      foreach(CustomAttribute ca in asm.CustomAttributes)
      {
        if(ca.AttributeType.ToString() == "System.Runtime.CompilerServices.InternalsVisibleToAttribute")
        {
          if((string)ca.ConstructorArguments[0].Value == name)
          {
            return true;
          }
        }
      }
      return false;
    }
  }
}
