using Util;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Generics {
    // If alliance replace shaman with paladin
    public enum Wow_Class {
        Druid, // 0
        Hunter, // 1
        Mage, // 2
        Priest, // 3
        Rogue, // 4
        Shaman, // 5
        Warlock, // 6
        Warrior
    }; // 3 bits

    public enum Role {
        Tank, // 0
        Healer, // 1
        Melee, // 2
        Ranged // 3
    }; // 2 bit
	/*
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
    public class Table 
    {
        // TO:DO MAKE GETTERS FOR tableNames, tableIndexes, startIndex and endIndex.
        private readonly List<string> tableNames;
        private readonly int[] indexToTables;
        private readonly string[] rawLines;
        private readonly int startIndex;
        private readonly int endIndex;
        private const string INIT_TABLE_NAME = "tables_list";

        public Table(string path) 
        {
            // TO:DO ALL INITS
            this.tableNames = new List<string>();
            // head is the init table name
            this.tableNames.Add(Table.INIT_TABLE_NAME);

            this.rawLines = File.ReadAllText(path).Split("\n");
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
                    int indexToTable = i;
                    string tableName = this.readTableName(aLine);
                    i = this.readTable(i, this.rawLines, tableName); // i is now equal to the line with },
                    if (tableName.Equals(this.tableNames.First())) 
                    {
                        this.indexToTables = new int[this.tableNames.Count];
                    }
                    this.indexToTables[j] = indexToTable;
                    #if (DEBUG)
                        Console.WriteLine(String.Format("Read table {0}", tableName));
                        if (!this.tableNames.Contains(tableName)) 
                        {
                            Error.Exception(String
                                .Format("Invalid name: <{0}>\n\ttableNames doesn't contain the object. tableNames is equal to: <{2}>"
                                    , tableName, this.tableNames));
                        } 
                    #endif
                    j++;
                } 
                i++; // next table or end.
            }
            endIndex = i-1;
            #if (DEBUG)
                Console.WriteLine("Finished initializing <Table>:");
                Console.WriteLine(this.toString());
            #endif 
        }

        public string[] getRawTableLines() 
        {
            return rawLines;
        }

        public string toString() 
        {
            string ret = "<Table>:";
            ret += String.Format("\n\t<INIT_TABLE_NAME>:\t{0}", Table.INIT_TABLE_NAME);
            ret += String.Format("\n\t<Start index of tables>:\t{0}", startIndex);
            ret += String.Format("\n\t<End index of tables>:\t{0}", endIndex);
            ret += "\n\t<Index to Tables array>:";
            for (int i=0; i < this.indexToTables.Length-1; i++)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", i, this.indexToTables[i]);
            }
            ret += "\n\t<Table names list>:";
            for (int i=0; i < this.tableNames.Count-1; i++)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", i, this.tableNames[i]);
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
        //      Function returns the end index of the table
        private int readTable(int i, string[] lines, string name) 
        {
            int j = i;
            string aLine = "";
            #if (DEBUG)
                Console.WriteLine("----readTable DEBUG info----");
                Console.WriteLine(String.Format("\t\tReceived i: <{0}>", i));
                Console.WriteLine(String.Format("\t\tTrying to read table: <{0}>", name));
            #endif
            if (name.Equals(this.tableNames.First())) 
            {
                j = this.findStartIndex(lines, j);
                while(true) 
                {
                    aLine = lines[j];
                    if (aLine == null)
                    {
                        Error.ThrowRosterError();
                    } 
                    if (aLine.Contains("}")) 
                    {
                        return j;
                    }
                    string subTable = this.readTableName(aLine);
                    #if (DEBUG)
                        Console.WriteLine(String.Format("Adding table named <{0}> to List<string> tableNames", subTable));
                    #endif
                    this.tableNames.Add(subTable);
                    j++;
                }  
            } else {
                // reading of any other table simply involves finding the end indice of the table
                while(!aLine.Contains("}"))
                {
                    // start index doesnt matter here as we dont need the info
                    aLine = lines[j];
                    if (aLine == null)
                    {
                        Error.ThrowRosterError();
                    } 
                    if (aLine.Contains("}")) 
                    {
                        return j;
                    }
                }
                //switch (aLine)                
            } // this.tableStartIndexes and 
            return Error.UNKNOWN_ERR;
        }

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
    // Should init all the lists, after only receiving class_a and admin_a
    public class AssignmentReceivers
    {
        private List<string> admin_a; // hold name for admins whom should receive all assignments
        private List<string> interrupt_a; // hold name of interrupters whom should receive interrupt assignments
        private List<string> tank_a; // hold name of tanks whom should receive tank assignments
        private List<string> healer_a; // hold name of healers whom should receive healer assignments
        private List<string> ranged_a; // hold name of ranged whom should receive ranged assignments
        private List<string> melee_a; // hold name of melees whom should  receive melee assignments
        private List<string>[] class_a; // class_A[(int) Wow_Class.Class] will output list of all players of that class
    }

    // Receives orders in init.
    // Object to hold all order stacks.
    public class Orders 
    {
        private Stack<string> tank_o; // tank order, remember first in, last out -> bottom of list gets pushed first
        private Stack<string> healer_o; // healer order
        private Stack<string> interrupt_o; // interrupt order
        private Stack<string> kiter_o; // kite order
    }

    public class RosterDictionary
    {
        private Dictionary<string, Player> roster = new Dictionary<string, Player>();
    }
}