using Template;
using System;
using System.Text;

namespace Assignments
{
	// type of assignment
	public enum AssignmentType
 	{
 	  Tank, // 0
   	Heal, // 1
   	Interrupt, // 2
   	Kite // 3
 	}

	public class Assignment : TemplateTask
	{
		private AssignmentType _type; // Assignment Type
		protected byte _info;

		public AssignmentType AssignmentType
		{
			get { return this._type; }
			private set {this._type = value; }
		}
		// All the possible Contents for Assignments, they also work as masks
		public enum Content : byte
		{
			Class = 0x80,
			Priority = 0x40,
			Index = 0x20,
		}

 		public static readonly string[] TYPE_TO_STR = {"Tank", "Heal", "Interrupt", "Kite"};
    public static readonly string[] TYPE_TO_RAW = {"tank", "heal", "interrupt", "kite"};

    public static byte CreateContent(params Assignment.Content[] assignmentContent)
		{
			byte ret = 0x00;
			foreach (Assignment.Content content in assignmentContent)
			{
				ret |= (byte) content;
			}
			return ret;
		}

		public Assignment(AssignmentType type) : base(TaskType.Assignment)
		{
			this.AssignmentType = type;
			this._info = 0x00;
		}

		public virtual void addContent(params Assignment.Content[] contentArr)
		{
			foreach (Assignment.Content content in contentArr)
			{
				this._info |= (byte) content;
			}
		}
	
		public virtual bool has(params Assignment.Content[] contentArr)
		{
			byte check = this._info; 
			foreach (Assignment.Content content in contentArr)
			{
				check &= (byte) content;
			}
			return (check > 0);
		}

		public override string ToRaw()
		{
			string replacement = String.Format("{0}{1}{2}",
													 							 ((char) TemplateTask.Seperator.Mid).ToString(), 
													 							 Assignment.TYPE_TO_RAW[(int) this.AssignmentType].ToString(),
													 							 ((char) TemplateTask.Seperator.End).ToString());
			return base.ToRaw().Replace(((char) TemplateTask.Seperator.End).ToString(),
																   replacement);
		}

		public override string ToString()
		{
			return String.Format("<Assignment>\n\t<Type>\n\t\t{0}",
													 Assignment.TYPE_TO_STR[(int) this.AssignmentType]);
		}
	}
}