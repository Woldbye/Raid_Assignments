using System;
using System.Text;
using Utilities.WorldOfWarcraft;
using Utilities;
using Utilities.LookUp;
/*
{task_value:a_value:cl_value}
: = compound
first definition, then _, then value
cl = class
si = stringindex
pr = priority
*/
namespace Templates.Tasks
{ 
  public sealed class TemplateTaskExpression : TemplateExpression
  {
    public TemplateTaskExpression(TemplateTaskContext context = null) : base(context) {}
    // modifies content such that it will look at top of stack call
    // it will then set output to
    // output = AssignmentFactory.Decorate(output, interpretation(top of stack))
    // we dont need to modify input, as it modifies it self by popping from stack
    // It returns true if it succesfully interpreted and decorated assignment.
    // false otherwise
    //    also false on empty interpretation
    public override bool Interpret()
    {
      if (!this.IsContextSet())
      {
        return false;
      }

      try 
      {
        string typeAsStr = this._context.Input.Dequeue();
        int type = TemplateTask.TASK_TO_RAW.IndexOfIgnoreCase(typeAsStr);
        if (type == Search.Fail)
        {
          return false;
        }
        this._context.Output = new PlainTemplateTask((TaskType) type);
      // error if stack is empty
      } catch (InvalidOperationException e)
      {
        #if(DEBUG)
          Console.WriteLine(String.Format("Failed interpretation - InvalidOperationException {0} was thrown", 
                                          e.Message));
        #endif
        return false;
      }
      return true;
    }
  }
} 