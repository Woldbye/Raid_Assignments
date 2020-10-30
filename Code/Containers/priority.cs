using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Utilities.LookUp;
using System.Linq;

namespace Containers // utilities
{
  public struct Priority : IEquatable<Priority>, IComparable<Priority>
  {
    private byte _number;
    public const int MAX = 16;

    public int Number
    {
      get { return (int) this._number; }
      private set 
      { 
        if (value > Priority.MAX)
        {
          throw new Exception("Tried to set Priority to a number above its Max");
        }
        this._number = Convert.ToByte(value);
      }
    }

    public Priority(int number)
    {
      try 
      {
        this._number = (byte) number; 
      } catch (Exception e)
      {
        throw new Exception("Tried to set Priority to a number above its Max - threw exception {0}", e);
      }  
    }

    public override bool Equals(object obj)
    {
        if (obj == null) 
        {
          return false;
        }
        Priority otherAsPrio;
        if (!obj.TryCast(out otherAsPrio))
        {
          return false;
        }
        return Equals(otherAsPrio);
    }

    public override int GetHashCode()
    {
      return (int) this.Number.GetHashCode();
    }

    public bool Equals(Priority other)
    {
      return (this.Number.Equals(other.Number));
    }

    public int CompareTo(Priority other)
    {
      int otherCmpVal = (other.Number == 0) ? Priority.MAX + 1 : other.Number;
      int thisCmpVal = (this.Number == 0) ? Priority.MAX + 1 : this.Number;
      if (other.Number == 0)
      {
        return (int) SortOrder.Follow;
      } else if (this.Number == 0)
      {
        return (int) SortOrder.Precede;
      }
      return otherCmpVal.CompareTo(thisCmpVal); 
    }

    public override string ToString()
    {
      return this.Number.ToString();
    }
  }
}