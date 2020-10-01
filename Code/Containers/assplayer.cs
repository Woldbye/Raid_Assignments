using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Wow_Objects;
using System.Linq;
using Enumerator;
using Readers;
using Indexes;

namespace Containers
{
  public enum AssPlayerInfo {
    Type,
    PlayerType,
    Class,
    PriorityNumber
  };

  // Assigned Player Type
  //  The type of the player assignment 
  public enum AssPlayerType 
  {
    Tank, // 0
    Heal, // 1
    Interrupt, // 2
    Kite // 3
  };

  // TO:DO REVERSE NUM SUCH THAT 1 WILL MAP TO 15 AND 15 TO 1
  // Represents a player assignment
  public class AssPlayer : Assignment
  { 
    protected override uint Info { get; set; }
    public static readonly int BitsNeededForAssType = AssPlayerType.Interrupt.Max().GetBitsNeeded();
    // If not a specific class assignment
    public static readonly int BitsNeededForClass = AssClass.Druid.Max().GetBitsNeeded();
    public static readonly int MaxPriorityNumber = 15;
    public static readonly int BitsNeededForPriorityNum = AssPlayer.MaxPriorityNumber.GetBitsNeeded();

    // assType is an AssType Enum:
    //      Message or Player
    // AssPlayerType is an AssPlType Enum
    //    Tank,
    //    Heal
    //    Kite
    //    Interrupt
    // num is the players number in the priority list to assign
    public AssPlayer(int AssPlayerType, int assClass, int priorityNumber)
    {
      if ((Assignment.BitsNeededForType + AssPlayer.BitsNeededForAssType +
           AssPlayer.BitsNeededForClass + AssPlayer.BitsNeededForPriorityNum) > 32)
      {
        Exceptions.ThrowArgument("The size of the arguments is too big, you might to use a long instead.");
      }
      this.setType((int) AssType.Player);
      this.setAssPlayerType(AssPlayerType);
      this.setClass(assClass);
      this.setPriorityNumber(priorityNumber);
    }

    // Returns the bin index to the last element
    public void setAssPlayerType(int type)
    {
      if (!AssPlayerType.IsDefined(typeof(AssPlayerType), type))
      {
        Exceptions.ThrowArgument("Invalid type of player assignment=" + type);
      }

      int end = 32 - Assignment.BitsNeededForType;
      int start = end-AssPlayer.BitsNeededForAssType;
      this.Info = (uint) Bytes.setBitsToNum((int) this.Info, type, start, end-1);
    }

    public void setClass(int classType)
    {
      if (!AssClass.IsDefined(typeof(AssClass), classType))
      {
        Exceptions.ThrowArgument("Invalid type of asssignment class=" + classType);
      }
      int end = 32 - Assignment.BitsNeededForType-AssPlayer.BitsNeededForAssType;
      int start = end - AssPlayer.BitsNeededForClass;
      this.Info = ((uint) Bytes.setBitsToNum((int) this.Info, classType, start, end-1));
    }

    // We transform big PriorityNumberbers to small to get the right sort order
    // PriorityNumber15 will transform to PriorityNumber1 and PriorityNumber1 to PriorityNumber15 etc.
    public void setPriorityNumber(int priorityNumber)
    {
      if (priorityNumber > AssPlayer.MaxPriorityNumber | priorityNumber < 0)
      {
        Exceptions.ThrowArgument("Invalid num argument=" + priorityNumber);
      } else if (priorityNumber > 0)
      {
        Console.WriteLine(priorityNumber);
        int end = 32 - Assignment.BitsNeededForType - AssPlayer.BitsNeededForAssType -
                  AssPlayer.BitsNeededForClass;
        int start = end - AssPlayer.BitsNeededForPriorityNum;
        int transformedPriorityNumber = AssPlayer.MaxPriorityNumber - priorityNumber + 1;
        this.Info = (uint) Bytes.setBitsToNum((int) this.Info, transformedPriorityNumber, start, end-1);
      }
    }

    public int getAssPlayerType()
    {
      int start = 32 - Assignment.BitsNeededForType - AssPlayer.BitsNeededForAssType + 1;
      int ret = Bytes.bitExtracted((int) this.Info,
                                    AssPlayer.BitsNeededForAssType,
                                    start);
      return ret;
    }

    public int getClass()
    {
      int start = 32 - Assignment.BitsNeededForType - AssPlayer.BitsNeededForAssType - AssPlayer.BitsNeededForClass + 1;
      int ret = Bytes.bitExtracted((int) this.Info,
                                    AssPlayer.BitsNeededForClass,
                                    start);
      return ret;
    }

    public int getPriorityNumber()
    {
      int start = 32 - Assignment.BitsNeededForType - 
                       AssPlayer.BitsNeededForAssType - 
                       AssPlayer.BitsNeededForClass - 
                       AssPlayer.BitsNeededForPriorityNum + 1;
      int transformedNum = Bytes.bitExtracted((int) this.Info,
                                               AssPlayer.BitsNeededForPriorityNum,
                                               start);
      return (transformedNum == 0) ? 0 : (Math.Abs(transformedNum - AssPlayer.MaxPriorityNumber) + 1);
    }

    public override string DebugString()
    {
      string ret = "<AssPlayer>";
      #if (DEBUG)
        int info = (int) this.Info;
        ret += "\n\t<Binary Representation>\n\t\t" + info.ToBinString();
        ret += "\n\t<Integer Representation>\n\t\t" + this.Info;
      #endif
      return ret;
    }

    public override string ToString()
    {
      string mid = TemplateInfo.GetSeperator(Seperator.Mid).ToString();
      string ret = TemplateInfo.GetSeperator(Seperator.Start).ToString();
      ret += LookUp.ASS_TYPE_TO_CHAR[(int) AssType.Player].ToString();
      ret += mid + LookUp.ASS_PL_TYPE_TO_STR[this.getAssPlayerType()].ToLower();
      //ret += 
      ret += mid + LookUp.ASS_CL_TO_STR[this.getClass()].ToLower();
      ret += mid + this.getPriorityNumber();
      ret += TemplateInfo.GetSeperator(Seperator.End).ToString();
      return ret;
    }
  }
}