using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IRubyRunner.Test
{
  [TestFixture]
  public class ProgramTest
  {

    [TestAttribute]
    public void TestIfItCanParseArguments()
    {
      // Arrange
      string currDir = AppDomain.CurrentDomain.BaseDirectory;
      string scriptFile = currDir + @"\script.rb";

      // Act
      string filename = Program.GetScriptFile(new[] { scriptFile });

      // Assert
      Assert.That(filename, Is.EqualTo(scriptFile));
      // Reset
    }

    [TestAttribute]
    public void TestIfItFailsOnNoFilename()
    {
      // Arrange            
      // Act
      TestDelegate act = () =>
      {
        string filename = Program.GetScriptFile(new string[] { });
      };

      // Assert

    }

    [TestAttribute]
    public void TestIfItFailsOnUnexistantFilename()
    {
      // Arrange
      string currDir = AppDomain.CurrentDomain.BaseDirectory;
      string scriptFile = currDir + @"\bad_script.rb";

      // Act
      TestDelegate act = () =>
        {
          string filename = Program.GetScriptFile(new[] { scriptFile });
        };

      // Assert
      Assert.Throws<FileNotFoundException>(act);
    }

    [TestAttribute]
    public void TestIfItCanGetScriptArgs()
    {
      // Arrange
      string[] expected = new string[] { "args", "args" };
      string[] args = new[] { "filename", "args", "args" };

      // Act
      string[] actual = Program.GetScriptArgs(args);

      // Assert
      Assert.That(actual, Is.EqualTo(expected));
    }

    [TestAttribute]
    public void TestIfItReturnsEmptyOnNoArgs()
    {
      // Arrange
      string[] expected = new string[] { };

      // Act
      string[] actual = Program.GetScriptArgs(new[] { "hi" });

      // Assert
      Assert.That(actual, Is.EqualTo(expected));
    }

    [TestAttribute]
    public void TestIfItLoadsSimpleScript()
    {
      // Arrange
      string currDir = AppDomain.CurrentDomain.BaseDirectory;
      string scriptFile = currDir + @"\script.rb";      
      byte[] buffer = new byte[7];      
      // Act
      string content = InterceptConsole(
        () => Program.ExecuteScript(scriptFile, new string[] { }),
        buffer);
      
      // Assert                            
      Assert.That(content, Is.EqualTo("test"));
    }

    [TestAttribute]
    public void TestIfItLoadsArgScript()
    {
      // Arrange
      string currDir = AppDomain.CurrentDomain.BaseDirectory;
      string scriptFile = currDir + @"\args_script.rb";
      byte[] buffer = new byte[9];
      // Act
      string content = InterceptConsole(
        () => Program.ExecuteScript(scriptFile, new string[] { "one", "two" }),
        buffer);

      // Assert                            
      Assert.That(content, Is.EqualTo("onetwo"));
    }

    public static string InterceptConsole(Action act, byte[] buffer)
    {
      var memStream = new MemoryStream(buffer);
      var writer = new StreamWriter(memStream, Encoding.UTF8);
      Console.SetOut(writer);      

      act();
      writer.Flush();
      Console.SetOut(Console.Out);

      return new string(Encoding.UTF8.GetChars(buffer).Skip(1).ToArray());
    }    
  }
}
