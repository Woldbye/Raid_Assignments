using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using System.Linq;
using Templates.Tasks;
using Templates.Tasks.Assignments;
  
namespace Templates.Tasks.Messages
{
  // Receives a Stack<string>, and interprets the string by delegating the 
  // interpretation to the appropriate assignment handler
  public sealed class MessageInterpreter : ITemplateInterpreter
  {
    private static readonly string[] _AssignmentDecorationFlags; 
    // Indexed by enum AssignmentDecoration - contains an interpreter for each AssignmentDecoration expression 
    private static readonly MessageExpression[] _AssignmentDecorationExpressions;
    private static MessageInterpreter instance; 

    static MessageInterpreter()
    {
      MessageInterpreter.instance = null;
      int decCount = Enum.GetValues(typeof(AssignmentDecoration)).Length;
      MessageInterpreter._AssignmentDecorationFlags = new string[decCount];

      MessageInterpreter._AssignmentDecorationExpressions = new MessageExpression[decCount];
      // implement MessageWowClass
      // implement MessageRole

      MessageInterpreter._AssignmentDecorationExpressions[(int) AssignmentDecoration.WowClass] = null;
      MessageInterpreter._AssignmentDecorationExpressions[(int) AssignmentDecoration.Priority] = null;
      // no interpreter for string index, as string index is not contained in the raw representation
      MessageInterpreter._AssignmentDecorationExpressions[(int) AssignmentDecoration.StringIndex] = null; 
    }

    /*
    private MessageInterpreter() {}
  
    public static MessageInterpreter Instance  
    {  
      get  
      {  
          if (MessageInterpreter.instance == null)  
          {  
              MessageInterpreter.instance = new MessageInterpreter();  
          }  
          return MessageInterpreter.instance;  
      }  
    }

    public static AssignmentExpression GetAssignmentDecorationExpression(AssignmentDecoration AssignmentDecoration)
    {
      return _AssignmentDecorationExpressions[(int) AssignmentDecoration];
    }

    public static string GetAssignmentDecorationFlag(AssignmentDecoration AssignmentDecoration)
    {
      return MessageInterpreter._AssignmentDecorationFlags[(int) AssignmentDecoration];
    }
    */
    public TemplateTask Interpret(string raw)
    {
      /*
      string[] rawAsArr = MessageInterpreter.SplitRaw(raw);
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
      if (!MessageInterpreter.InterpretAssignmentType(context, flags.Dequeue()))
      {
        throw new ArgumentException("Error - couldn't interpret assignment type");
      }
                 
      while (!context.Input.IsEmpty() && !flags.IsEmpty())
      {
        bool isInterpretationSuccess = false;
        string flag = flags.Dequeue();
        int AssignmentDecoration = MessageInterpreter._AssignmentDecorationFlags.IndexOfIgnoreCase(flag);
        if (AssignmentDecoration == Search.Fail)
        {
          throw new ArgumentException(String.Format("{0} isn't a valid assignment flag", flag));
        }
        AssignmentExpression assExp = MessageInterpreter.GetAssignmentDecorationExpression((AssignmentDecoration) AssignmentDecoration);
        if (assExp != null)
        {
          assExp.Context = context;
          isInterpretationSuccess = assExp.Interpret();
        }
        if (!isInterpretationSuccess)
        {
          throw new ArgumentException(String.Format("Failed to interpret AssignmentDecoration {0}", (AssignmentDecoration) AssignmentDecoration));
        }
      }
      return context.Output;
      */
      return null;
    }
    /*
    // returns true if success
    private static bool InterpretAssignmentType(TemplateTaskContext context, string flag)
    {
      if (context.Input.IsEmpty() |
          !flag.Equals(TemplateTask.Flags.Assignment))
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
    */
  } 
}