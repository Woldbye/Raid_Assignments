using System;
using Utilities.WorldOfWarcraft;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Readers;

namespace Utilities // utilities
{ 
  public static class Errors
  {
    public const int ERROR_CODE = Int32.MinValue;
  }

  public static class Search
  {
    public const int Fail = -1;
    public const int Success = 1;

  }

  public static class Exceptions 
  {
    public static void ThrowPlayerAssignment(int lineNum, int lineIndex, Player player, int type, int num)
    {
      string msg = String.Format("Invalid assignment of:\n\t{0}\n\t<int type>\n\t\t{1}\n\t<type number>\n\t\t{2}",
                                  player.ToString(),
                                  type,
                                  num);
      msg += String.Format("\n\t<Line num>\n\t\t{0}\n\t<Line index>\n\t\t{1}", 
                            lineNum,
                            lineIndex);
      throw new FormatException(msg);
    }

    public static void ThrowAssignment(string assignment)
    {
      throw new FormatException(String.Format("{0} is not supported as a valid assignment."));
    }

    public static void ThrowTemplate(string errLine, string path)
    {
      throw new FormatException(String.Format("The line {0} at path: {1} is invalid", errLine, path));
    }

    public static void ThrowTemplate(Exception e, string path)
    {
      throw new FormatException(String.Format("Reading of path: {0} threw exception {1}", path, e));
    }

    public static void ThrowRoster() 
    {
      throw new FormatException(String.Format("Invalid format of {0}", Roster.PATH));  
    }

    public static void ThrowSignUp() 
    {
      throw new FormatException(String.Format("Invalid format of {0}", SignUp.PATH)); 
    }

    public static void ThrowNotImplemented(string funcName) 
    {
      throw new Exception(String.Format("Function {0} is not implemented yet", funcName));
    }

    public static void ThrowException(string msg) 
    {
      throw new Exception(msg);
    }

    public static void ThrowArgument(string msg)
    {
      throw new ArgumentException(msg);
    }
  }
}