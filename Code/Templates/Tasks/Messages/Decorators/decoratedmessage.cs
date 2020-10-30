using Templates.Tasks;
using Utilities;
using System;
using Utilities.WorldOfWarcraft;
using Containers;
using Indexes;
using Utilities.LookUp;

namespace Templates.Tasks.Messages 
{
  // All possible assignment AssignmentDecorations. 
  // a decorated assignment is sorted based on the order of the enum (wow.class > priority > stringindex)
  public enum MessageDecoration
  {
    WowClass = 0,
  }

  public abstract class DecoratedMessage : Message
  {
    public Message _message;
    public object[] Decorations;

    // public static new DecoratedMessageFlags Flags = new DecoratedMessageFlags.Instance;

    public object this[MessageDecoration dec]
    {
      get { return this.Decorations[(int) dec]; }
      // to:do make safe
      set { this.Decorations[(int) dec] = value; }
    }

    public DecoratedMessage(Message message) : base()
    {
      this._message = message;
    }

    public bool Contains(params MessageDecoration[] decorations)
    {
      bool contains = true;
      foreach (MessageDecoration decoration in decorations)
      {
        contains &= (this[decoration] != null);
      }
      return contains;
    }

    public override string ToRaw()
    {
      return this._message.ToRaw(); 
    }

    #if(DEBUG)
    public void PrintDecorations()
    {
      Console.WriteLine("Printing all Decorations for this: " + this.ToRaw());
      foreach (object obj in this.Decorations)
      {
        if (obj != null)
        {
          Console.WriteLine("\t" + obj.ToString());
        }
      }
    }
    #endif
  }
}

