using Template;
using Enumerator;
using Utilities.LookUp;
using Containers;

namespace Template.Assignment 
{
	// Class decorator for assignment
	public class WithPriority : AssignmentDecorator
	{	
		// TO:DO FIX FIX FIX FIX
		public readonly Priority _priority;

		public WithPriority(Assignment assignment, Priority priority) : base(assignment)
		{
			this._priority = priority;
			base._assignment.set(Assignment.Ammendment.Priority);
		}

		public override string getRaw()
		{
			if (base._assignment != null)
			{
				string baseRaw = base.getRaw();
				StringBuilder rawBuilder = new StringBuilder(baseRaw);
				rawBuilder.Remove(baseRaw.Length - 1, 1);
				rawBuilder.Append((char) TemplateTask.Seperator.Mid);
				rawBuilder.Append(this._priority.Number.ToString());
				rawBuilder.Append((char) TemplateTask.Seperator.End);
				return rawBuilder.ToString();
			} 
			return String.Empty;
		}

		public Priority getPriority()
		{
			return this._priority;
		}
	}
}