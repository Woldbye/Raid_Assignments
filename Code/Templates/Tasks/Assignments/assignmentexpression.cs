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
  public abstract class AssignmentExpression : TemplateExpression
  {
    public AssignmentExpression(TemplateTaskContext context = null) : base(context) {}
    // modifies content such that it will look at top of stack call
    // it will then set output to
    // output = AssignmentFactory.Decorate(output, interpretation(top of stack))
    // we dont need to modify input, as it modifies it self by popping from stack
    // It returns true if it succesfully interpreted and decorated assignment.
    // false otherwise
    //    also false on empty interpretation
    public override bool Interpret()
    {
      if (!base.IsContextSet())
      {
        return false;
      }

      try 
      {
        object decoration = this.InterpretDecoration(base._context.Input.Dequeue());
        Type decType = decoration.GetType();
        if (decType.Equals(typeof(AssignmentType)))
        {
          base._context.Output = AssignmentFactory.Construct((AssignmentType) decoration);
        } else {
          if (base._context.Output != null)
          {
            Assignment outputAsAssignment = base._context.Output as Assignment;
            if (outputAsAssignment == null)
            {
              return false;
            }
            base._context.Output = AssignmentFactory.Decorate(outputAsAssignment, decoration);
          } else {
            throw new ArgumentNullException("The AssignmentContext doesn't contain an assignment or an assignment type to construct from");
            return false;
          }
        }
        return true;
        // error if stack is empty
      } catch (InvalidOperationException e)
      {
        #if(DEBUG)
          Console.WriteLine(String.Format("Failed interpretation - InvalidOperationException {0} was thrown", 
                                          e.Message));
        #endif
        return false;
      }
    }

    public abstract object InterpretDecoration(string raw);
  }
} 