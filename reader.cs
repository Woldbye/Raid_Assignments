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
	so it will be able to be called as: TableReader reader = new TableReader(path_to_roster_tables.txt)
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
	public class TableReader 
	{	
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
		// NOTE: Dict cant be omitted - needed for signed_up(string sign_up) 
		// key: name -> Player obj
		private Dictionary<string, Player> roster = new Dictionary<string, Player>();
		// ALL THE ABOVE HAVE BEEN MOVED TO CLASSES INSTEAD
		// This class should instead just init AssignmentReceivers, RosterDic and Orders.
		// The information can then be extracted by accessing the subobjects.
		// private AssignmentReceivers assignStrings;
		// private RosterDictionary;
		// private Orders; 
		// private Table;


		public TableReader() {
			// Inits
			string[] rawRosterLines = File.ReadAllText(Strings.RAID_ROSTER_PATH).Split("\n");
			#if (DEBUG)
				Console.WriteLine("--- Read {0}---", Strings.RAID_ROSTER_PATH);
				int debug_count = 0;
			#endif
			int initSize_o = 8;
			int initSize_a = 12;
			this.interrupt_a = new List<string>(initSize_a);
			this.admin_a = new List<string>(initSize_a);
			this.healer_a = new List<string>(initSize_a);
			this.ranged_a = new List<string>(initSize_a);
			this.tank_a = new List<string>(initSize_a);
			this.melee_a = new List<string>(initSize_a);
			this.tank_o = new Stack<string>(initSize_o);
			this.healer_o = new Stack<string>(initSize_o);
			this.interrupt_o = new Stack<string>(initSize_o);
			this.kiter_o = new Stack<string>(initSize_o);
			this.class_a = new List<string>[Enum.GetNames(typeof(Wow_Class)).Length];
			// Init loop for Array of lists class_a
			foreach(Wow_Class class_ in Enum.GetValues(typeof(Wow_Class))) {
				this.class_a[(int) class_] = new List<string>();
				#if (DEBUG)
					debug_count++;
					Console.WriteLine("this.class_a[{0}] init complete", debug_count);
				#endif 
			}
			this.roster = new Dictionary<string, Player>();
			// extract tables and send them to their respective handlers
			string aLine = "";
			int startIndex = 0;
			while(true)
			{
				aLine = rawRosterLines[startIndex];
			    startIndex++;
			    if (aLine == null)
			    {
			    	Error.ThrowRosterError();
			    } else if (aLine.Equals("#START")) {
			    	break;
			    }
			}
			// Tables start at startIndex :)
			#if (DEBUG)
				Console.WriteLine("Start index of file: {0}", startIndex);
			#endif

			// Read from the first table how many tables in total.
			List<string> table_names = new List<string>();
			aLine = rawRosterLines[startIndex];
			/*
			if (!this.checkTableStartFormat(this.rosterLines, startIndex, tbName)) {
				Error.throwRosterError();
			}
			*/
			while(!aLine.Contains(",")) {
				aLine = fileLines[startIndex];
				startIndex++;
			}
			/*
			loop through file, for each [object] use appripriate readerFunction
			call functions to Read tables
			#if (DEBUG)
				print roster
			#endif
				*/
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

		}

		private void readInterruptO(string TankO_Format) {

		}

		private void readHealerO(string TankO_Format) {
			
		}

		private void readKiteO(string TankO_Format) {
			
		}

		private void readTankO(string TankO_Format) {
			
		}

		public Stack<string> getTankO() {
			throw new Exception();
		}

		public Stack<string> getInterruptO() {
			throw new Exception();
		}

		public Stack<string> getHealerO() {
			throw new Exception();
		}

		public Stack getKiteO() {
			throw new Exception();
		}

		public Player getPlayerInRoster(string name) {
			throw new Exception();
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