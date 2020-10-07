using Template;
using Enumerator;
using Utilities.LookUp;
using Indexes;
using Assignments;

namespace Assignments.Decorator
{
	// StringIndex decorator for assignment
	// index doesnt change the raw in template, so 
	public class WithIndex : AssignmentDecorator
	{
		private StringIndex _index;

		public StringIndex Index
		{
			get { return this._index; }
			private set { this._index = value; }
		}

		public WithIndex(Assignment assignment, StringIndex index) : base(assignment)
		{
			this.Index = index;
			base.addContent(Assignment.Content.Index);
		}

		public StringIndex getIndex()
		{
			return this._index;
		}
	}
}