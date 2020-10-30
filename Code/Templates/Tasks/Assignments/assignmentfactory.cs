using Templates.Tasks;
using Enumerator;
using Indexes;
using Containers;
using System;
using Utilities.WorldOfWarcraft;
using System.Text;
using Templates.Tasks.Assignments;

namespace Templates.Tasks.Assignments {
	public static class AssignmentFactory
	{	
		// Receives an AssignmentType and will construct the appropriate assignment based on the next
		// parameter variables received.
		//		parameter variables must be of type Wow.Class, StringIndex or Priority values 
		public static Assignment Construct(AssignmentType assType, params object[] Decorations)
		{
			Assignment assignment = new PlainAssignment(assType);
			return AssignmentFactory.Decorate(assignment, Decorations);
		}

		// further develop the input assignment 
		public static Assignment Decorate(Assignment assignment, params object[] decorations)
		{
			Assignment retAssignment = assignment; 
			foreach (object decoration in decorations)
			{
				Type valueType = decoration.GetType();
				DecoratedAssignment assAsDecorated = assignment as DecoratedAssignment;
				if (valueType.Equals(typeof(Wow.Class)))
				{
					if (assAsDecorated != null)
					{
						assignment = new WithClass(assignment, (Wow.Class) decoration, assAsDecorated.Decorations);
					} else {	
						assignment = new WithClass(assignment, (Wow.Class) decoration);
					}
				} else if (valueType.Equals(typeof(StringIndex))) 
				{
					if (assAsDecorated != null)
					{
						assignment = new WithIndex(assignment, (StringIndex) decoration, assAsDecorated.Decorations);
					} else {	
						assignment = new WithIndex(assignment, (StringIndex) decoration);
					}
				} else if (valueType.Equals(typeof(Priority))) 
				{
					if (assAsDecorated != null)
					{
						assignment = new WithPriority(assignment, (Priority) decoration, assAsDecorated.Decorations);
					} else {	
						assignment = new WithPriority(assignment, (Priority) decoration);
					}
				} else {
					throw new ArgumentException(String.Format("{0} is not a valid assignment decoration", valueType));
				}
			}

			return assignment;
		}
	}	
}
