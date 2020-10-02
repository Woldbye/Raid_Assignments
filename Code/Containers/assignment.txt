using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Wow_Objects;
using System.Linq;
using Enumerator;
using Indexes;


namespace Containers
{
  // Assignment Type
  public enum AssType
  {
    Message, // 0
    Player // 1
  };
  
  // can either be message 0 or player 1
  // next bits decide type 
  public abstract class Assignment : IEquatable<Assignment>, IComparable<Assignment>
  {
    protected abstract uint Info { get; set; }
    public static int BitsNeededForType = AssType.Message.Max().GetBitsNeeded();

    public abstract string DebugString();

    public static int StringToAssType(string value)
    {
      int typeAsInt = LookUp.ASS_TYPE_TO_CHAR.IndexOfIgnoreCase(value);
      if (typeAsInt < 0)
      {
        Exceptions.ThrowAssignment(value);
      }  
      return typeAsInt;
    }

    public static int StringToType(int enumInstanceInt, string value, string[][] typesToStr)
    {
      if (enumInstanceInt < 0 | enumInstanceInt >= typesToStr.Length)
      {
        Exceptions.ThrowArgument(String.Format("[{0}] isn't a valid index into the typeLookUpArrays", enumInstanceInt));
      }
      string[] testSample = typesToStr[enumInstanceInt];
      int typeAsInt = testSample.IndexOfIgnoreCase(value);
      Console.WriteLine(enumInstanceInt);
      Console.WriteLine(typesToStr.Length);
      Console.WriteLine(value);
      Console.WriteLine(typeAsInt);
      if (typeAsInt < 0)
      {
        Exceptions.ThrowAssignment(value);
      }  
      return typeAsInt;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) 
        {
          return false;
        }
        Assignment objAsAss = obj as Assignment;
        if (objAsAss == null)
        {
          return false;
        } else {
          return Equals(objAsAss);
        } 
    }

    protected void setType(int type)
    {
      if (!AssType.IsDefined(typeof(AssType), type))
      {
        Exceptions.ThrowArgument("Invalid argument type="+type);
      }
      int end = 32;
      int start = end-Assignment.BitsNeededForType;
      this.Info = (uint) Bytes.setBitsToNum((int) this.Info, type, start, end);
    }

    public int getAssignmentType()
    {
      return Bytes.bitExtracted((int) this.Info, 
                                Assignment.BitsNeededForType,
                                32-Assignment.BitsNeededForType);
    }

    public override int GetHashCode()
    {
      return (int) this.Info;
    }

    public bool Equals(Assignment other)
    {
      if (other == null) 
      {
        return false;
      }
      return (this.Info.Equals(other.Info));
    }

    public int CompareTo(Assignment other)
    {
      if (other == null)
      {
        return 1;
      } else {
        return other.Info.CompareTo(this.Info); 
      }
    }
  }
}