using Template;
using Enumerator;
using Indexes;
using Containers;
using System;
using Wow_Objects;
using Assignments.Decorator;

namespace Assignments {
	public enum Factory
	{
		AssignmentType = 0,
		Class,
		Index,
		Priority
	}

	public static class AssignmentFactory
	{	
		public static object[] CreateAssignmentInput()
		{
			return new object[Enum.GetNames(typeof(Factory)).Length];
		}

		// Checks whether input byte has Class, Priority And Index bit set.
		// For each set bit, it will append the class to Assignment
		// 		   		Input:   
		// byte: assignmentContent
		// object[]: values
		//		the index corresponding to Factory.Enum values should contain the object needed for 
		//		the appropriate type constructor.
		//		etc. index 0 should be an AssignmentType enum.
		public static Assignment CreateAssignment(byte assignmentContent, object[] values)
		{
			Assignment assignment;
			if (values[(int) Factory.AssignmentType] != null)
			{
				AssignmentType assType = (AssignmentType) values[(int) Factory.AssignmentType]; 
				assignment = new PlainAssignment(assType);
			} else {
				throw new Exception("Invalid type of assignment - " + (Type) values[(int) Factory.AssignmentType]);
			}
			Console.WriteLine("Made it this far and raw is " + assignment.ToRaw());
			if (assignmentContent.has(Assignment.Content.Class))
			{
				if (values[(int) Factory.Class] != null)
				{
					Wow.Class _class = (Wow.Class) values[(int) Factory.Class];
					assignment = new WithClass(assignment, _class);
				}
			} 
			Console.WriteLine("Made it this far and raw is " + assignment.ToRaw());
			if (assignmentContent.has(Assignment.Content.Index))
			{
				if (values[(int) Factory.Index] != null)
				{
					StringIndex index = (StringIndex) values[(int) Factory.Index]; 
					assignment = new WithIndex(assignment, index);
				}
			}
			Console.WriteLine("Made it this far and raw is " + assignment.ToRaw());
			if (assignmentContent.has(Assignment.Content.Priority))
			{
				if (values[(int) Factory.Priority] != null)
				{
					Priority priority = (Priority) values[(int) Factory.Priority]; 
					assignment = new WithPriority(assignment, priority);
				}
			}
			Console.WriteLine("Made it this far and raw is " + assignment.ToRaw());
			return assignment;
		}

		private static bool has(this byte _byte, Assignment.Content content)
		{
			return ((_byte & (byte) content) > 0);
		}  
	}	
}