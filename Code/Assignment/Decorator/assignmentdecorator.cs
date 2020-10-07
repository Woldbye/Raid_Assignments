using Template;
using Assignments;

namespace Assignments.Decorator 
{
	public abstract class AssignmentDecorator : Assignment
	{
		// type of assignment
		private Assignment _assignment;

		public AssignmentDecorator(Assignment assignment) : base(assignment.AssignmentType)
		{
			this._assignment = assignment;
		}

		public override void addContent(params Assignment.Content[] content)
		{
			this._assignment.addContent(content);
		}

		public override bool has(params Assignment.Content[] content)
		{
			return this._assignment.has(content);
		}

		public override string ToRaw()
		{
			return this._assignment.ToRaw(); 
		}

		public override string ToString()
		{
			return this._assignment.ToString();
		}
	}
}

