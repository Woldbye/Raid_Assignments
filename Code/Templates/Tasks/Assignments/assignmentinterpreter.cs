using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using System.Linq;
using Templates.Tasks;
using Templates.Tasks.Assignments;
using Decoration = Templates.Tasks.Assignments.AssignmentDecoration;

namespace Templates.Tasks.Assignments
{
  // Receives a Stack<string>, and interprets the string by delegating the 
  // interpretation to the appropriate assignment handler
  public sealed class AssignmentInterpreter : ITemplateInterpreter
  {
    // Indexed by enum AssignmentDecoration - contains an interpreter for each AssignmentDecoration expression 
    private static readonly AssignmentExpression[] _decorationExpressions;
    private static AssignmentInterpreter instance; 

    static AssignmentInterpreter()
    {
      AssignmentInterpreter.instance = null;
      int decCount = Enum.GetValues(typeof(Decoration)).Length;

      AssignmentInterpreter._decorationExpressions = new AssignmentExpression[decCount];
      AssignmentInterpreter._decorationExpressions[(int) Decoration.WowClass] = new ClassExpression();
      AssignmentInterpreter._decorationExpressions[(int) Decoration.Priority] = new PriorityExpression();
      // no interpreter for string index, as string index is not contained in the raw representation
      AssignmentInterpreter._decorationExpressions[(int) Decoration.StringIndex] = null; 
    }

    private AssignmentInterpreter() {}
  
    public static AssignmentInterpreter Instance  
    {  
      get  
      {  
          if (AssignmentInterpreter.instance == null)  
          {  
              AssignmentInterpreter.instance = new AssignmentInterpreter();  
          }  
          return AssignmentInterpreter.instance;  
      }  
    }

    public static AssignmentExpression GetDecorationExpression(Decoration decoration)
    {
      return AssignmentInterpreter._decorationExpressions[(int) decoration];
    }

    public TemplateTask Interpret(string raw)
    {
      string[] rawAsArr = AssignmentInterpreter.SplitRaw(raw);
      Queue<string> contextQueue = new Queue<string>(rawAsArr.Length);
      Queue<string> flags = new Queue<string>(rawAsArr.Length);
      // initialize queues
      // rawAssignmentDecoration
      foreach (string rawDec in rawAsArr)
      {
        string[] rawDecAsArr = rawDec.Split(TemplateTask.Seperator.Value, 2);
        flags.Enqueue(rawDecAsArr[0]);
        contextQueue.Enqueue(rawDecAsArr[1]);
      }

      if (flags.IsEmpty() | contextQueue.IsEmpty())
      {
        throw new ArgumentException(String.Format("Invalid string format - Expecting flag_value:flag2_value2:...:flagN_valueN"));
      }

      TemplateTaskContext context = new TemplateTaskContext(contextQueue);
      if (!AssignmentInterpreter.InterpretAssignmentType(context, flags.Dequeue()))
      {
        throw new ArgumentException("Error - couldn't interpret assignment type");
      }
                 
      while (!context.Input.IsEmpty() && !flags.IsEmpty())
      {
        bool isInterpretationSuccess = false;
        string flag = flags.Dequeue();
        int assDecoration = DecoratedAssignment.Flags.AsArray.IndexOfIgnoreCase(flag);
        if (assDecoration == Search.Fail)
        {
          throw new ArgumentException(String.Format("{0} isn't a valid assignment flag", flag));
        }
        AssignmentExpression assExp = AssignmentInterpreter.GetDecorationExpression((Decoration) assDecoration);
        if (assExp != null)
        {
          assExp.Context = context;
          isInterpretationSuccess = assExp.Interpret();
        }
        if (!isInterpretationSuccess)
        {
          throw new ArgumentException(String.Format("Failed to interpret AssignmentDecoration {0}", (Decoration) assDecoration));
        }
      }
      return context.Output;
    }

    // returns true if success
    private static bool InterpretAssignmentType(TemplateTaskContext context, string flag)
    {
      if (context.Input.IsEmpty() |
          !flag.Equals(Assignment.TypeFlag))
      {
        return false;       
      }
      AssignmentTypeExpression assTypeExpression = new AssignmentTypeExpression();
      assTypeExpression.Context = context; 
      return assTypeExpression.Interpret();      
    }

    // removes last and first letter of the raw assignments
    //  and then it splits string based on :
    public static string[] SplitRaw(string rawStr)
    {
      return rawStr.Split(TemplateTask.Seperator.Mid);
    }
  } 
}