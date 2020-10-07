using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using System.Linq;

namespace Indexes // utilities
{ 
  public struct StringIndex
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
  }
}