using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Utilities.LookUp;
using System.Linq;
using System.Text;

namespace Indexes // utilities
{ 
  public struct StringIndex : IEquatable<StringIndex>, IComparable<StringIndex>
  {
    private ushort _start;
    private ushort _end;

    public int Start
    {
      get { return (int) this._start; }
      set { 
        this._start = (ushort) value;
      } 
    }

    public int End
    {
      get { return (int) this._end; }
      set { 
        this._end = (ushort) value;
      } 
    }

    public StringIndex(int start, int end)
    {
      try 
      {
        this._start = Convert.ToUInt16(start);
        this._end = Convert.ToUInt16(end);
      } catch (Exception e)
      {
        throw new Exception(String.Format("Input for StringIndex is invalid - assignment threw {0}", e));
      }
    }

    public override bool Equals(object obj)
    {
        if (obj == null) 
        {
          return false;
        }
        StringIndex objAsStringIndex;
        if (!obj.TryCast(out objAsStringIndex))
        {
          return false;
        }
        return Equals(objAsStringIndex);
    }

    public override int GetHashCode()
    {
      return (int) (this.End.GetHashCode() | this.Start.GetHashCode());
    }

    public bool Equals(StringIndex other)
    {
      if (other.End == 0) 
      {
        return false;
      }
      return (this.End.Equals(other.End) && this.Start.Equals(other.Start));
    }

    public int CompareTo(StringIndex other)
    {
      // uninitialized
      if (other.End == 0 && this.End == 0)
      {
        return (int) SortOrder.Same;
      }
      if (other.End == 0)
      {
        return (int) SortOrder.Follow;
      }  
      if (this.End == 0)
      {
        return (int) SortOrder.Precede;
      }
      int thisCombosition = this.Start + this.End;
      int otherCombosition = other.Start + other.End;
      return otherCombosition.CompareTo(thisCombosition); 
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder("<StringIndex>\n");
      sb.Append("\t<Start> - {");
      sb.Append(this._start);
      sb.Append("}\n\t");
      sb.Append("<End>   - {");
      sb.Append(this._end);
      sb.Append("}");
      return sb.ToString();
    }
  }
}