using System;
using System.IO;

namespace Readers 
{ 
  // Class TextReader will initialize a path variable and rawLines variable containing the 
  //  path to file and the file content as a string[].
  // Had to add initTextReader since else we cant properly overwrite readFile if needed.
  public abstract class FileReader
  {
    protected string path;

    protected FileReader(string path)
    {
      this.path = path;
    }

    public string getPath()
    {
      return this.path;
    }

    protected virtual string readFile(string path)
    {
      string rawLines;
      try
      {
        rawLines = File.ReadAllText(path);
      } catch (Exception e)
      {
        throw new Exception(String.Format("Read @path: {0}\n\tException: {1}", path, e));
      }
      return rawLines;
    }
  }
}