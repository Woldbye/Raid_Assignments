using Templates.Tasks;
using Utilities;
using System;
using Utilities.WorldOfWarcraft;
using Containers;
using Indexes;
using Utilities.LookUp;

namespace Templates.Tasks.Assignments 
{
	// All possible assignment AssignmentDecorations. 
	// a decorated assignment is sorted based on the order of the enum (wow.class > priority > stringindex)
	public enum AssignmentDecoration
	{
		WowClass = 0,
		Priority,
		StringIndex,
	}

	public abstract class DecoratedAssignment : Assignment, IEquatable<DecoratedAssignment>, IComparable<DecoratedAssignment>
	{
		// singleton pattern
		private static DecoratedAssignmentInfo Info = (DecoratedAssignment.Info == null) ? 
																																				new DecoratedAssignmentInfo() : 
																																				DecoratedAssignment.Info;
    public static DecoratedAssignmentInfo ValueTypes => Info;

    public static new DecoratedAssignmentFlags Flags = DecoratedAssignmentFlags.Instance; 
		// type of assignment
		public Assignment _assignment;
		// held value types for each AssignmentDecoration. Will be null if the AssignmentDecoration doesnt hold that value.
		public object[] Decorations;

		// get value of Wow.Class AssignmentDecoration
		public object this[AssignmentDecoration dec]
		{
			get { return this.Decorations[(int) dec]; }
			set 
			{ 
				if (value == null | value.GetType().Equals(DecoratedAssignment.ValueTypes[dec]))
				{
					this.Decorations[(int) dec] = value;
				}
				#if(DEBUG)
				else {
					Console.WriteLine("Error couldnt set dec as valuetype is not correct..");
					Console.WriteLine(String.Format(" Expected: {0} but type is {1}", DecoratedAssignment.ValueTypes[dec], value.GetType()));
				}
				#endif
			}
		}

    static DecoratedAssignment()
    {
      DecoratedTask<AssignmentDecoration>.Info = DecoratedAssignmentInfo.Instance;
      DecoratedTask<AssignmentDecoration>.Flags = DecoratedAssignmentFlags.Instance;
    }

		public DecoratedAssignment(Assignment assignment) : base(assignment.AssignmentType)
		{
			this._assignment = assignment;
			this.Decorations = new Object[DecoratedAssignment.ValueTypes.Count];
		}

		public DecoratedAssignment(Assignment assignment, Object[] previousDecorations) : base(assignment.AssignmentType)
		{
			this._assignment = assignment;
			if (previousDecorations.Length == DecoratedAssignment.ValueTypes.Count)
			{
				this.Decorations = previousDecorations;
			} else {
				throw new ArgumentException("previousDecorations has an invalid length " + previousDecorations.Length);
			}
		}

		public bool Contains(params AssignmentDecoration[] decorations)
		{
			bool contains = true;
			foreach (AssignmentDecoration decoration in decorations)
			{
				contains &= (this[decoration] != null);
			}
			return contains;
		}

		public override string ToRaw()
		{
			return this._assignment.ToRaw(); 
		}

		public override string ToString()
		{
			return this._assignment.ToString();
		}

		public override int GetHashCode()
    {
    	int hashcode = this._assignment.GetHashCode();
    	foreach (object obj in Decorations)
    	{
    		if (obj != null)
    		{
    			hashcode |= obj.GetHashCode();
    			hashcode %= 5;
    		}
    	}
      return hashcode;
    }

    public bool Equals(DecoratedAssignment other)
    {
      if (other == null) 
      {
        return false;
      }
      if (!this._assignment.Equals(other._assignment))
      {
      	return false;
      }

      for (int i=0; i < DecoratedAssignment.ValueTypes.Count; i++)
      {
      	AssignmentDecoration decoration = (AssignmentDecoration) i;
      	if (this.Contains(decoration))
      	{
      		if (!other.Contains(decoration))
      		{
      			return false;
      		}
      		int comparison = this.CompareAs(decoration, other);
      		if (comparison != (int) SortOrder.Same)
      		{
      			return false;
      		}
      	} else if (other.Contains(decoration))
      	{
      		return false;
      	}
      }

      return true;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) 
        {
          return false;
        }
        DecoratedAssignment objAsDecorator = obj as DecoratedAssignment;
        if (objAsDecorator == null)
        {
          return false;
        } else {
          return Equals(objAsDecorator);
        } 
    }
    // ignore stringIndex in sorting
    public int CompareAs(AssignmentDecoration decoration, DecoratedAssignment other)
    {
    	switch (decoration)
    	{
    		case AssignmentDecoration.WowClass:
    			Wow.Class thisClass;
    			Wow.Class otherClass;
    			if (this[decoration].TryCast(out thisClass) &&
    				  other[decoration].TryCast(out otherClass))
    			{
    				return otherClass.CompareTo(thisClass);
    			}
    			break;
    		case AssignmentDecoration.Priority:
    		  Priority thisPriority;
    		  Priority otherPriority;
    		  if (this[decoration].TryCast(out thisPriority) &&
    		  	  other[decoration].TryCast(out otherPriority))
    		  {
    		  	return otherPriority.CompareTo(thisPriority);
    		  }
    			break;
    		case AssignmentDecoration.StringIndex:
    		  StringIndex thisIndex;
    		  StringIndex otherIndex;
    		  if (this[decoration].TryCast(out thisIndex) &&
    		  	  other[decoration].TryCast(out otherIndex))
    		  {
    		  	return otherIndex.CompareTo(thisIndex);
    		  }
    			break;
    		default:
    			break;
    	}
    	return (int) SortOrder.Same;
    }
    /*
			First order is class specific over everything else
				Second order is Priority assignments.
					Third order is StringIndex as they are unspecific
    */
    public int CompareTo(DecoratedAssignment other)
    {
      if (other == null)
      {  
        return (int) SortOrder.Follow;
      } 
      int comparison = this._assignment.AssignmentType.CompareTo(other._assignment.AssignmentType);
      if (comparison != (int) SortOrder.Same)
      {
      	return comparison;
      }

      for (int i=0; i < DecoratedAssignment.ValueTypes.Count - 1; i++)
      {
      	AssignmentDecoration AssignmentDecoration = (AssignmentDecoration) i;
      	if (!this.Contains(AssignmentDecoration) && !other.Contains(AssignmentDecoration))
      	{
      		continue;
      	}
      	if (!other.Contains(AssignmentDecoration))
      	{
      		return (int) SortOrder.Precede;
      	}
      	if (!this.Contains(AssignmentDecoration))
      	{
      		return (int) SortOrder.Follow;
      	}
      	comparison = this.CompareAs(AssignmentDecoration, other);
      	if (comparison != (int) SortOrder.Same)
      	{
      		return comparison;
      	}
      }
      return (int) SortOrder.Same;
    }
    #if(DEBUG)
    public void PrintDecorations()
    {
    	Console.WriteLine("Printing all Decorations for this: " + this.ToRaw());
    	foreach (object obj in this.Decorations)
    	{
    		if (obj != null)
    		{
    			Console.WriteLine("\t" + obj.ToString());
    		}
    	}
    }
    #endif
  }
}

