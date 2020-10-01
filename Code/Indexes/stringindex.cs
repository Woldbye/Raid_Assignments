using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using System.Linq;

namespace Indexes // utilities
{ 
  // string indexing
  // 12 bits of unused space for derived classes
  public struct StringIndex
  {
    private uint info;
    public static readonly int Max = 0xFFFF; // 16 bits
    public static readonly int BitsNeededForIndex = StringIndex.Max.GetBitsNeeded();

    public StringIndex (int start, int end)
    {
      this.info = (uint) 0;
      this.setStart(start);
      this.setEnd(end);
    }

    public void setStart(int start)
    {
      bool badIndex = (start > StringIndex.Max) | (start < 0);
      if (badIndex)
      {
        Exceptions.ThrowArgument(String.Format(
          "Argument Start is outside range start={0} and Max is {1}",
          start,
          StringIndex.Max));
      } else if (start > 0)
      {
        this.info = (uint) Bytes.setBitsToNum((int) this.info, 
                                              start, 
                                              0, 
                                              StringIndex.BitsNeededForIndex-1); 
      }
    }

    public void setEnd(int end)
    {
      bool badIndex = (end > StringIndex.Max) | (end < 0);
      if (badIndex)
      {
        Exceptions.ThrowArgument(String.Format(
          "Argument Start is outside range start={0} and Max is {1}",
          end,
          StringIndex.Max));
      } else if (end > 0)
      {
        int startIndex = StringIndex.BitsNeededForIndex;
        int endIndex = 32 - 1;
        this.info = (uint) Bytes.setBitsToNum((int) this.info, 
                                              end, 
                                              startIndex,
                                              endIndex);        
      }
    } 

    public int getStart()
    {
      return Bytes.bitExtracted((int) this.info, StringIndex.BitsNeededForIndex, 1);
    }

    public int getEnd()
    {
      return Bytes.bitExtracted((int) this.info, StringIndex.BitsNeededForIndex, StringIndex.BitsNeededForIndex+1);
    }

    public override string ToString()
    {
      string ret = "<String Index>";
      ret += String.Format("\n\t<Info>\n\t\t<Binary Representation>\n\t\t\t{0}",
                            this.info.ToBinString());
      ret += String.Format("\n\t<Start>\n\t\t{0}", this.getStart());
      ret += String.Format("\n\t<End>\n\t\t{0}", this.getEnd());
      return ret;
    }
  }
}