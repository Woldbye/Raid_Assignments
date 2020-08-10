using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Util;
using Generics;

namespace Translate // utilities
{
	/*
	Table Reader, for reading roster_tables.txt and extracting information.
	so it will be able to be called as: Reader reader = new Reader(path_to_roster_tables.txt)
										reader.getAdmins_a()
										reader.getClass_a
										...
										get function for all
										no setter functions, all variables are private
	
	Class TbReader
		private string file = read entire file in variable during constructor
		private List admins_a =
		...
		private List interrupts_a
		private Player class_A[','];
		private Stacks
		private Dictionary: Key Name -> Player obj

		// path to roster_tables.txt
		constructor(str path) 
			File file = equal to file at path str path
			read file into this.file
			close file
			init all variables;
			call functions to Read tables

		private readerFunctions

		public getFunctions
	*/
	// Table reader for raid_roster.txt
	public class Reader 
	{	
		// Holds the names of all the assignment receivers.
		// constructor receives List<string>[] namesByClass, List<string> admins
		private AssignmentReceivers a_receivers;
		// Holds the priority stacks.
		// constructor receives Stack<string>[] priorities
		private Priorities priorities;
		// Constructor receives path to raid_roster.txt, and the table object 
		// Holds variety of table information including but not limited to indexes of each table and names.
		private Table tableInfo;
		/*
		private List<string> admin_a; // hold name for admins whom should receive all assignments
		private List<string> interrupt_a; // hold name of interrupters whom should receive interrupt assignments
		private List<string> tank_a; // hold name of tanks whom should receive tank assignments
		private List<string> healer_a; // hold name of healers whom should receive healer assignments
		private List<string> ranged_a; // hold name of ranged whom should receive ranged assignments
		private List<string> melee_a; // hold name of melees whom should  receive melee assignments
		private Stack<string> tank_o; // tank order, remember first in, last out -> bottom of list gets pushed first
		private Stack<string> healer_o; // healer order
		private Stack<string> interrupt_o; // interrupt order
		private Stack<string> kiter_o; // kite order
		private List<string>[] class_a; // class_A[(int) Wow_Class.Class] will output list of all players of that class
		*/
		// NOTE: Dict cant be omitted - needed for signed_up(string sign_up) 
		// key: name -> Player obj
		private Dictionary<string, Player> roster = new Dictionary<string, Player>();
		
		public Reader() {
			this.tableInfo = new Table(Strings.RAID_ROSTER_PATH);
			int[] tableIndexes = this.tableInfo.getTableIndexes();
			/* TO:DO
				Make function readRoster.
					Read first table (roster table) and init AssignmentReceivers:
						-Loop first table
						*init this.roster
						*init this.class_a
						-Then send it to Assignment Receivers
			*/
			this.priorities = new Priority(this.readPriorities());
		}

		/*		 
		reads a valid roster_format string and sets 
			admin_a
			interrupt_a
			tank_a
			healer_a
			ranged_a
			melee_a
			class_a
			roster dict
		*/ 
		private void readRoster(string roster_format) {
			Error.Exception();
		}

		// Extracts priority info and outputs it in a Stack<string>[]. 
		private Stack<string>[] readPriorities()
		{
			string[] file = this.tableInfo.getRawLines();
			string aLine = "";
			Stack<string>[] priorities = new Stack<string>[this.tableInfo.getPrioIndexes().Count];
			for (int i=0; i < tableIndexes.Length-1; i++)
			{
				// init the priority stack
				priorities[i] = new Stack<string>();
				// index to the start of the table content
				int fileIndex = tableIndexes[i];
				// reading of any other table simply involves finding the end indice of the table
                while(true)
                {
                    // start index doesnt matter here as we dont need the info
                    aLine = Strings.Trim(file[fileIndex]);
                    if (aLine == null)
                    {
                        Error.ThrowRosterError();
                    } 
                    if (aLine.Contains("}")) 
                    {
                        break;
                    }
                    // TO:DO IMPLEMENT CHECK FOR PLAYER IN ROSTER DURING DEBUG
                    #if (DEBUG)
                    	Console.WriteLine(String.Format("Adding {0} to Priority Stack:{1}", aLine, (Priority) i))
                    #endif
                    priorities[i].Push(aLine);
                } 
			}
			return priorities;
		}

		public Player getPlayerInRoster(string name) {
			Error.Exception();
		}
	}
	/*
	What to read from roster_tables.txt:
	  	Stacks:
			tank_o
			healer_o
			interrupt_o
			kiter_o
		Array:
		class_a[]
			After this is read, we can read all the remaining _a lists
		List:
			admins_a
			tanks_a 
			healers_a
			melee_a 
			ranged_a
			interrupt_a
		Dictionary:
			roster: 
	*/
}