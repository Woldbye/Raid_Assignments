using System;
using System.Text;

namespace Templates.Tasks
{
  // Type of task
  public enum TaskType
  {
    Message, // 0
    Assignment // 1
  }

  public abstract class TemplateTask
  {	
		// Variable
		protected readonly TaskType _taskType;

    public static class Flags
    {
      public static readonly string Task = "task";
      public static readonly string Assignment = "ass";
    }

    // small class to hold text seperator characters
    public static class Seperator 
    {
      public const char Start = '{', Mid = ':', End = '}', Value = '_';   
    }

  	// Const and static methods
		public static readonly string[] TASK_TO_STR = {
        "Message", "Assignment"
    };
    public static readonly char[] TASK_TO_RAW = {
    	'm', 'a'
    };

  	// constructor with Enum
		public TemplateTask(TaskType task)
		{
			this._taskType = task;
		}

		public TaskType GetTaskType()
		{
			return this._taskType;
		}

		// should return the raw string representation in the template of the task.
		public virtual string ToRaw()
		{
      StringBuilder rawBuilder = new StringBuilder();
      rawBuilder.Append((char) TemplateTask.Seperator.Start);
      rawBuilder.Append(TemplateTask.Flags.Task);
      rawBuilder.Append(TemplateTask.Seperator.Value);
      rawBuilder.Append(TemplateTask.TASK_TO_RAW[(int) this._taskType].ToString());
      rawBuilder.Append(TemplateTask.Seperator.End);
			return rawBuilder.ToString();
		}

		public override string ToString()
		{
			return String.Format("<Template Task>\n\t<Type>\n\t\t{0}", TemplateTask.TASK_TO_STR[(int) this._taskType]);
		}
	}
}