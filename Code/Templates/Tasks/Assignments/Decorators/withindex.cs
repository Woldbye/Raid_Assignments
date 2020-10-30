using Enumerator;
using Utilities.LookUp;
using Indexes;
using System.Text;
using Utilities;
using Templates.Tasks;

namespace Templates.Tasks.Assignments
{
	// StringIndex decorator for assignment
	// index doesnt change the raw in template, so 
	public class WithIndex : DecoratedAssignment	
	{
		public StringIndex Index { get { return (StringIndex) base[AssignmentDecoration.StringIndex]; } }

    public WithIndex(Assignment assignment, StringIndex index, object[] previousAssignmentDecorations) : base(assignment, previousAssignmentDecorations)
    {
    	base[AssignmentDecoration.StringIndex] = index;
    }

		public WithIndex(Assignment assignment, StringIndex index) : base(assignment)
		{
			base[AssignmentDecoration.StringIndex] = index;
		}

		#if(DEBUG)
		public override string ToRaw()
		{
			string baseRaw = base.ToRaw();
			StringBuilder rawBuilder = new StringBuilder(baseRaw);
			rawBuilder.Append("(SI: ");
			rawBuilder.Append(this.Index.Start);
			rawBuilder.Append(", ");
			rawBuilder.Append(this.Index.End);
			rawBuilder.Append(")");
			return rawBuilder.ToString();
		}
		#endif
	}
}