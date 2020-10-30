using Enumerator;
using Containers;
using System;
using System.Text;
using Utilities;
using Templates.Tasks;

namespace Templates.Tasks.Assignments
{
	// Class decorator for assignment
	public class WithPriority : DecoratedAssignment
	{	
		public int Number { get { return ((Priority) base[AssignmentDecoration.Priority]).Number; } }
		
		public WithPriority(Assignment assignment, Priority priority, object[] previousDecoration) : base(assignment, previousDecoration)
		{
			base[AssignmentDecoration.Priority] = priority;
		}

		public WithPriority(Assignment assignment, Priority priority) : base(assignment)
		{
			base[AssignmentDecoration.Priority] = priority;
		}

		public override string ToRaw()
		{
			string baseRaw = base.ToRaw();
			StringBuilder rawBuilder = new StringBuilder(baseRaw);
			rawBuilder.Remove(baseRaw.Length - 1, 1);
			rawBuilder.Append((char) TemplateTask.Seperator.Mid);
			rawBuilder.Append(DecoratedAssignment.Flags[AssignmentDecoration.Priority]);
      rawBuilder.Append(TemplateTask.Seperator.Value);
			rawBuilder.Append(Number.ToString());
			rawBuilder.Append((char) TemplateTask.Seperator.End);
			return rawBuilder.ToString();
		}
	}
}