using Templates.Tasks;
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
namespace Templates.Tasks.Assignments
{
  public sealed class AssignmentTypeExpression : AssignmentExpression
  {
    public AssignmentTypeExpression() : base() {}
    
    public override object InterpretDecoration(string raw)
    {
      object obj = Assignment.TYPE_TO_RAW.IndexOfIgnoreCase(raw);
      if ((int) obj == Search.Fail)
      {
        return null;
      }
      return (AssignmentType) obj;
    }
  }
}