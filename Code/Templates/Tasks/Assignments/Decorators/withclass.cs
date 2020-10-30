using Enumerator;
using Utilities.LookUp;
using System.Text;
using Utilities.WorldOfWarcraft;
using System;
using Utilities;
using Templates.Tasks;

namespace Templates.Tasks.Assignments
{
	// Class decorator for assignment
	public class WithClass : DecoratedAssignment
	{
		public Wow.Class Class { get { return (Wow.Class) base[AssignmentDecoration.WowClass]; } }

    public static readonly string[] CLASS_TO_RAW = {
    	"druid", "hunter", "mage", "priest", "rogue", "shaman", "warlock",
    	"warrior"
    };

		public WithClass(Assignment assignment, Wow.Class wow_class, object[] previousAssignmentDecorations) : base(assignment, previousAssignmentDecorations)
		{
			base[AssignmentDecoration.WowClass] = wow_class;
		}

		public WithClass(Assignment assignment, Wow.Class wow_class) : base(assignment)
		{
			base[AssignmentDecoration.WowClass] = wow_class;
		}

		public override string ToRaw()
		{
			string baseRaw = base.ToRaw();
			StringBuilder rawBuilder = new StringBuilder(baseRaw);
			rawBuilder.Remove(baseRaw.Length - 1, 1);
			rawBuilder.Append((char) TemplateTask.Seperator.Mid);
			rawBuilder.Append(DecoratedAssignment.Flags[AssignmentDecoration.WowClass]);
      rawBuilder.Append(TemplateTask.Seperator.Value);
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