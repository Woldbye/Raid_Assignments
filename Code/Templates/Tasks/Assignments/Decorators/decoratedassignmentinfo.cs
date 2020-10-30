using System;
using Generics;
using Utilities.WorldOfWarcraft;
using Containers;
using Indexes;

namespace Templates.Tasks.Assignments
{
  // Had to create this class, as I had some issues comparing types through loops without runtime type
  //  calling with GetType(). (i.e. static Type[] arrays with typeof() values  didnt work)
  public sealed class DecoratedAssignmentInfo : GenericInputInfo<AssignmentDecoration>
  {
    private static DecoratedAssignmentInfo _instance;

    public DecoratedAssignmentInfo() : base() {}

    public override void InitTypes()
    {
      this[AssignmentDecoration.WowClass] = Wow.Class.Warrior.GetType();
      this[AssignmentDecoration.Priority] = new Priority(0).GetType();
      this[AssignmentDecoration.StringIndex] = new StringIndex(0, 1).GetType();
    }

    public static DecoratedAssignmentInfo Instance  
    {  
      get  
      {  
        if (DecoratedAssignmentInfo._instance == null)  
        {  
          DecoratedAssignmentInfo._instance = new DecoratedAssignmentInfo();  
        }  
        return DecoratedAssignmentInfo._instance;  
      }  
    }
  }
}