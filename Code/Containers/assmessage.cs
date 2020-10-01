using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Wow_Objects;
using System.Linq;
using Readers;
using Enumerator;
using Indexes;

namespace Containers 
{  
  public enum AssMsgInfo {
    Type,
    MessageType 
  };
  // TO:DO FIX SO IT CAN KEEP MORE THAN ONE MESSAGETYPE SO WE CAN IMPLEMENT + AND - OPs. 
  //    Should implement sub class "Message types"?
  // CLASSES
  public class AssMessage : Assignment
  {
    public static int BitsNeededForMsgType = MsgType.Interrupt.Max().GetBitsNeeded();
    protected override uint Info { get; set; }

    public AssMessage(int msgType)
    {
      this.setType((int) AssType.Message);
      this.setMsgType(msgType);
    }

    public void setMsgType(int msgType)
    {
      if (!MsgType.IsDefined(typeof(MsgType), msgType))
      {
        Exceptions.ThrowArgument("Invalid Message Type=" + msgType);
      }
      int end = 32 - Assignment.BitsNeededForType;
      int start = end - AssMessage.BitsNeededForMsgType;
      this.Info = (uint) Bytes.setBitsToNum((int) this.Info, msgType, start, end);
    }

    public int getMsgType()
    {
      int start = 32 - Assignment.BitsNeededForType - AssMessage.BitsNeededForMsgType + 1; 
      return Bytes.bitExtracted((int) this.Info, AssMessage.BitsNeededForMsgType, start);
    }

    // The strAing representation of the message. This is used to "Replace" later.
    public override string ToString()
    {
      string ret = TemplateInfo.GetSeperator(Seperator.Start).ToString();
      ret += LookUp.ASS_TYPE_TO_CHAR[(int) AssType.Message].ToString();
      ret += TemplateInfo.GetSeperator(Seperator.Mid).ToString();
      ret += LookUp.MSG_TYPE_TO_STR[this.getMsgType()].ToLower();
      ret += TemplateInfo.GetSeperator(Seperator.End).ToString();
      return ret;
    }

    public override string DebugString()
    {
      string ret = "<AssMessage>";
      #if (DEBUG)
        int info = (int) this.Info;
        ret += "\n\t<Binary Representation>\n\t\t" + info.ToBinString();
        ret += "\n\t<Integer Representation>\n\t\t" + this.Info;
      #endif
      return ret;
    }
  }
}