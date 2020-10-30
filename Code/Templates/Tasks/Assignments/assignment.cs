using Templates.Tasks;
using System;
using System.Text;
using Utilities.WorldOfWarcraft;
using Containers;
using Utilities;
using Utilities.LookUp;
using Indexes;

namespace Templates.Tasks.Assignments
{
	// type of assignment
	public enum AssignmentType
 	{
 	  Tank, // 0
   	Heal, // 1
   	Interrupt, // 2
   	Kite // 3
 	}

	public abstract class Assignment : TemplateTask, IEquatable<Assignment>, IComparable<Assignment>
	{ 
		private AssignmentType _type; // Assignment Type

		public AssignmentType AssignmentType
		{
			get { return this._type; }
			private set { this._type = value; }
		}

    public static readonly string TypeFlag = "ass";
 		public static readonly string[] TYPE_TO_STR = {"Tank", "Heal", "Interrupt", "Kite"};
    public static readonly string[] TYPE_TO_RAW = {"tank", "heal", "interrupt", "kite"};

		public Assignment(AssignmentType type) : base(TaskType.Assignment)
		{
			this.AssignmentType = type;
		}

		public override int GetHashCode()
    {
      return (int) this._type.GetHashCode();
    }

    public bool Equals(Assignment other)
    {
    	if (other == null)
    	{
    		return false;
    	}

			if (!this.AssignmentType.Equals(other.AssignmentType))
    	{
    		return false;
    	}

    	DecoratedAssignment thisDecorated = this as DecoratedAssignment;
    	DecoratedAssignment otherDecorated = other as DecoratedAssignment;
    	if (thisDecorated == null | otherDecorated == null)
    	{
    		return false;
    	}

    	return thisDecorated.Equals(otherDecorated);
    }

    // Make true or false loop to type check:
    // example:
    // for (int i=Assignment.Extension.WowClass; )
    public override bool Equals(object obj)
    {
      if (obj == null) 
      {
        return false;
      }
      Assignment objAsAss = obj as Assignment;
      if (objAsAss == null)
      {
        return false;
      } else {
        return Equals(objAsAss);
      } 
    }
    
    public int CompareTo(Assignment other)
    {
    	if (other == null)
    	{
    		return (int) SortOrder.Follow;
    	}

    	int assignmentTypeCmp = ((int) this.AssignmentType).CompareTo(( (int) other.AssignmentType));
    	if (assignmentTypeCmp != (int) SortOrder.Same)
    	{
    		return assignmentTypeCmp;
    	}

    	DecoratedAssignment thisDecorated = this as DecoratedAssignment;
    	DecoratedAssignment otherDecorated = other as DecoratedAssignment;
    	
      if (thisDecorated == null && otherDecorated == null)
      {
        return (int) SortOrder.Same;
      }

    	if (otherDecorated == null)
    	{
    		return (int) SortOrder.Precede;
    	}

    	if (thisDecorated == null)
    	{
    		return (int) SortOrder.Follow;
    	}
    	return thisDecorated.CompareTo(otherDecorated);
    }

		public override string ToRaw()
		{
			StringBuilder replacement = new StringBuilder(TemplateTask.Seperator.Mid.ToString());
      replacement.Append(Assignment.TypeFlag);
      replacement.Append(TemplateTask.Seperator.Value);
			replacement.Append(Assignment.TYPE_TO_RAW[(int) this.AssignmentType]);
			replacement.Append(TemplateTask.Seperator.End.ToString());
			return base.ToRaw().Replace(TemplateTask.Seperator.End.ToString(),
																  replacement.ToString());
		}

		public override string ToString()
		{
			return String.Format("<Assignment>\n\t<Type>\n\t\t{0}",
													 Assignment.TYPE_TO_STR[(int) this.AssignmentType]);
		}
	}
}