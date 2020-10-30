using System; 
using System.Text;
using Templates.Tasks.Assignments;
using Generics;
using Templates.Tasks;

namespace Templates.Tasks.Messages
{
  public sealed class DecoratedMessageFlags : GenericFlags<MessageDecoration>
  {
    private static DecoratedMessageFlags _instance;

    static DecoratedMessageFlags() {}
    
    public override void InitFlags()
    {
      base[MessageDecoration.WowClass] = "cl";
    }
      
    public static DecoratedMessageFlags Instance  
    {  
      get  
      {  
        if (DecoratedMessageFlags._instance == null)  
        {  
          DecoratedMessageFlags._instance = new DecoratedMessageFlags();  
        }  
        return DecoratedMessageFlags._instance;  
      }  
    }
  }
}