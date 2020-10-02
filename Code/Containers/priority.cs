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
    public readonly byte _number;
    public const int MAX = 16;

    public int Number
    {
      get { return (int) this._number; }
      set 
      { 
        if (value > Priority.MAX)
        {
          throw new Exception(String.Format("Tried to set Priority to a number above its Max - Exception {0}", e));
        }
        this._number = Convert.ToByte(value);
      }
    }

    public Priority(int number)
    {
      this.Number = number;   
    }
  }
}