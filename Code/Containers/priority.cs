using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using System.Linq;

namespace Containers // utilities
{ // TO:DO Implement Priority with reverse sorting: 1>2>3>..>16>0 min 0 should be higher than number 15 etc. 
  public struct Priority
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

    public override string ToString()
    {
      return this._number.ToString();
    }
  }
}