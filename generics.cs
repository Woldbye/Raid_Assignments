using Util;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Generics {
    // If alliance replace shaman with paladin
    public enum Wow_Class 
    {
        Druid, // 0
        Hunter, // 1
        Mage, // 2
        Priest, // 3
        Rogue, // 4
        Shaman, // 5
        Warlock, // 6
        Warrior // 7
    }; // 3 bits

    public enum Role 
    {
        Tank, // 0
        Healer, // 1
        Melee, // 2
        Ranged // 3
    }; // 2 bit

    public enum Priority
    {
        Tank,
        Healer,
        Interrupt,
        Kiters
    };

    // Should match the names of the priority tables.
    private const string PRIO_FLAG = "prio";
    public static string[] PRIORITY_STR = ["tank_" + PRIO_FLAG, "healer_" + PRIO_FLAG, "interrupt_" + PRIO_FLAG, "kiter_" + PRIO_FLAG];
    // Constructor receives path to raid_roster.txt, and the table object 
    // will hold index to each of the tables to read, and the name of the tables.
    public class Table 
    {
        // Holds the name of each table
        private readonly List<string> tableNames;
        // Holds the index to each of the priority tables
        private readonly List<int> indexToPrios;
        /*
         Holds the index to all of the tables in rawLines.
         If tableA is contained at index 2 in tableNames, its index in rawLines will similarily 
         be contained at indexToTables[2]
         TO:DO Convert type to List<int> for simplification.
        */
        private readonly int[] indexToTables;
        /*
         The path file as RawLines. Each line in the array corresponds to a line in the file such that 1st line in file is at index 0.
        */
        private readonly string[] rawLines;
        // The index where all the tables start
        private readonly int startIndex;
        // The index where all the tables end
        private readonly int endIndex;
        private const string INIT_TABLE_NAME = "tables_list";

        public Table(string path) 
        {
            this.tableNames = new List<string>();
            // head is the init table name
            this.tableNames.Add(Table.INIT_TABLE_NAME);
            this.rawLines = File.ReadAllText(path).Split("\n");
            this.indexToPrios = new List<int>(); // we init this as a list since we dont know beforehand how many prio tables are present
            // extract tables and send them to their respective handlers
            int i = 0;    
            string aLine = "";
            while(true)
            {
                aLine = this.rawLines[i];
                if (aLine == null)
                {
                    Error.ThrowRosterError();
                } else if (aLine.Contains("#START")) 
                {
                    break;
                }
            }
            this.startIndex = i;
            i++;
            int j = 0; // table index

            // loop through entire file, till the end is reached.
            // For each table found, set the index to the Table and extract tableNames if its the first table.
            while (!aLine.Contains("#END")) 
            {
                aLine = this.rawLines[i];
                if (aLine.Contains("[")) 
                {
                    bool isPrio;
                    int indexToTable = -1;
                    int endIndex = -1;

                    string tableName = this.readTableName(aLine);
                    Tuple<bool, int> tableInfo = this.readTable(i, this.rawLines, tableName); 
                    // extract tuple info
                    isPrio = tableInfo.Item1; 
                    i = tableInfo.Item2; // i is now equal to the line with },
                    indexToTable = tableInfo.Item3;
                    
                    if (tableName.Equals(this.tableNames.First())) 
                    {
                        this.indexToTables = new int[this.tableNames.Count];
                    } else if (!this.tableNames.Contains(tableName))
                    {
                        Error.Exception(String
                                .Format("Invalid name: <{0}>\n\ttableNames doesn't contain the object. tableNames is equal to: <{2}>"
                                    , tableName, this.tableNames));
                    }
                    if (isPrio)
                    {
                        this.indexToPrios.Add(indexToTable);    
                    }
                    // Would be faster to append, but we use index here for clarity
                    this.indexToTables[j] = indexToTable;
                    #if (DEBUG)
                        Console.WriteLine(String.Format("Read table {0}", tableName));
                    #endif
                    j++;
                } 
                i++; // next table or end.
            }
            // small check
            if (this.indexToPrios.Count != Priority.GetNames(typeof(Priority)).Length)
            {
                Error.ThrowRosterError();
            }

            endIndex = i-1;
            #if (DEBUG)
                Console.WriteLine("Finished initializing <Table>:");
                Console.WriteLine(this.toString());
            #endif 
        }
        public int[] getTableIndexes()
        {
            return this.indexToTables;
        }

        public int getTableIndex(int i)
        {
            return this.tableIndexes[i];
        }

        public List<int> getPrioIndexes()
        {
            return this.indexToPrios;
        }

        // returns -1 if it cant be found
        public int getTableIndexByName(string name)
        {
            int index = this.tableNames.FindIndex(x => x.Contains(name.ToLower()));
            if (index == -1) 
            {
                return index;
            } else {
                return this.tableIndexs[index];
            }
        }

        public int getStartIndex() 
        {
            return this.startIndex;
        }

        public int getEndIndex() 
        {
            return this.endIndex;
        }

        public string[] getRawLines() 
        {
            return this.rawLines;
        }

        public List<string> getTableNames()
        {
            return this.tableNames;
        }

        public string getTableName(int i) 
        {
            return this.tableNames[i];
        }

        public override string toString() 
        {
            string ret = "<Table>:";
            ret += String.Format("\n\t<INIT_TABLE_NAME>:\t{0}", Table.INIT_TABLE_NAME);
            ret += String.Format("\n\t<Start index of tables>:\t{0}", startIndex);
            ret += String.Format("\n\t<End index of tables>:\t{0}", endIndex);
            ret += "\n\t<TableNames and their indexes>:";
            ret += String.Format("\n\t\tNum:\tName\tIndex");
            for (int i=0; i < this.indexToTables.Length-1; i++)
            {
                ret += String.Format("\n\t\t{0}:\t{1}:\t{2}", i+1, this.getTableName(i), this.indexToTables[i]);
            }
            ret += "\n\t<Index to Priority Table>:";
            int i=0;
            foreach (int prioI in this.indexToPrios)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", i+1, prioI);
                i++;
            }
            return ret;
        }

        // Table name in format [tableName] so we just trim (also converts to lower-case)
        // and remove the first and last letter.
        private string readTableName(string line) 
        {
            string trimmedLine = Strings.Trim(line);
            return trimmedLine.Substring(1, trimmedLine.Length-2);
        }

        // Take index to table and read the table by updating the index info and tableNames variables
        // This reader assumes the format is correct on the table, and the received index i points to the declartion of the table.
        //      It further assumes there are no empty tables.
        // The function will also initialize the indexToTables array, as its the first function to 
        // confirm the size of the array.
        // Parameters: 
        //      <i> - Type int: Index to the beginning of the table   
        //      <lines> - Type string[]: file containing the tables as a string array for each line.
        //      <name> - Type string: name of the table to read.
        //      Function returns a tuple containing: 
        //          {bool} whether the table is a priority table, 
        //          {int} end index of the table,
        //          {int} start index of the table lines
        private Tuple<bool, int, int> readTable(int i, string[] lines, string name) 
        {
            int j = i;
            bool isPrio = false;
            string aLine = "";
            int beginIndex = -1;
            #if (DEBUG)
                Console.WriteLine("----readTable DEBUG info----");
                Console.WriteLine(String.Format("\t\tReceived i: <{0}>", i));
                Console.WriteLine(String.Format("\t\tTrying to read table: <{0}>", name));
            #endif
            if (name.Equals(this.tableNames.First())) 
            {
                beginIndex = this.findStartIndex(lines, j);
                j = beginIndex;
                while(true) 
                {
                    aLine = lines[j];
                    if (aLine == null)
                    {
                        Error.ThrowRosterError();
                    } 
                    if (aLine.Contains("}")) 
                    {
                        break;
                    }
                    string subTable = Strings.Trim(aLine);
                    #if (DEBUG)
                        Console.WriteLine(String.Format("Adding table named <{0}> to List<string> tableNames", subTable));
                    #endif
                    this.tableNames.Add(subTable);
                    j++;
                }  
            } else if(PRIORITY_STR.Any(s => s.Contains(name))) {
                isPrio = true;
                beginIndex = this.findStartIndex(lines, j);
                j = beginIndex;
                while(true)
                {
                    aLine = lines[j];
                    if (aLine == null)
                    {
                        Error.ThrowRosterError();
                    } 
                    if (aLine.Contains("}")) 
                    {
                        break;
                    }
                    j++;
                }
            } else {
                beginIndex = this.findStartIndex(lines, j);
                j = beginIndex;
                // reading of any other table simply involves finding the end indice of the table
                while(true)
                {
                    // start index doesnt matter here as we dont need the info
                    aLine = lines[j];
                    if (aLine == null)
                    {
                        Error.ThrowRosterError();
                    } 
                    if (aLine.Contains("}")) 
                    {
                        break;
                    }
                    j++;
                }       
            } 
            if (j < 0 | beginIndex < 0)
            {
                Error.ThrowRosterError();
            }
            return Tuple.Create(isPrio, j, beginIndex);
        }

        // Input: receive the file lines and the current index. 
        // Output: it returns the start index of the table content.
        private int findStartIndex(string[] lines, int index) 
        {
            string aLine = "";
            char flag = '{';        
            int i = index;
            while(!aLine.Contains(flag))
            {
                aLine = lines[i];
                i++;
            }
            return i;
        }

        // Checks the format for the beginning two lines of a table
        public bool checkTableStartFormat(string[] fileLines, int index, string tbName) {
            string aLine = fileLines[index];
            if(!aLine.Contains("[" + tbName + "]")) {
                return false;
            }
            index++;
            aLine = fileLines[index];
            if(!aLine.Contains("{")) {
                return false;
            }
            return true;
        }
    }

    /*  0  111
        Player:
            -- Player base object --
            Name: String

            Role - 4 options: (tank), (melee DPS), (ranged DPS), (healer) 
                Best represented by 2 bits: 00 (4 options)
            Class:
                8 options - 000 3 bits
            in total we can represent with 7 bits
            2 for role, 3 for class, 1 for back-up tank, 1 for interrupter = 7
            0*0*{0}[0 00](00): (Role) [class] {back-up tank} *interrupter*
    */
    public class Player
    { 
    	private Byte[] nameID;
    	private byte info;
    	// Constructor
    	// Int = interrupter
    	public Player(string name, Wow_Class class_, Role role_, bool isOT, bool isInt) 
    	{	
    		this.nameID = Strings.Hash(name);
    		this.info = (byte) 0; 
            this.setOT(isOT);
            this.setInterrupt(isInt);
    		this.setClass(class_);
    		this.setRole(role_);
			#if (DEBUG)
				Console.WriteLine("---Player init complete, printing info:---");
    			Console.WriteLine(this.ToString());
    		#endif   		
    	}


        public bool Equals(string str) {
            Byte[] strID = Strings.Hash(str);
            return ByteOP.ByteArrCompare(this.nameID, strID);
        }

        public override bool Equals(Object obj)
        {
        //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {

                return false;
            } else {
                Player obj_pl = (Player) obj;
                return ByteOP.ByteArrCompare(this.nameID, obj_pl.nameID);
            }
        }

    	// Function to extract k bits from p position 
		// and returns the extracted value as byte 
		// public static byte bitExtracted(byte number, int k, int p) 
    	// first 3 bits
    	public Wow_Class getClass() 
    	{
    		return (Wow_Class) ByteOP.bitExtracted(this.info, 3, 5+1);
    	}

    	public Role getRole() {
    		return (Role) ByteOP.bitExtracted(this.info, 2, 3+1);
    	}

    	// index 6
    	public bool isOT() 
    	{
    		return Convert.ToBoolean(this.info & (1 << 1));
    	}

    	public bool isInterrupter() 
    	{
    		return Convert.ToBoolean(this.info & (1 << 2));
    	}

    	// class is the first/last 3 bits, lets try last :)
    	public void setClass(Wow_Class class_) 
    	{
    		this.info = ByteOP.setBitsToNum(this.info, (byte) class_, (byte) 5, (byte) 7);
    	}

    	// role 3 to 4 index
    	public void setRole(Role role_) 
    	{
    		this.info =  ByteOP.setBitsToNum(this.info, (byte) role_, (byte) 3, (byte) 4);
    	}
    	
    	public void setInterrupt(bool interrupt) 
    	{
    		this.info = ByteOP.modifyBit(this.info, 2, interrupt);
    	}

    	public void setOT(bool OT) 
    	{
    		this.info = ByteOP.modifyBit(this.info, 1, OT);
    	}

        public override string ToString() 
        {
        	string ret = "<Player>";
        	#if (DEBUG)
        		ret += "\n\t<Binary Representation>\n\t\t" + Strings.ByteToStr(this.info);
        	#endif
        	ret += "\n\t<Name>\n\t\t" + Strings.HashToString(this.nameID); // return string
        	ret += "\n\t<Class>\n\t\t" + this.getClass().ToString();
        	ret += "\n\t<Role>\n\t\t" + this.getRole().ToString();
        	ret += "\n\t<Interrupter>\n\t\t" + (this.isInterrupter() ? "Yes" : "No");
        	ret += "\n\t<Off-Tank>\n\t\t" + (this.isOT() ? "Yes" : "No");
        	return ret;
        }
    }

    // Object to hold all names of the receivers of each assignment.
    // constructor receives List<string>[] namesByClass, List<string> admins
    public class AssignmentReceivers
    {
        private List<string> admin_a; // hold name for admins whom should receive all assignments
        private List<string> interrupt_a; // hold name of interrupters whom should receive interrupt assignments
        private List<string> tank_a; // hold name of tanks whom should receive tank assignments
        private List<string> healer_a; // hold name of healers whom should receive healer assignments
        private List<string> ranged_a; // hold name of ranged whom should receive ranged assignments
        private List<string> melee_a; // hold name of melees whom should  receive melee assignments
        private List<string>[] class_a; // class_A[(int) Wow_Class.Class] will output list of all players of that class

        public AssignmentReceivers(List<string>[] namesByClass, List<string> admins) 
        {
            this.admin_a = admins;
            this.class_a = nameByClass;
            // init healers first as tank_a and ranged_a depends on healers.
            this.healer_a = this.initHealer();
            this.rranged_a = this.initRanged();
            this.interrupt_a = this.initInterrupt();
            this.melee_a = this.initMelee();
            this.tank_a = this.initTank();
        }

        // healer = Priests, Druids, Shamans, Admins
        private List<string> initHealer() 
        {
            List<string> healers = new List<string>();
            Wow_Class[] healerClasses = [Wow_Class.Druid, Wow_Class.Priest, Wow_Class.Shaman];
            for (int i=0; i < healerClasses.Length-1; i++) 
            {
                Wow_Class healerClass = healerClasses[i];
                healers.addRange(this.class_a[healerClass]);
            } 
            healers.addRange(this.admin_a);
            return healers;
        }

        // ranged = healer_a, Warlocks, Mages, Hunters,
        private List<string> initRanged() 
        {
            List<string> ranged = new List<string>();
            Wow_Class[] rangedClasses = [Wow_Class.Warlocks, Wow_Class.Mage, Wow_Class.Hunter];
            for (int i=0; i < rangedClasses.Length-1; i++) 
            {
                Wow_Class rangedClass = rangedClasses[i];
                ranged.addRange(this.class_a[rangedClass]);
            } 
            ranged.addRange(this.admin_a);
            ranged.addRange(this.healer_a);
            return ranged;
        }

        // melee = Warriors, Rogues, Druids, Admins
        private List<string> initMelee() 
        {
            List<string> melee = new List<string>();
            Wow_Class[] meleeClasses = [Wow_Class.Warrior, Wow_Class.Rogue, Wow_Class.Druid];
            for (int i=0; i < meleeClasses.Length-1; i++) 
            {
                Wow_Class meleeClass = meleeClasses[i];
                melee.addRange(this.class_a[meleeClass]);
            } 
            melee.addRange(this.admin_a);
            return melee;
        }

        // tanks = Warriors, Druids, Admins, healer_a
        private List<string> initTank() 
        {
            List<string> tanks = new List<string>();
            Wow_Class[] tankClasses = [Wow_Class.Druid, Wow_Class.Warrior];
            for (int i=0; i < tankClasses.Length-1; i++) 
            {
                Wow_Class tankClass = tankClasses[i];
                tanks.addRange(this.class_a[tankClass]);
            } 
            tanks.addRange(this.admin_a);
            tanks.addRange(this.healer_a);
            return tanks;
        }

        // interrupt = Rogues, Warriors, Mages, Shamans, Admins
        private List<string> initInterrupt() 
        {
            List<string> interrupters = new List<string>();
            Wow_Class[] interruptClasses = [Wow_Class.Rogue, Wow_Class.Mage, Wow_Class.Warrior, Wow_Class.Shaman];
            for (int i=0; i < interruptClasses.Length-1; i++) 
            {
                Wow_Class interruptClass = interruptClasses[i];
                interrupters.addRange(this.class_a[interruptClass]);
            } 
            interrupters.addRange(this.admin_a);
            return interrupters;
        }

        public getNamesOfClass(enum wow_class)  
        {
            return this.class_a[wow_class]; 
        }

        public List<string> getTanks() 
        {
            return this.tank_a;
        }

        public List<string> getHealers() 
        {
            return this.healer_a;
        }

        public List<string> getInterrupters() 
        {
            return this.interrupt_a;
        }

        public List<string> getMelees()
        {
            return this.melee_a;
        }
        
        public List<string> getRanged() 
        {
            return this.ranged_a;
        }
    }

    // Object to hold all order stacks.
    // constructor receives Stack<string>[] priorities
    public class Priorities 
    {
        private Stack<string>[] priorities;

        // receives a Stack<string> array. The index of the array should match the enum priority
        public Priorities(Stack<string>[] priorities)  
        {
            this.priorities = priorities;
        }

        public Stack<string> getPriority(Priority priority) 
        {
            return this.priorities[(int) priority];
        }
    }

    /*
    // Object holding the RosterDictionary
    public class RosterDictionary
    {
        private Dictionary<string, Player> roster = new Dictionary<string, Player>();

    }
    */
}