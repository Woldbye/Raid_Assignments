using System;

namespace Template
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
		protected readonly TaskType _task;

    // small class to hold text seperator characters
    public static class Seperator 
    {
      public const char Start = '{', Mid = ':', End = '}';   
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
			this._task = task;
		}

		public TaskType getTaskType()
		{
			return this._task;
		}

		// should return the raw string representation in the template of the task.
		public virtual string ToRaw()
		{
			return String.Format("{0}{1}{2}", 
													 ((char) TemplateTask.Seperator.Start).ToString(), 
													 TemplateTask.TASK_TO_RAW[(int) this._task].ToString(),
													 ((char) TemplateTask.Seperator.End).ToString());
		}

		public override string ToString()
		{
			return String.Format("<Template Task>\n\t<Type>\n\t\t{0}", TemplateTask.TASK_TO_STR[(int) this._task]);
		}
	}
}