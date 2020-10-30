using System; 
using System.Text;
using Templates.Tasks.Assignments; 
using Generics;

namespace Templates.Tasks.Assignments
{
  public sealed class DecoratedAssignmentFlags : GenericFlags<AssignmentDecoration>
  {
    private static DecoratedAssignmentFlags _instance;

    static DecoratedAssignmentFlags() {}
    
    public override void InitFlags()
    {
      base[AssignmentDecoration.WowClass] = "cl";
      base[AssignmentDecoration.Priority] = "pr";
      base[AssignmentDecoration.StringIndex] = String.Empty;
    }
      
    public static DecoratedAssignmentFlags Instance  
    {  
      get  
      {  
        if (DecoratedAssignmentFlags._instance == null)  
        {  
          DecoratedAssignmentFlags._instance = new DecoratedAssignmentFlags();  
        }  
        return DecoratedAssignmentFlags._instance;  
      }  
    }
  }
}