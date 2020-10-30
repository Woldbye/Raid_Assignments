using System;
using Utilities.WorldOfWarcraft;
using Containers;
using Indexes;

namespace Templates.Tasks.Messages
{
  // Had to create this class, as I had some issues comparing types through loops without runtime type
  //  calling with GetType(). (i.e. static Type[] arrays with typeof() values  didnt work)
  public sealed class DecoratedMessageInfo
  {
    private int count;
    
    public int Count
    {
      get { return this.count; }
      private set { this.count = value;}
    }

    // the values each AssignmentDecoration holds (indexed by AssignmentDecoration enum).
    private Type[] decoratedValueTypes;

    public Type this[MessageDecoration dec]
    {
      get { return this.decoratedValueTypes[(int) dec]; }
      private set { this.decoratedValueTypes[(int) dec] = value; }
    }

    public DecoratedMessageInfo()
    {
      this.Count = Enum.GetValues(typeof(MessageDecoration)).Length;
      this.decoratedValueTypes = new Type[this.count];
      this[MessageDecoration.WowClass] = Wow.Class.Warrior.GetType();
    }
  }
}