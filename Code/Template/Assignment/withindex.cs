using Template;
using Enumerator;
using Utilities.LookUp;
using Indexes;

namespace Template.Assignment 
{
	// StringIndex decorator for assignment
	// index doesnt change the raw in template, so 
	public class WithIndex : AssignmentDecorator
	{
		public readonly StringIndex _index;

		public WithIndex(Assignment assignment, StringIndex index) : base(assignment)
		{
			this._index = index;
			base._assignment.set(Assignment.Ammendment.Index);
		}

		public StringIndex getIndex()
		{
			return this._index;
		}
	}
}