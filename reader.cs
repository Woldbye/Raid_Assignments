using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Util;
using Generics;
using System.Linq;

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

	public class Discord
	{ 
    private SignUp signUpInfo;
    private Priorities prios;
    private List<string> playerNames;

		public Discord(string path, 
                   Priorities allPrios, 
                   List<string> allPlayerNames)
		{
      this.signUpInfo = new SignUp(path);
      this.playerNames = this.readPlayerNames(this.signUpInfo.getRawLines());
      this.prios = this.adjustPriorities(allPrios, this.playerNames);
		}

    private Priorities adjustPriorities(Priorities allPrios, List<string> names)
    {
      string[] prios = Priority.GetNames(typeof(Priority));
      int stackCount = prios.Length;
      Stack<string>[] adjPrios = new Stack<string>[stackCount];
      for (int i=0; i < stackCount; i++)
      {
        adjPrios[i] = new Stack<string>();

      }

      for (int i=0; i < stackCount; i++)
      {
        Priority priority = (Priority) i;
        Stack<string> adjPrio = this.adjustPrio(allPrios.getPriority(priority), names); 
        adjPrios[i] = adjPrio;
      }

      return new Priorities(adjPrios);
    }

    public Stack<string> getPriority(int priority)
    {
      return this.prios.getPriority((Priority) priority);
    }

    private Stack<string> adjustPrio(Stack<string> prio, List<string> names)
    {
      Stack<string> ret = new Stack<string>();
      List<string> stackInitList = new List<string>();
      foreach (string name in prio)
      {
        if (names.Contains(name))
        {
          stackInitList.Add(name);
        }
      }
      int i = 1;
      int length = stackInitList.Count;
      string[] stackInitArray = new string[length];
        
      // push in reverse order so first we convert to array.
      // fst in stackInitList is last to be pushed
      foreach(string name in stackInitList) 
      {
        stackInitArray[length-i] = name;
        i++;
      }

      for (int j=0; j < length; j++)
      {
        string name = stackInitArray[j];
        ret.Push(name);
      }

      return ret;
    }

    private List<string> readPlayerNames(string[] lines)
    {
      List<string> playerNames = new List<string>();
      // go to each index contained in SignUp
      int factionCount = Faction.GetNames(typeof(Faction)).Length;
      #if (DEBUG)
          Console.WriteLine("----readTable DEBUG info----");
      #endif
      for (int i=0; i < factionCount; i++)
      {
        int faction = i;
        string factionStr = Strings.FACTION_TO_STR[faction];
        int index = this.signUpInfo.getIndexToFaction(faction);
        #if (DEBUG)
            Console.WriteLine(String.Format("\t\tReceived faction: <{0}>", factionStr));
            Console.WriteLine(String.Format("\t\t\tTrying to read from line: <{0}>", index));
        #endif
        string line = lines[index];
        string headline = this.signUpInfo.extractHeadline(line);
        while(headline.Contains(factionStr))
        {
          string name = this.readName(line);
          #if (DEBUG)
            Console.WriteLine("\t\tPlayerNames add OP:");
            Console.WriteLine(String.Format("\t\t\t<{0}>", name));
          #endif
          playerNames.Add(name);
        }
      }

      return playerNames;
    }

    private string readName(string line)
    {
      return Strings.FindSecondLetterSubstring(line);
    }
	}

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

    public List<string> getPlayerNames()
    {
      return this.roster.Keys.ToList();
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