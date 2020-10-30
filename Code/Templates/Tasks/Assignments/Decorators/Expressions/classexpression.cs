using Templates.Tasks;
using System;
using System.Text;
using Utilities.WorldOfWarcraft;
using Containers;
using Utilities;
using Utilities.LookUp;
using Indexes;
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
  public sealed class ClassExpression : AssignmentExpression
  {
    public ClassExpression() : base() {}

    public override object InterpretDecoration(string raw)
    {
      object obj = Wow.CLASS_TO_STR.IndexOfIgnoreCase(raw);
      if ((int) obj == Search.Fail)
      {
        return null;
      }

      return (Wow.Class) obj;
    }
  }
}