using Template;

namespace Template.Assignment 
{
	public abstract class AssignmentDecorator : Assignment
	{
		// type of assignment
		protected Assignment _assignment;

		public AssignmentDecorator(Assignment assignment) : base(assignment.type)
		{
			this._assignment = assignment;
		}

		public Assignment.Type getAssignmentType()
		{
			return this._type;
		}

		public override string getRaw()
		{
			return base.getRaw(); 
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}

