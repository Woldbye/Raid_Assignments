namespace Template
{
	public abstract class TemplateTask
	{	
		// Variable
		protected readonly TemplateTask.Type _type;
		
		//Enums
		// Type of task
		public enum Type
		{
			Message, // 0
 		  Assignment // 1
		}

		// works for indexing Assignment.Seperators
  	public enum Seperator : char
  	{
  	  Start = '{', // indicates start of assignment
  	  Mid = ':',
  	  End = '}' // indicates end of assignment
  	}
  	// Const and static methods
		public const string[] TYPE_TO_STR = {
        "Message", "Assignment"
    };
    public const char[] TYPE_TO_RAW = {
    	'm', 'a'
    };

    public static char GetSeperator(Seperator seperator)
    {
      return Assignment.Seperators[(int) seperator];
    }

  	// constructor with Enum
		public TemplateTask(TemplateTask.Type type)
		{
			this._type = type;
		}

		public TemplateTask.Type getTaskType()
		{
			return this._type;
		}

		// should return the raw string representation in the template of the task.
		public virtual string getRaw()
		{
			return String.Format("{0}{1}{2}", 
													 ((char) TemplateTask.Seperator.Start).ToString(), 
													 TemplateTask.TYPE_TO_RAW[(int) this._type].ToString(),
													 ((char) TemplateTask.Seperator.End).ToString());
		}

		public override string ToString()
		{
			return String.Format("<Template Task>\n\t<Type>\n\t\t{0}", TemplateTask.TYPE_TO_STR[(int) this._type]);
		}
	}
}