using Templates.Tasks;
using System;
using System.Text;
using Utilities.WorldOfWarcraft;
using Containers;
using Utilities;
using Utilities.LookUp;
/*
{task_value:a_value:cl_value}
: = compound
first definition, then _, then value
task = task type
ass = assignment type
cl = class
pr = priority
*/
namespace Templates.Tasks.Assignments
{ 
  public sealed class PriorityExpression : AssignmentExpression
  {
    public PriorityExpression() : base() {}
    
    public override object InterpretDecoration(string raw)
    {
      return Int32.TryParse(raw, out int number) ? new Priority(number) : new Priority(0);
    }
  }
}