using Template;

namespace Template.Assignment 
{
	public class Assignment : TemplateTask
	{
		public readonly Assignment.Type _type; // Assignment Type
		protected byte _info;

		// All the possible Ammendments for Assignments, they also work as masks
		public enum Ammendment : byte
		{
			Class = 0x80,
			Priority = 0x40,
			Index = 0x20,
		}

		/*
			Class | Priority | Index 
		*/
		// type of assignment
		public enum Type
 		{
 	    Tank, // 0
    	Heal, // 1
    	Interrupt, // 2
    	Kite // 3
 		}
 		public const string[] TYPE_TO_STR = {
      "Tank", "Healer", "Interrupter", "Kiter"
    	};

    public const string[] TYPE_TO_RAW = {
    	"tank", "healer", "interrupter", "kiter"
    };

		public Assignment(Assignment.Type type) : base(TemplateTask.Type.Assignment)
		{
			this._type = type;
			this._info = 0x00;
		}

		protected void set(Assignment.Ammendment ammendment)
		{
			this._info |= (byte) ammendment; 
		}

		public bool has(Assignment.Ammendment ammendment)
		{
			return ((this._info & (byte) ammendment) > 0);
		}

		public Assignment.Type getAssignmentType()
		{
			return this._type;
		}

		public override string getRaw()
		{
			string replacement = String.Format("{0}{1}{2}",
													 							 ((char) TemplateTask.Seperator.Mid).ToString(), 
													 							 Assignment.TYPE_TO_RAW[(int) this._type].ToString(),
													 							 ((char) TemplateTask.Seperator.End).ToString());
			return base.GetRaw().Replace(((char) TemplateTask.Seperator.End).ToString(),
																   replacement);
		}

		public override string ToString()
		{
			return String.Format("<Assignment>\n\t<Type>\n\t\t{0}",
													 Assignment.TYPE_TO_STR[(int) type]);
		}
	}
}