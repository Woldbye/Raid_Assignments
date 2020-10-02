using Template.Assignment;
using Enumerators;
using Indexes;

namespace Template {
	public enum Factory
	{
		AssignmentType = 0,
		Class,
		Index,
		Priority
	}

	public static class AssignmentFactory
	{	
		// Checks whether input byte has Class, Priority And Index bit set.
		// For each set bit, it will append the class to Assignment
		// 		   		Input:   
		// byte: ammendments
		// object[]: values
		//		the index corresponding to Factory.Enum values should contain the object needed for 
		//		the appropriate type constructor.
		//		etc. index 0 should be an AssignmentType enum.
		public static Assignment CreateAssignment(byte ammendments, object[] values)
		{
			Assignment assignment;
			if (values[(int) Factory.AssignmentType].GetType() == typeof(Assignment.Type))
			{
				Assignment.Type assType = (Assignment.Type) values[(int) Factory.AssignmentType]; 
				assignment = new PlainAssignment(assType);
			} else {
				throw new Exception("Invalid type of assignment");
			}
			if (ammendments.Has(Assignment.Ammendment.Class))
			{
				if (values[(int) Factory.Class].GetType() == typeof(Wow_Class))
				{
					Wow_Class _class = (Wow_Class) values[(int) Factory.Class];
					assignment = new WithClass(assignment, _class);
				}
			}
			if (ammendments.Has(Assignment.Ammendment.Index))
			{
				if (values[(int) Factory.Index].GetType() == typeof(StringIndex))
				{
					StringIndex index = (StringIndex) values[(int) Factory.Index]; 
					assignment = new WithIndex(assignment, index);
				}
			}
			if (ammendments.Has(Assignment.Ammendment.Priority))
			{
				if (values[(int) Factory.Priority].GetType() == typeof(Priority))
				{
					Priority priority = (Priority) values[(int) Factory.Priority]; 
					assignment = new WithPriority(assignment, priority);
				}
			}
			return assignment;
		}

		private static bool Has(this byte _byte, Assignment.Ammendment ammendment)
		{
			return ((_byte & (byte) ammendment) > 0);
		}  
	}	
}