using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using System.Linq;

namespace Indexes // utilities
{ 
  /*
  // Derivation of StrIndex. Also contains a number to represent the line number.
  public class FileIndex : StringIndex
  {
    public FileIndex (int start, int end, int lineNum) : base(start, end)
    {
      bool badIndex = (lineNum > StringIndex.MAX);
      if (badIndex)
      {
        Exceptions.ThrowArgument(String.Format(
          "Argument lineNum is outside range lineNum={0} and MAX={2}",
          lineNum,
          StringIndex.MAX));
      }

      this.setLineNum(lineNum);
    }

    private void setLineNum(int lineNum)
    {
      base.info = Bytes.setBitsToNum(base.info, lineNum, 20, 29);
    }

    public int getLineNum()
    {
      return Bytes.bitExtracted(this.info, 10, 21);
    }

    public override string ToString()
    {
      string ret = "<File Index>\n";
      ret += String.Format("\t<Info>\n\t\t<Binary Representation>\n\t\t\t{0}\n",
                            this.info.ToBinString());
      ret += String.Format("\t<Line Num>\n\t\t{0}\n", this.getLineNum());
      ret += "\n" + base.ToString().Replace("\n", "\n\t");
      return ret;
    }
    /*
    public override string ToString(int tabCount)
    {
      string tabs = '\t'.Multiply(tabCount);
      string ret = String.Format("{0}<File Index>\n", tabs);
      ret += String.Format("{0}\t<Info>\n{0}\t\t<Binary Representation>\n{0}\t\t\t{1}\n",
                            tabs,
                            this.info.ToBinString());
      ret += String.Format("{0}\t<Line Num>\n{0}\t\t{1}\n", tabs, this.getLineNum());
      ret += base.ToString(1 + tabCount); 
      return ret;
    }

  }
  */
}