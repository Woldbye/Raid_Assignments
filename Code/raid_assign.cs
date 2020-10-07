using Utilities;
using System;
using System.Collections.Generic;
using Readers;
using Containers;
using Writers;
using Enumerator;
using Indexes;
using Wow_Objects;
using Template;
using Assignments;
using Assignments.Decorator;

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
		byte assContent = Assignment.CreateContent(Assignment.Content.Class, 
																							 Assignment.Content.Priority, 
																							 Assignment.Content.Index);
		object[] values = AssignmentFactory.CreateAssignmentInput();
		values[(int) Factory.AssignmentType] = AssignmentType.Heal;
		values[(int) Factory.Class] = Wow_Class.Warlock;
		values[(int) Factory.Index] = new StringIndex(1, 2);
		values[(int) Factory.Priority] = new Priority(1);
		Assignment assignment = AssignmentFactory.CreateAssignment(assContent, values);
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