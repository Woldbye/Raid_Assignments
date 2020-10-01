using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Utilities // utilities
{ 
  public static class Print
  {
    // no need for constructor
    public static void ByteAsBin(byte byte_to_print) {
      string byte_str = Convert.ToString(byte_to_print, 2).PadLeft(8, '0');
      Console.WriteLine(byte_str);
    }

    // no need for constructor
    public static void IntAsBin(int int_to_print) {
      string byte_str = Convert.ToString(int_to_print, 2).PadLeft(32, '0');
      Console.WriteLine(byte_str);
    }

    // WARNING: Might empty the stack, if so make cp of stackArr before print.
    public static void StackArr(Stack<string>[] stackArr)
    {
      Console.WriteLine("--Starting print of Stack Array--");
      for (int i=0; i < stackArr.Length; i++)
      {
        Console.WriteLine(String.Format("  <Stack Array {0}>", i));
        foreach (string name in stackArr[i])
        {
          Console.WriteLine("\n\t" + name);
        }
      }
    }
  }
}