using Utilities;
using System;
using System.Collections.Generic;
using Readers;
using Containers;
using Writers;
using Enumerator;
using Indexes;
using Utilities.WorldOfWarcraft;
using Templates.Tasks;
using Utilities.LookUp;
using Templates.Tasks.Assignments;
using Templates;
using Templates.Tasks.Messages;

class Raid_Assigner
{
		// TO:DO -
		// use the same namespace across multiple files to clarify program 
		// Convert all data_table files to .csv
		//	implement reading of .csv files
		// Make all dictionaries be:
		// name.hash -> Player 
		// IMPLEMENT DISCORD AND READER/ROSTER WITH SAME INTERFACE: EXTRACT
		// Implement Util library as Extension Methods where appropriate 
		// Make all flags const
		// MAKE GENERIC 32COMPRESSOR
		// All ToString() methods use StringBuilder.
	public static int Main() 
	{
		string raw = "{task_a:ass_tank:cl_warlock:pr_2}";
		Console.WriteLine("Raw: " + raw);
		Console.WriteLine("Interpretation of raw: " + TemplateTaskInterpreter.Instance.Interpret(raw).ToRaw().Equals(raw));

		//Stack<string>
		//AssignmentContext context = 







		/*
		List<Assignment> assignments = new List<Assignment>();
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank));
		Console.WriteLine(assignments.Count);
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Warrior));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, new Priority(1)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Druid, new Priority(2)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Druid, new Priority(14)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Druid, new Priority(3)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, new StringIndex(1,3)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Warrior, new Priority(1)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Warrior, new Priority(15)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Warrior, new Priority(0)));
		

		Assignment this_ = (AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Druid, new Priority(14)));
		Assignment other = (AssignmentFactory.Construct(AssignmentType.Tank, Wow.Class.Druid, new Priority(3)));
		Console.WriteLine(this_.CompareTo(other));
		/*
		/*
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Heal,
																								Wow.Class.Warrior,
																								new Priority(15)));

		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank,
																								Wow.Class.Warrior));

		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank,
																								Wow.Class.Warrior,
																								new Priority(2)));

		assignments.Add(AssignmentFactory.Construct(AssignmentType.Heal,
																								Wow.Class.Warrior,
																								new Priority(8)));

		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank,
																								Wow.Class.Druid,
																								new StringIndex(1, 10)));

		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank,
																								Wow.Class.Rogue));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank,
																								Wow.Class.Rogue,
																								new Priority(1)));
				assignments.Add(AssignmentFactory.Construct(AssignmentType.Tank,
																								new Priority(1)));
		assignments.Add(AssignmentFactory.Construct(AssignmentType.Heal,
																								Wow.Class.Warlock));

		assignments.Add(AssignmentFactory.Construct(AssignmentType.Heal,
																								Wow.Class.Warrior));

		assignments.Add(AssignmentFactory.Construct(AssignmentType.Heal,
																								Wow.Class.Shaman));
		*/
																								/*
		Console.WriteLine("---Before sort---");
		foreach(Assignment ass in assignments)
		{
			Console.WriteLine("\t" + ass.ToRaw());
		}
		assignments.Sort();
		Console.WriteLine("---After sort---");
		foreach(Assignment ass in assignments)
		{
			Console.WriteLine("\t" + ass.ToRaw());
		}
		*/
		/*
		object[] values = AssignmentFactory.CreateAssignmentInput();
		values[(int) Factory.AssignmentType] = AssignmentType.Heal;
		values[(int) Factory.Class] = Wow_Class.Warlock;
		values[(int) Factory.Index] = new StringIndex(1, 2);
		values[(int) Factory.Priority] = new Priority(1);
		List<Assignment> assignments = new List<Assignment>();
		assignments.Add(AssignmentFactory.CreateAssignment(assContent, values));
		values[(int) Factory.Class] = Wow_Class.Warrior;
		assignments.Add(AssignmentFactory.CreateAssignment(assContent, values));
		*/
		
		// Console.WriteLine(assignment.ToRaw());
		/*
		List<Assignment> assList = new List<Assignment>();
		assList.Add(new AssMessage((int) MsgType.Admin));
		assList.Add(new AssMessage((int) MsgType.Druid));
		assList.Add(new AssPlayer((int) AssPlayerType.Heal, (int) AssClass.Warrior, 1));
		assList.Add(new AssPlayer((int) AssPlayerType.Tank, (int) AssClass.Warrior, 2));
		assList.Add(new AssPlayer((int) AssPlayerType.Interrupt, (int) AssClass.Rogue, 1));
		assList.Add(new AssPlayer((int) AssPlayerType.Interrupt, (int) AssClass.None, 0));
		assList.Add(new AssPlayer((int) AssPlayerType.Interrupt, (int) AssClass.None, 9));
		assList.Add(new AssPlayer((int) AssPlayerType.Heal, (int) AssClass.Druid, 3));
		assList.Add(new AssPlayer((int) AssPlayerType.Tank, (int) AssClass.None, 15));
		assList.Add(new AssPlayer((int) AssPlayerType.Heal, (int) AssClass.None, 15));
		assList.Sort();

		foreach (Assignment ass in assList)
		{
			Console.WriteLine(ass.ToString());
		}
		*/
		return 0;
	}
}