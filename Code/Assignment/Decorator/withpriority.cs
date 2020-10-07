using Template;
using Enumerator;
using Utilities.LookUp;
using Containers;
using System;
using System.Text;
using Assignments;

namespace Assignments.Decorator
{
	// Class decorator for assignment
	public class WithPriority : AssignmentDecorator
	{	
		// TO:DO FIX FIX FIX FIX
		public readonly Priority _priority;

		public WithPriority(Assignment assignment, Priority priority) : base(assignment)
		{
			this._priority = priority;
			base.addContent(Assignment.Content.Priority);
		}

		public override string ToRaw()
		{
			string baseRaw = base.ToRaw();
			StringBuilder rawBuilder = new StringBuilder(baseRaw);
			rawBuilder.Remove(baseRaw.Length - 1, 1);
			rawBuilder.Append((char) TemplateTask.Seperator.Mid);
			rawBuilder.Append(this._priority.Number.ToString());
			rawBuilder.Append((char) TemplateTask.Seperator.End);
			return rawBuilder.ToString();
		}

		public Priority getPriority()
		{
			return this._priority;
		}
	}
}