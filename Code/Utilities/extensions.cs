using System;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Templates.Tasks.Assignments;

// This file holds extensions for C# built-in classes. 
namespace Utilities 
{ 
  // TO:DO SUBNAMESPACE EXTENSIONS:
  public static class ValueTypeExtension
  {
    public static bool IsInteger(this ValueType value)
    {
      return (value is SByte ||
              value is Int16 || 
              value is Int32 || 
              value is Int64 || 
              value is Byte || 
              value is UInt16 || 
              value is UInt32 || 
              value is UInt64);
       // detained: value is BigInteger 
    }

    public static bool IsNumeric(this ValueType value)
    {
      return (value is Byte ||
              value is Int16 ||
              value is Int32 ||
              value is Int64 ||
              value is SByte ||
              value is UInt16 ||
              value is UInt32 ||
              value is UInt64 ||
              value is Decimal ||
              value is Double ||
              value is Single);
      //  detained: value is BigInteger
    }
  }

  public static class CharExtension
  {
    public static bool IsAscii(this char c)
    {
      return ((int) c < 128) ? true : false;
    }

    public static string Multiply(this char c, int n)
    {
      string ret = "";
      for (int i=0; i < n; i++)
      {
        ret += c.ToString();
      }
      return ret;
    }
  }

  public static class UintExtension
  {
    public static string ToBinString(this uint i)
    {
      return IntExtension.ToBinString((int) i);
    }

  }

  public static class IntExtension
  {
    public static string ToBinString(this int i)
    {
      return Convert.ToString(i, 2).PadLeft(32, '0');
    }

    // Returns the amount of bits needed to represent the number
    public static int GetBitsNeeded(this int i)
    {
      int iCp = i;
      int count = 0;
      while (iCp > 0)
      {
        count++;
        iCp = (iCp >> 1);
      }

      return count;
    }
  }

  public static class GenericQueueExtension
  {
    public static bool IsEmpty<T>(this Queue<T> genericQ)
    {
      if (genericQ.Count != 0)
      {
        return false;
      }

      return true;
    }
  }

  public static class ByteExtension
  {
    public static string ToBinString(this byte b)
    {
      return Convert.ToString(b, 2).PadLeft(8, '0');
    }
  }

  public static class EnumExtension
  { 
    // Returns the maximum integer value of the input enumerator.
    public static int Max(this Enum eType)
    {
      return Enum.GetValues(eType.GetType()).Cast<int>().Max();
    }

    // Returns the maximum integer value of the input enumerator.
    public static int Min(this Enum eType)
    {
      return Enum.GetValues(eType.GetType()).Cast<int>().Min();
    }
  }

  public static class CharArrayExtension
  {
    public static int IndexOfIgnoreCase(this char[] charArr, char value)
    {
      int j = -1;
      for (int i=0; i < charArr.Length; i++)
      {
        bool same = char.ToUpper(charArr[i]).Equals(char.ToUpper(value));
        if (same) 
        {
          j = i;
          return j;
        }
      }
      return j;
    }

    public static int IndexOfIgnoreCase(this char[] charArr, string value)
    {
      int j = -1;
      for (int i=0; i < charArr.Length; i++)
      {
        string charAsStr = charArr[i].ToString();
        bool same = String.Equals(value, charAsStr, StringComparison.OrdinalIgnoreCase);
        if (same) 
        {
          j = i;
          return j;
        }
      }
      return j;
    }
  }

  public static class TypeExtension
  {
    public static int IndexOf(this Type[] types, Type type)
    {
      int j = -1;
      for (int i=0; i < types.Length; i++)
      {
        if (types[i].Equals(type))
        {
          j = i;
        }
      }
      return j;
    }

    public static bool TryCast<T>(this object obj, out T result)
    {
      if (obj is T)
      {
          result = (T)obj;
          return true;
      }
    
      result = default(T);
      return false;
    }

    public static bool IsAssignmentDecoration(this object obj)
    {
      if (obj != null)
      {
        Type objType = obj.GetType();
        for (int i=0; i < DecoratedAssignment.ValueTypes.Count; i++)
        {
          AssignmentDecoration decoration = (AssignmentDecoration) i;
          if (objType.Equals(DecoratedAssignment.ValueTypes[decoration]))
          {
            return true;
          }
        }
      }
      return false;
    }
  }

  public static class StringArrayExtension
  {
    // Case insentitive indexOf method
    // String array extension
    public static int IndexOfIgnoreCase(this string[] strArr, string value)
    {
      int j = -1;
      for (int i=0; i < strArr.Length; i++)
      {
        bool same = String.Equals(value, strArr[i], StringComparison.OrdinalIgnoreCase);
        if (same)
        {
          j = i;
          return j;
        }
      }
      return j;
    }
  }
}