using Template.Assignment;

namespace Template.Assignment 
{
	public class PlainAssignment : Assignment
	{
		public PlainAssignment(Assignment.Type type) : base(type)
		{
			this.type = type;
		}
	}
}