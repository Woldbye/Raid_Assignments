using System.Collections.Generic;
using System.Text;
using System;
using Templates.Tasks.Assignments;
using Templates.Tasks.Messages;
using Utilities;

// the abstract interpreter:
// split string up in two queues:
//  flags and values
//  ensure flags and values

namespace Templates.Tasks
{
  public class TemplateTaskInterpreter : ITemplateInterpreter
  {
    private static readonly ITemplateInterpreter[] _tempInterpreter;
    private static TemplateTaskInterpreter _instance;

    public ITemplateInterpreter this[TaskType type]
    {
      get { return TemplateTaskInterpreter._tempInterpreter[(int) type]; }
    }

    static TemplateTaskInterpreter()
    {
      TemplateTaskInterpreter._instance = null;
      int typesCount = Enum.GetValues(typeof(TaskType)).Length;
      TemplateTaskInterpreter._tempInterpreter = new ITemplateInterpreter[typesCount];
      TemplateTaskInterpreter._tempInterpreter[(int) TaskType.Assignment] = AssignmentInterpreter.Instance;
      TemplateTaskInterpreter._tempInterpreter[(int) TaskType.Message] = null;// new MessageExpression();
    }

    private TemplateTaskInterpreter() {}

    public static TemplateTaskInterpreter Instance
    {
      get 
      {
        if (TemplateTaskInterpreter._instance == null)
        {
          TemplateTaskInterpreter._instance = new TemplateTaskInterpreter();
        } 
        return TemplateTaskInterpreter._instance;
      }
    }

    public TemplateTask Interpret(string raw)
    {
      string rdyRaw = this.Trim(raw);
      string[] splitRaw = rdyRaw.Split(TemplateTask.Seperator.Mid, 2);
      string typeInfo = splitRaw[0];
      string remainder = splitRaw[1];

      string[] splitTypeInfo = typeInfo.Split(TemplateTask.Seperator.Value, 2);
      Queue<string> contextInput = new Queue<string>();
      string taskFlag = splitTypeInfo[0];
      if ((splitTypeInfo[1].Length != 1) | 
          (taskFlag.Length < 1) |
          !taskFlag.Equals(TemplateTask.Flags.Task))
      {
        throw new ArgumentException("Invalid Task Expression " + typeInfo);
      }
      contextInput.Enqueue(splitTypeInfo[1]);
      TemplateTaskExpression taskExp = new TemplateTaskExpression(new TemplateTaskContext(contextInput));
      if (!taskExp.Interpret())
      {
        return null;
      }

      return this[(TaskType) taskExp.Context.Output.GetTaskType()].Interpret(remainder);
    }

    private string Trim(string raw)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(raw);
      if (sb.Length > 1)
      {
        sb.Remove(0, 1);
        sb.Remove(sb.Length-1, 1);
      }

      return sb.ToString();
    } 
  }
}
