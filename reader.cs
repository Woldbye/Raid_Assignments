using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Util;
using Generics;

namespace Translate // utilities
{	
	/*
		// TO:DO - 
		// 	Implement class SignUp
		//		  Should read a file which contains copy paste of discord sign up
		// 		  Constructor receives path to file and look up roster dictionary.
		//  	  For each signed member it should look up in dictionary and add the appropriate player.
		//	
		//  Lets say we need interrupters. We will now be able to:
		//		cp interrupt stack
		//		pop player from stack
		//		if player in sign up
		//			assign player to interrupt
		//	  Same idea for healers
	*/
	/*
	public class Discord
	{ 
		
		public Discord(string path, Dictionary<string, Player> roster)
		{
			// Same idea as Reader class. All info about the sign up is kept in the class SignUp.
			// idea:
		}
	}
	*/
	// Table reader for raid_roster.txt
	public class Reader 
	{	
		// Holds the names of all the assignment receivers.
		// constructor receives List<string>[] namesByClass, List<string> admins
		private AssignmentReceivers receivers;
		// Holds the priority stacks.
		// constructor receives Stack<string>[] priorities
		private Priorities priorities;
		// Constructor receives path to raid_roster.txt, and the table object 
		// Holds variety of table information including but not limited to indexes of each table and names.
		private Table tableInfo;
		private Dictionary<string, Player> roster = new Dictionary<string, Player>();
		
		public Reader() {
			// table needs init first, as other function depends on it.
			this.tableInfo = new Table(Strings.RAID_ROSTER_PATH);
			Tuple<Dictionary<string, Player>, List<string>[], List<string>> readRoster = this.readRoster();
			this.roster = readRoster.Item1;
			this.receivers = new AssignmentReceivers(readRoster.Item2, readRoster.Item3);
			this.priorities = new Priorities(this.readPriorities());
		}

		/*		 
		reads a valid roster string and returns 
			class_a = List<String>[sizeOf(Wow_Class Enum)], each index 
				contains a list of all the the names of players of that class.
			DRoster = Dictionary<string, player>, look up for each player by name.
			admins = name of all admins
		*/ 
		private Tuple<Dictionary<string, Player>, List<string>[], List<string>> readRoster() 
		{
			// name of roster table
			const string nameRosterTb = "roster"; 
			int rosterIndex = this.tableInfo.getTableIndexByName(nameRosterTb);
			string[] file = this.tableInfo.getRawLines();
			string aLine = "";

			// A few inits :)
			Dictionary<string, Player> DRoster = new Dictionary<string, Player>(this.tableInfo.getRosterCount());
			int arraySize = Wow_Class.GetNames(typeof(Wow_Class)).Length;
			List<string>[] class_a = new List<string>[arraySize];
			List<string> admins = new List<string>();

			for(int i=0; i < arraySize; i++) 
			{
				class_a[i] = new List<string>();
			} 
			int counter = 1;
			while (true)
			{
				aLine = file[rosterIndex];
				if (aLine == null)
        {
            Error.ThrowRosterError();
        } 
        if (aLine[0].Equals(Table.FINISH_FLAG)) 
        {
            break;
        }
        Player player = this.ExtractPlayer(aLine);
        // add player to all three lists:
        string name = player.getName();
        DRoster.Add(name, player);
        class_a[(int) player.getClass()].Add(name);
        if (player.isAdmin())
        {
        	admins.Add(name);
        }
        rosterIndex++;
        counter++;
			}

			return Tuple.Create(DRoster, class_a, admins);
		}
		// public Player(string name, Wow_Class class_, Role role_, bool isOT, bool isInt)
    private Player ExtractPlayer(string pl_line)
    {
    	#if (DEBUG)
    		string[] plInfoStr = {"name", "class", "role", "isOT", "isInt", "isAdmin"};
            Console.WriteLine("----ExtractPlayer DEBUG info----");
            Console.WriteLine(String.Format("\tReceived pl_line: <{0}>", pl_line));
      #endif
      string line = pl_line;
			List<int> infoIndexes = this.tableInfo.getPlayerInfoIndexes();
			string[] info = new string[infoIndexes.Count];
			int i = 0;

			// extract info
			foreach(int start in infoIndexes) 
			{
				int j = start;
				while (Char.IsLetter(line[j]))
				{
					if (line[j] == null)
					{
						Error.ThrowRosterError();
					}
					j++;
					if (!(j < line.Length))
					{
						break;
					}
				}
				int count = j - start;
				info[i] = line.Substring(start, count);
				#if (DEBUG)
					Console.WriteLine(String.Format("\t\tAdded {0} as {1}", info[i], plInfoStr[i]));
				#endif
				i++;
			}
      // int ot admin
			// cast info and init player
			string name = info[0];
			Wow_Class wow_class = (Wow_Class) Array.IndexOf(Strings.CLASS_TO_STR, info[1]);
			Role role = (Role) Array.IndexOf(Strings.ROLE_TO_STR, info[2]);
			bool isOT = (info[4].Equals("Yes")) ? true : false;
			bool isInt = (info[3].Equals("Yes")) ? true : false;
			bool isAdmin = (info[5].Equals("Yes")) ? true : false;
			Player retPlayer = new Player(name, wow_class, role, isOT, isInt, isAdmin);
			return retPlayer;
    }

		// Extracts priority info and outputs it in a Stack<string>[]. 
		private Stack<string>[] readPriorities()
		{
			string[] file = this.tableInfo.getRawLines();
			string aLine = "";
			List<int> prioIndexes = this.tableInfo.getPrioIndexes();
			Stack<string>[] priorities = new Stack<string>[prioIndexes.Count];
			int i = 0;
			foreach (int index in prioIndexes)
			{
				// init the priority stack
				priorities[i] = new Stack<string>();
				// index to the start of the table content
				int prioIndex = index; 

				// reading of any other table simply involves finding the end indice of the table
                while(true)
                {
                    // start index doesnt matter here as we dont need the info
                    aLine = Strings.Trim(file[prioIndex]);
                    if (aLine == null)
                    {
                        Error.ThrowRosterError();
                    } 
                    if (aLine[0].Equals(Table.FINISH_FLAG)) 
                    {
                        break;
                    }
                    priorities[i].Push(aLine);
                    prioIndex++;
                }
                i++;
			}
			return priorities;
		}

    public AssignmentReceivers getReceivers()
    {
      return this.receivers;
    }

    public Priorities getPriorities()
    {
      return this.priorities;
    }

		public Player getPlayerInRoster(string name) {
			return this.roster[name];
		}
	}
}