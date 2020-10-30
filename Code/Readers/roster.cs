using System;
using System.IO;
using System.Collections.Generic;
using Enumerator;
using System.Collections;
using Utilities;
using Containers;
using Utilities.WorldOfWarcraft;
using System.Linq;
using Readers;

namespace Readers 
{	
	/*
		// TO:DO - 
		FIX SO IT DOESNT READ INTERRUPT OR OT INFO
	*/
  // Table reader for raid_roster.txt
	public class Roster : ITextReader<string[]>
	{	
		// Holds the names of all the assignment receivers.
		// constructor receives List<string>[] namesByClass, List<string> admins
		private AssignmentReceivers receivers;
		// Holds the priority stacks.
		// constructor receives Stack<string>[] priorities
		private Priorities priorities;
		// Constructor receives path to raid_roster.txt, and the table object 
		// Holds variety of table information including but not limited to indexes of each table and names.
		private RosterInfo rosterInfo;
		private Dictionary<string, Player> roster = new Dictionary<string, Player>();
		
    public const string PATH = "raid_roster.txt";

		public Roster() {
			// table needs init first, as other function depends on it.
			this.rosterInfo = new RosterInfo();
			this.read(this.rosterInfo.getRawLines());
			this.priorities = new Priorities(this.readPriorities());
      Console.WriteLine("****Finished reader init****");
		}

		/*		 
		reads a valid roster string and sets
      AssignmentReceivers
      the Roster dictionary = Dictionary<string, player>, look up for each player by name.
		*/ 
		public void read(string[] lines) 
		{
			// name of roster table
			const string nameRosterTb = "roster"; 
			int rosterIndex = this.rosterInfo.getTableIndexByName(nameRosterTb);
			string[] file = lines;
			string aLine = "";

			// A few inits :)
			Dictionary<string, Player> DRoster = new Dictionary<string, Player>(this.rosterInfo.getRosterCount());
			int arraySize = Wow.Class.GetNames(typeof(Wow.Class)).Length;
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
            Exceptions.ThrowRoster();
        } 
        if (aLine[0].Equals(RosterInfo.FINISH_FLAG)) 
        {
            break;
        }
        Player player = this.ExtractPlayer(aLine);
        // add player to all three lists:
        string name = player.Name;
        DRoster.Add(name, player);
        class_a[(int) player.Class].Add(name);
        if (player.IsAdmin)
        {
        	admins.Add(name);
        }
        rosterIndex++;
        counter++;
			}
      // set roster
      this.roster = DRoster;
      // set receivers
      this.receivers = new AssignmentReceivers(class_a, admins);
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
			List<int> infoIndexes = this.rosterInfo.getPlayerInfoIndexes();
			string[] info = new string[infoIndexes.Count];
			int i = 0;

			// extract info
			foreach(int start in infoIndexes) 
			{
				int j = start;
				while (Char.IsLetter(line[j]))
				{
					j++;
					if (j >= line.Length)
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
			Wow.Class wow_class = (Wow.Class) Array.IndexOf(Wow.CLASS_TO_STR, info[1]);
			Wow.Role role = (Wow.Role) Array.IndexOf(Wow.ROLE_TO_STR, info[2]);
			bool isOT = (info[4].Equals("Yes")) ? true : false;
			bool isInt = (info[3].Equals("Yes")) ? true : false;
			bool isAdmin = (info[5].Equals("Yes")) ? true : false;
			Player retPlayer = new Player(name, wow_class, role, isAdmin);
			return retPlayer;
    }

		// Extracts priority info and outputs it in a Stack<string>[]. 
		private Stack<string>[] readPriorities()
		{
			string[] file = this.rosterInfo.getRawLines();
			string aLine = "";
			List<int> prioIndexes = this.rosterInfo.getPrioIndexes();
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
                        Exceptions.ThrowRoster();
                    } 
                    if (aLine[0].Equals(RosterInfo.FINISH_FLAG)) 
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

    public List<string> getPlayerNames()
    {
      List<string> ret = new List<string>();
      foreach (string key in this.roster.Keys)
      {
        ret.Add(key.ToLower());
      }
      return ret;
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