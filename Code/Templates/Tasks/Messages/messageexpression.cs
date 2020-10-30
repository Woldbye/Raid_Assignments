using System;
using Templates.Tasks.Assignments; // to:do remove import
using Templates.Tasks;

namespace Templates.Tasks.Messages
{
  public abstract class MessageExpression : TemplateExpression
  {
    public MessageExpression(TemplateTaskContext context = null) : base(context) {}

    public override bool Interpret()
    {
      // For each element in the context queue, call interpreter
      if (!base.IsContextSet())
      {
        return false;
      }

      try 
      {
        object decoration = this.InterpretDecoration(base._context.Input.Dequeue());
        Type decType = decoration.GetType();
        if (decType.Equals(typeof(Message)))
        {
          // base._context.Output = MessageFactory.Construct((AssignmentType) decoration);
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