using Template;
using Enumerator;
using Utilities.LookUp;

namespace Template.Assignment 
{
	// Class decorator for assignment
	public class WithClass : AssignmentDecorator
	{
		public readonly Wow_Class _class;

    	public const string[] CLASS_TO_RAW = {
    		"druid", "hunter", "mage", "priest", "rogue", "shaman", "warlock",
    		"warrior"
    	};

		public WithClass(Assignment assignment, Wow_Class wow_class) : base(assignment)
		{
			this._class = wow_class;
			base._assignment.set(Assignment.Ammendment.Class);
		}

		public override string getRaw()
		{
			if (base._assignment != null)
			{
				string baseRaw = base.getRaw();
				StringBuilder rawBuilder = new StringBuilder(baseRaw);
				rawBuilder.Remove(baseRaw.Length - 1, 1);
				rawBuilder.Append((char) TemplateTask.Seperator.Mid);
				rawBuilder.Append(ClassDecorator.CLASS_TO_RAW[(int) this._class]);
				rawBuilder.Append((char) TemplateTask.Seperator.End);
				return rawBuilder.ToString();
			} 
			return String.Empty;
		}

		public Wow_Class getClass()
		{
			return this._class;
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}