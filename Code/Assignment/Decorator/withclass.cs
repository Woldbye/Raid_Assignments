using Template;
using Enumerator;
using Utilities.LookUp;
using System.Text;
using Wow_Objects;
using Assignments;

namespace Assignments.Decorator
{
	// Class decorator for assignment
	public class WithClass : AssignmentDecorator
	{
		private Wow.Class _class;
		
		public Wow.Class Class 
		{
			get { return this._class; }
			private set { this._class = value; }
		}

    public static readonly string[] CLASS_TO_RAW = {
    	"druid", "hunter", "mage", "priest", "rogue", "shaman", "warlock",
    	"warrior"
    };

		public WithClass(Assignment assignment, Wow.Class wow_class) : base(assignment)
		{
			this.Class = wow_class;
			base.addContent(Assignment.Content.Class);
		}

		public override string ToRaw()
		{
			string baseRaw = base.ToRaw();
			StringBuilder rawBuilder = new StringBuilder(baseRaw);
			rawBuilder.Remove(baseRaw.Length - 1, 1);
			rawBuilder.Append((char) TemplateTask.Seperator.Mid);
			rawBuilder.Append(WithClass.CLASS_TO_RAW[(int) this.Class]);
			rawBuilder.Append((char) TemplateTask.Seperator.End);
			return rawBuilder.ToString();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}