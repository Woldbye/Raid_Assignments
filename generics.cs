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

    public enum Faction
    {
        Tank, // 0
        Hunter, // 1
        Druid, // 2
        Warrior, // 3
        Mage, // 4
        Shaman, // 5
        Rogue, // 6
        Warlock, // 7
        Priest // 8
    }; // 4 bits

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

    public enum Date
    {
        Error = -1,
        Calender = 0,
        Clock = 1
    };
    
    public class SignUp
    {
        // total count of signed up players.
        private int count;
        // count for each role
        private readonly int[] rolesCount;
        // the sign up files by lines.
        private readonly string[] rawLines;
        // Index to each of the "factions".
        private readonly int[] indexToFaction;
        // Any line that terminates with ":", corresponds to a new group that needs to be read.
        public const char NEW_FLAG = ':';
        private int GMTOffset;
        // date of event.
        private readonly DateTime date;
        // headline name for each of the info regarding dates.
        // calender is date, clock is time.
        public readonly string[] DATE_NAMES = {"CMcalendar", "CMclock"};

        public static int MAX_FACTION_COUNT = Faction.GetNames(typeof(Faction)).Length;
        // Use Role lookup to find info
        public SignUp(string path)
        {
            this.rolesCount = new int[Role.GetNames(typeof(Role)).Length];
            this.rawLines = File.ReadAllText(path).Split("\n");
            int i = this.setCount(this.rawLines);
            // Next step is rolesCount and date
            Tuple<int[], DateTime, int> roleDateExtract = this.extractRolesCountNDate(this.rawLines, i);
            this.rolesCount = roleDateExtract.Item1;
            this.date = roleDateExtract.Item2;
            Console.WriteLine(String.Format("Succesfully set date to {0}", date.ToString()));
            i = roleDateExtract.Item3;

            this.indexToFaction = this.extractFactionIndexes(this.rawLines, i);
            #if (DEBUG)
                Console.WriteLine("----Finished init of SignUp----");
                Console.WriteLine(String.Format("{0}", this.ToString()));
            #endif
        }


        public int[] extractFactionIndexes(string[] lines, int start)
        {
            int i = start;  
            int[] ret = new int[SignUp.MAX_FACTION_COUNT];
            #if (DEBUG)
                int counter = 0;
                Console.WriteLine("----Extract Faction Indexes debug info----");
                Console.WriteLine(String.Format("\t\tReceived i: <{0}>", start));
                Console.WriteLine(String.Format("\t\tMaximum i of lines is: <{0}>", lines.Length-1));
                Console.WriteLine(String.Format("\t\tLine at start index equals:\n\t\t\t{0}", lines[start]));
            #endif

            while (i < lines.Length-1)
            {
                Tuple<int, int, int> factionInfo = this.nextFactionIndex(lines, i);
                int faction = factionInfo.Item3;
                i = factionInfo.Item2;
                int factionStart = factionInfo.Item1;
                #if (DEBUG)
                    counter++;
                    Console.WriteLine(String.Format("\t\tRead new faction: <{0}>", Strings.FACTION_TO_STR[faction]));
                    Console.WriteLine("\t\t\t\t\t<Indexes>:");
                    Console.WriteLine(String.Format("\t\t\t\t\t\t<Start>: {0}", factionStart));
                    Console.WriteLine(String.Format("\t\t\t\t\t\t<End>: {0}", i));
                #endif
                ret[faction] = factionStart;
            }

            return ret;
        }

        // Here index would correspond to index to the first line.
        // startIndex is index to ":Tank: 9 Vogn/Slæde"
        // endIndex is index to ":Tank: 27 Yrotaris"
        // extractIndexToFaction(string lines[], int index)
        //     returns Tuple<startIndex, endIndex, faction>   
        private Tuple<int, int, int> nextFactionIndex(string[] lines, int start)
        {
            int startIndex = -1;
            int endIndex = -1;
            int faction = -1;
            int i = start;

            while (true)
            {
                if (i >= lines.Length)
                {
                    Error.ThrowSignUpError();
                }
                string line = lines[i];
                string headline = this.extractHeadline(line); 
                // array index
                int j = Array.FindIndex(Strings.FACTION_TO_STR, 
                        s => s.ToLower().Equals(headline));
                if (j != -1)
                {
                    faction = j;
                    startIndex = i+1;
                    while (true)
                    {
                        if (i == lines.Length)
                        {
                            endIndex = i-1;
                            break;
                        }
                        line = lines[i];
                        if (line.Length < 2)
                        {
                            endIndex = i;
                            break;
                        }
                        i++;
                    }
                    break;
                } else {
                    i++;
                }
            }

            return Tuple.Create(startIndex, endIndex, faction);
        }

        // Extracts the first word in a line
        // for a line containing ":{info}:". "info" will be returned where no c in info is whitespace or upper
        // if empty string is returned something went wrong
        public string extractHeadline(string line)
        {
            int start = 0;
            int end = 0;

            string ret = "";
            // Table name in format [tableName] so we just trim (also converts to lower-case)
            // and remove the first and last letter.
            if (line.Length > 0)
            {
                int count = 0;
                for (int i=0; i < line.Length; i++)
                {
                    char c = line[i];

                    if (c == NEW_FLAG)
                    {
                        count++;
                        // if start we increase, if end we decrease
                        if (count <= 1)
                        {
                            // if line is ":" return empty or we set start index.
                            start = i+1;
                        } else {
                            end = i-1;
                            break;
                        }
                    }
                }
                if (count > 1)
                {
                    ret = Strings.Trim(line.Substring(start, (end - start)+1));    
                }
            } 

            return ret;
        }

        // Input: 
        //      string[]: the discord_signup file in a string[]
        //      int: startIndex of the search.
        // Output:
        //      int[]: RolesCount array
        //      DateTime: the set time of the event
        //      int: End index of search
        // rolesCount arr
        // Will return index to last line + 1, 
        private Tuple<int[], DateTime, int> extractRolesCountNDate(string[] lines, int startIndex)
        {  
            int ROLE_COUNT = Role.GetNames(typeof(Role)).Length;
            int i = startIndex;
            int[] ret = new int[ROLE_COUNT];
            const int DATE_ARR_SIZE = 4;
            int[] calender = new int[DATE_ARR_SIZE];
            int[] clock = new int[DATE_ARR_SIZE];
            int count = 0;
            int[] roleOrder = {(int) Role.Tank, (int) Role.Melee, 
                               (int) Role.Ranged, (int) Role.Healer};
            
            while (count <= 3) 
            {
                int role = roleOrder[count];
                string line = lines[i];
                string roleStr = Strings.ROLE_TO_STR[role]; 
                // if line_array find index can find rolestr 
                if (line.Contains(roleStr))
                {
                    // role is int value of string so we know already based on match what string it is.
                    // it can be tanks, ranged or healers. Ranged and healers should be handled the same,
                    // tanks and melees together
                    if (role == (int) Role.Tank) 
                    {
                        Tuple<int, int> numNIndex = Strings.FindIntNIndex(line);
                        ret[(int) Role.Tank] = numNIndex.Item1;
                        // Max int size is 2, so we simply do +2 and we will always
                        //  move past first integer.
                        int lineIndex = numNIndex.Item2 + 2;
                        // Read melee now
                        numNIndex = Strings.FindIntNIndex(line.Substring(lineIndex, line.Length-lineIndex));
                        ret[(int) Role.Melee] = numNIndex.Item1;
                        count++;
                    } else if (role != (int) Role.Melee)
                    {
                        // Ranged, Healer
                        int num = Strings.FindInt(line);
                        ret[role] = num;  
                    }
                    count++;  
                } else if (line.Contains(Strings.DATE_TO_STR[(int) Date.Calender])) {
                    calender = this.readDate(line);
                    if (calender[0] == (int) Date.Error) 
                    {
                        Error.ThrowSignUpError();
                    }
                } else if (line.Contains(Strings.DATE_TO_STR[(int) Date.Clock])) {
                    clock = this.readDate(line);
                    if (clock[0] == (int) Date.Error) 
                    {
                        Error.ThrowSignUpError();
                    }
                }
                i++;
            }
            // year, month, day, hour, minute, second);
            DateTime date = new DateTime(calender[1], calender[2], calender[3],
                                         clock[1], clock[2], 0);
            this.GMTOffset = clock[3];

            return Tuple.Create(ret, date, i);
        }

        // Reads the lines and sets this.count accordingly. if Error it thwos an exception
        // Will return the index to the :signups: line.
        private int setCount(string[] lines)
        {
            int i = 0;
            string signUpFlag = "signups";

            while (true)
            {
                if (i >= lines.Length)
                {
                    throw new ArgumentOutOfRangeException(
                       String.Format("The length of the file is {0} and the index is {1}", lines.Length, i));
                }
                string line = lines[i];
                string headline = this.extractHeadline(line);
                /*
                if (headline.Length <= 0)
                {
                    Console.WriteLine("-" + line + "-");
                    Error.ThrowSignUpError();
                }
                */
                // Console.WriteLine("Made it past headline.length check");
                if (signUpFlag.Equals(headline))
                {
                    int imCount = Strings.ConvertToInt(line.Substring(line.Length-2, 2));
                    if (imCount == Strings.ERROR)
                    {
                        Error.ThrowSignUpError();
                    } else {
                        #if (DEBUG)
                            Console.WriteLine("Succesfully set count to " + imCount);
                        #endif 
                        this.count = imCount;
                    }
                    break;
                }
                i++;
            }

            return i;
        }


        // Returns int[] containing info about the read date
        // int[0] is ternary value, 
        //      if (int[0] == -1) -> error
        //      if (int[0] == 0) -> calender
        //      if (int[0] == 1) -> clock
        private int[] readDate(string line)
        {
            // Either its [type, day, month, year] or [type, hour, min, gmt+]
            const int MAX_INFO_SIZE = 4;
            
            int[] ret = new int[MAX_INFO_SIZE];

            string line_cp = line;
            if (line[0].Equals(NEW_FLAG))
            {
                line_cp = line_cp.Substring(1, line_cp.Length-1);
            }
            // start at the shortest type i.
            int i = Strings.DATE_TO_STR[(int) Date.Clock].Length;
            // find first number
            while (true)
            {
                if (line_cp.Length <= i)
                {
                    throw new ArgumentOutOfRangeException(
                        String.Format("The length of the line is {0} and the index is {1}", line_cp.Length, i));
                }
                char c = line_cp[i];
                if (c == null)
                {
                    ret[0] = (int) Date.Error;
                    return ret;
                } else if (Char.IsNumber(c)) 
                {
                    break;
                }
                i++;
            }
            // Contains would be safer, but this is much faster :)
            if (Strings.DATE_TO_STR[(int) Date.Calender].Equals(line_cp.Substring(0, Strings.DATE_TO_STR[(int) Date.Calender].Length)))
            {
                // handle CMcalendar: 19-08-2020 type string
                int day = Strings.ConvertToInt(line_cp.Substring(i, 2));
                int month = Strings.ConvertToInt(line_cp.Substring(i+3, 2));
                int year = Strings.ConvertToInt(line_cp.Substring(i+6, 4));
                // Type
                ret[0] = (int) Date.Calender;
                ret[1] = year;
                ret[2] = month;
                ret[3] = day;
            } else if (Strings.DATE_TO_STR[(int) Date.Clock].Equals(line_cp.Substring(0, Strings.DATE_TO_STR[(int) Date.Clock].Length)))
            {
                // handle CMclock: 18:45 GMT +2  
                ret[0] = (int) Date.Clock;  
                int hour = Strings.ConvertToInt(line_cp.Substring(i, 2));
                int min = Strings.ConvertToInt(line_cp.Substring(i+3, 2));
                // last 3 symbols cause GMT can be two cipher
                int GMToffset = Strings.ConvertToInt(line_cp.Substring(line_cp.Length-3, 3));
                // Type
                ret[0] = (int) Date.Clock;
                ret[1] = hour;
                ret[2] = min;
                ret[3] = GMToffset;
            } else {
                ret[0] = (int) Date.Error;
            }

            if (ret[1] == Strings.ERROR | ret[2] == Strings.ERROR | ret[3] == Strings.ERROR)
            {
                ret[0] = (int) Date.Error;
            }
            return ret;
        }

        public override string ToString()
        {
            string ret = "<SignUp>:";
            ret += "\n\t<Date>";
            ret += String.Format("\n\t\t{0} GMT {1}{2}", this.date.ToString(), (this.GMTOffset >= 0) ? "+" : "-", this.GMTOffset);
            ret += "\n\t<Total number of signed players>:";
            ret += String.Format("\n\t\t{0}", this.count);
            ret += "\n\t<Total number of each role>";
            for (int i=0; i < this.rolesCount.Length; i++)
            {
                string role = Strings.ROLE_TO_STR[i];
                ret += String.Format("\n\t\t<{0} count>", role);                
                ret += String.Format("\n\t\t\t{0}", this.rolesCount[i]);
            }
            ret += "\n\t<Indexes to sign up groups>";
            for (int i=0; i < this.indexToFaction.Length; i++) 
            {
                string faction = Strings.FACTION_TO_STR[i];
                ret += String.Format("\n\t\t<{0}>", faction);
                ret += String.Format("\n\t\t\t{0}", indexToFaction[i]);
                #if (DEBUG)
                    ret += String.Format("\n\t\t\tLine at index:");
                    ret += String.Format("\n\t\t\t\t{0}", this.rawLines[indexToFaction[i]]);
                #endif
            }
         
            return ret;
        }

        public string[] getRawLines()
        {
            return this.rawLines;
        }

        public int getCount()
        {
            return this.count;
        }

        public int getIndexToFaction(int faction) 
        {
            return this.indexToFaction[faction];
        }
    }
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
        private readonly List<int> playerInfoIndexes;
        /*
         The path file as RawLines. Each line in the array corresponds to a line in the file such that 1st line in file is at index 0.
        */
        private readonly string[] rawLines;
        // The index where all the tables start
        private readonly int startIndex;
        // The index where all the tables end
        private readonly int endIndex;
        private const string ROSTER_TABLE_NAME = "roster";
        private const string INIT_TABLE_NAME = "tables_list";
        public const char FINISH_FLAG = '}'; 
        public const char BEGIN_FLAG = '{';
        public const char NAME_FLAG = '[';
        public const char NAME_END_FLAG = ']';
        public const string START_FLAG = "#START";
        public const string END_FLAG = "#END";
        private readonly int rosterCount;

        public Table(string path) 
        {
            this.playerInfoIndexes = new List<int>();
            this.tableNames = new List<string>();
            this.tableNames.Add(INIT_TABLE_NAME);
            // head is the init table name
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
                } else if (aLine.Contains(Table.START_FLAG)) 
                {
                    break;
                }
                i++;
            }
            this.startIndex = i;
            i++;
            int j = 0; // table index
           
            // loop through entire file, till the end is reached.
            // For each table found, set the index to the Table and extract tableNames if its the first table.
            while (!aLine.Contains(Table.END_FLAG)) 
            {
                aLine = this.rawLines[i];
                if (aLine[0].Equals(Table.NAME_FLAG)) 
                {
                    bool isPrio;
                    int indexToTable = -1;
                    int endIndex = -1;

                    string tableName = this.readTableName(aLine);
                    Tuple<bool, int, int> tableInfo = this.readTable(i, this.rawLines, tableName); 
                    // extract tuple info
                    isPrio = tableInfo.Item1; 
                    i = tableInfo.Item2; // i is now equal to the line with },
                    indexToTable = tableInfo.Item3;
                    
                    if (tableName.Equals(Table.INIT_TABLE_NAME)) 
                    {
                        this.indexToTables = new int[this.tableNames.Count];
                    } else if (tableName.Equals(Table.ROSTER_TABLE_NAME)) {
                        this.rosterCount = i - indexToTable;
                    } else if (!this.tableNames.Contains(tableName)) {
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
                        Console.WriteLine(String.Format("Finished read of table {0}\n", tableName));
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
                Console.WriteLine(this.ToString());
            #endif 
        }
        public int getRosterCount()
        {
            return this.rosterCount;
        }

        public int[] getTableIndexes()
        {
            return this.indexToTables;
        }

        public int getTableIndex(int i)
        {
            return this.indexToTables[i];
        }

        public List<int> getPrioIndexes()
        {
            return this.indexToPrios;
        }

        public List<int> getPlayerInfoIndexes()
        {
            return this.playerInfoIndexes;
        }

        // returns -1 if it cant be found
        public int getTableIndexByName(string name)
        {
            int index = this.tableNames.FindIndex(x => x.Contains(name.ToLower()));
            if (index == -1) 
            {
                return index;
            } else {
                return this.indexToTables[index];
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
        // TO:DO SIMPLIFY IF STATEMENTS TO AVOID CODE DUPLICATION
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
                Console.WriteLine(String.Format("\t\tLine at start index equals:\n\t\t\t{0}", lines[i]));
            #endif
            if (name.Equals(Table.INIT_TABLE_NAME)) 
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
                    if (aLine[0].Equals(Table.FINISH_FLAG)) 
                    {
                        break;
                    }
                    string subTable = Strings.Trim(aLine);
                    this.tableNames.Add(subTable);
                    j++;
                }  
            } else {
                if(Priorities.PRIO_STR.Any(s => s.Contains(name))) 
                {
                    isPrio = true;
                // If its the roster table set this.playerInfoIndexes;
                } else if (name.Equals(Table.ROSTER_TABLE_NAME)) 
                {
                    
                    j++;
                    // index into the line
                    int k = 0;
                    aLine = lines[j];
                    foreach (char c in aLine)
                    {
                        if (c.Equals(Table.NAME_FLAG))
                        {
                            // +1 cuz we want the index to the first letter of the word.
                            this.playerInfoIndexes.Add(k);
                        }
                        k++;
                    }
                }
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
                    if (aLine[0].Equals(Table.FINISH_FLAG)) 
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
            #if (DEBUG)
                Console.WriteLine(String.Format("\t\tEnd index of table: <{0}>", j));
                Console.WriteLine(String.Format("\t\tLine at end index equals:\n\t\t\t{0}", lines[j]));
                Console.WriteLine(String.Format("\t\tBegin index of table: <{0}>", beginIndex));
                Console.WriteLine(String.Format("\t\tLine at begin index equals:\n\t\t\t{0}", lines[beginIndex]));
                Console.WriteLine(String.Format("\t\tIs it a prio table?:\n\t\t\t{0}", isPrio));
            #endif
            return Tuple.Create(isPrio, j, beginIndex);
        }

        // Input: receive the file lines and the current index. 
        // Output: it returns the start index of the table content.
        private int findStartIndex(string[] lines, int index) 
        {
            string aLine = "";       
            int i = index;
            while(!aLine.Contains(Table.BEGIN_FLAG))
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

        public override string ToString() 
        {
            string ret = "<Table>:";
            ret += String.Format("\n\t<INIT_TABLE_NAME>:\t{0}", Table.INIT_TABLE_NAME);
            ret += String.Format("\n\t<Start index of tables>:\t{0}", startIndex);
            ret += String.Format("\n\t<End index of tables>:\t{0}", endIndex);
            ret += String.Format("\n\t<Roster Count>:\t{0}", this.rosterCount);
            ret += "\n\t<TableNames and their indexes>:";
            ret += String.Format("\n\t\tNum:\tName\t\tIndex");
            for (int i=0; i < this.indexToTables.Length; i++)
            {
                if (i==1)
                {
                    ret += String.Format("\n\t\t{0}:\t{1}:\t\t{2}", i, this.tableNames[i], this.indexToTables[i]);    
                } else {
                    ret += String.Format("\n\t\t{0}:\t{1}:\t{2}", i, this.tableNames[i], this.indexToTables[i]);
                }
            }
            ret += "\n\t<Index to Priority Table>:";
            int j = 0;
            foreach (int prioI in this.indexToPrios)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, prioI);
                j++;
            }
            ret += "\n\t<Index to Player Info>:";
            j = 0;
            foreach (int infoIndex in this.playerInfoIndexes)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, infoIndex);
                j++;
            }
            ret += "\n";
            return ret;
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
            |0|*0*{0}[0 00](00): (Role) [class] {back-up tank} *interrupter* |admin|
    */
    public class Player
    { 
    	private Byte[] nameID;
    	private byte info;
    	// Constructor
    	// Int = interrupter
    	public Player(string name, Wow_Class class_, Role role_, bool isOT, bool isInt, bool isAdmin) 
    	{	
    		this.nameID = Strings.Hash(name);
    		this.info = (byte) 0; 
            this.setAdmin(isAdmin);
            this.setOT(isOT);
            this.setInterrupt(isInt);
    		this.setClass(class_);
    		this.setRole(role_);
			#if (DEBUG)
				Console.WriteLine("---Player init complete, printing info:---");
    			Console.WriteLine(this.ToString());
    		#endif   		
    	}

        public string getName()
        {
            return Strings.HashToString(this.nameID);
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

        public bool isAdmin()
        {
            return Convert.ToBoolean(this.info & 1);
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

        public void setAdmin(bool admin) 
        {
            this.info = ByteOP.modifyBit(this.info, 0, admin);
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
            ret += "\n\t<Admin>\n\t\t" + (this.isAdmin() ? "Yes" : "No");
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
            this.class_a = namesByClass;
            // init healers first as tank_a and ranged_a depends on healers.
            this.healer_a = this.initHealer();
            this.ranged_a = this.initRanged();
            this.interrupt_a = this.initInterrupt();
            this.melee_a = this.initMelee();
            this.tank_a = this.initTank();
            #if (DEBUG)
                Console.WriteLine("Finished Init of AssignmentReceivers");
                Console.WriteLine(this.ToString());
            #endif
        }

        private string assignment_To_String(string type)
        {
            string ret = "{p:";

            switch (type)
            {
                case "interrupt":
                    foreach(string interrupter in this.interrupt_a)
                    {
                        ret += interrupter;
                        if (!(interrupter.Equals(this.interrupt_a.Last())))
                        {
                            ret += ","; 
                        } 
                    }
                    break;
                case "tank":
                    foreach(string tank in this.tank_a)
                    {
                        ret += tank;
                        if (!(tank.Equals(this.tank_a.Last())))
                        {
                            ret += ","; 
                        } 
                    }
                    break;
                case "healer":
                    foreach(string healer in this.healer_a)
                    {
                        ret += healer;
                        if (!(healer.Equals(this.healer_a.Last())))
                        {
                            ret += ","; 
                        } 
                    }
                    break;
                case "melee":
                    foreach(string melee in this.melee_a)
                    {
                        ret += melee;
                        if (!(melee.Equals(this.melee_a.Last())))
                        {
                            ret += ","; 
                        }
                    }
                    break;
                case "ranged":
                    foreach(string ranged in this.ranged_a)
                    {
                        ret += ranged;
                        if (!(ranged.Equals(this.ranged_a.Last())))
                        {
                            ret += ","; 
                        } 
                    }
                    break;
                // classes
                case "druid":
                    List<string> druids = this.class_a[(int) Wow_Class.Druid];
                    foreach(string druid in druids)
                    {
                        ret += druid;
                        if (!(druid.Equals(druids.Last())))
                        {
                            ret += ","; 
                        }
                    }
                    break;
                case "hunter":
                    List<string> hunters = this.class_a[(int) Wow_Class.Hunter];
                    foreach(string hunter in hunters)
                    {
                        ret += hunter;
                        if (!(hunter.Equals(hunters.Last())))
                        {
                            ret += ","; 
                        }
                    }
                    break;
                case "mage":
                    List<string> mages = this.class_a[(int) Wow_Class.Mage];
                    foreach(string mage in mages)
                    {
                        ret += mage;
                        if (!(mage.Equals(mages.Last())))
                        {
                            ret += ","; 
                        } 
                    }
                    break;
                case "priest":
                    List<string> priests = this.class_a[(int) Wow_Class.Priest];
                    foreach(string priest in priests)
                    {
                        ret += priest;
                        if (!(priest.Equals(priests.Last())))
                        {
                            ret += ","; 
                        }
                    }
                    break;
                case "rogue":
                    List<string> rogues = this.class_a[(int) Wow_Class.Rogue];
                    foreach(string rogue in rogues)
                    {
                        ret += rogue;
                        if (!(rogue.Equals(rogues.Last())))
                        {
                            ret += ","; 
                        } 
                    }
                    break;
                case "shaman":
                    List<string> shamans = this.class_a[(int) Wow_Class.Shaman];
                    foreach(string shaman in shamans)
                    {
                        ret += shaman;
                        if (!(shaman.Equals(shamans.Last())))
                        {
                            ret += ","; 
                        } 
                    }
                    break;
                case "warlock":
                    List<string> warlocks = this.class_a[(int) Wow_Class.Warlock];
                    foreach(string warlock in warlocks)
                    {
                        ret += warlock;
                        if (!(warlock.Equals(warlocks.Last())))
                        {
                            ret += ","; 
                        }
                    }
                    break;
                case "warrior":
                    List<string> warriors = this.class_a[(int) Wow_Class.Warrior];
                    foreach(string warrior in warriors)
                    {
                        ret += warrior;
                        if (!(warrior.Equals(warriors.Last())))
                        {
                            ret += ","; 
                        }
                    }
                    break;
                case "admin":
                    foreach(string admin in this.admin_a)
                    {
                        ret += admin;
                        if (!(admin.Equals(this.admin_a.Last())))
                        {
                            ret += ","; 
                        }
                    }
                    break;
                default:
                    throw new ArgumentException(String.Format("Argument {0} is invalid", type), "argument");
            }
            ret += "}";
            return ret;
        }

        /*
        Receives a format string containing the class that needs assignments and outputs the corresponding exorsus
        string that prints the assignments to those.
        The end of the assignment string should be prepended by {/p}
        List of format strings:
            1: // for each role
                {healer}
                {/p}
            2: // for each class
                {rogue}
                {/p}
            3:  
                {admin}
                {/p}
        Also supports + and - OPs. 
            1:  // will add all healer_a and tank_a
                {healer + tank}
                {/p}
            2: // will subtract all healer_a from tank_a and output any remaining
                {tank_a - healer_a}
                {/p}
        */ 
        public string ToExorsus(string msgType, string[] msg)
        {
            string ret = "";
            ret += this.assignment_To_String(msgType);
            ret += "\n";
            foreach (string line in msg)
            {
                ret += line;
                ret += "\n";
            }
            ret += "{/p}";
            return ret;
        }

        // healer = Priests, Druids, Shamans, Admins
        private List<string> initHealer() 
        {
            List<string> healers = new List<string>();
            int[] healerClasses = {(int) Wow_Class.Druid, (int) Wow_Class.Priest, (int) Wow_Class.Shaman};
            for (int i=0; i < healerClasses.Length; i++) 
            {
                int healerClass = healerClasses[i];
                healers.AddRange(this.class_a[healerClass]);
            } 
            healers.AddRange(this.admin_a);
            return healers.Distinct().ToList();
        }

        // ranged = healer_a, Warlocks, Mages, Hunters,
        private List<string> initRanged() 
        {
            List<string> ranged = new List<string>();
            int[] rangedClasses = { (int) Wow_Class.Warlock, (int) Wow_Class.Mage, (int) Wow_Class.Hunter};
            for (int i=0; i < rangedClasses.Length; i++) 
            {
                int rangedClass = rangedClasses[i];
                ranged.AddRange(this.class_a[rangedClass]);
            } 
            ranged.AddRange(this.admin_a);
            ranged.AddRange(this.healer_a);
            return ranged.Distinct().ToList();
        }

        // melee = Warriors, Rogues, Druids, Admins
        private List<string> initMelee() 
        {
            List<string> melee = new List<string>();
            int[] meleeClasses = { (int) Wow_Class.Warrior, (int) Wow_Class.Rogue, (int) Wow_Class.Druid};
            for (int i=0; i < meleeClasses.Length; i++) 
            {
                int meleeClass = meleeClasses[i];
                melee.AddRange(this.class_a[meleeClass]);
            } 
            melee.AddRange(this.admin_a);
            return melee.Distinct().ToList();
        }

        // tanks = Warriors, Druids, Admins, healer_a
        private List<string> initTank() 
        {
            List<string> tanks = new List<string>();
            int[] tankClasses = { (int) Wow_Class.Druid, (int) Wow_Class.Warrior};
            for (int i=0; i < tankClasses.Length; i++) 
            {
                int tankClass = tankClasses[i];
                tanks.AddRange(this.class_a[tankClass]);
            } 
            tanks.AddRange(this.admin_a);
            tanks.AddRange(this.healer_a);
            return tanks.Distinct().ToList();
        }

        // interrupt = Rogues, Warriors, Mages, Shamans, Admins
        private List<string> initInterrupt() 
        {
            List<string> interrupters = new List<string>();
            int[] interruptClasses = { (int) Wow_Class.Rogue, (int) Wow_Class.Mage, (int) Wow_Class.Warrior, (int) Wow_Class.Shaman};
            for (int i=0; i < interruptClasses.Length; i++) 
            {
                int interruptClass = interruptClasses[i];
                interrupters.AddRange(this.class_a[interruptClass]);
            } 
            interrupters.AddRange(this.admin_a);
            return interrupters.Distinct().ToList();
        }

        public List<string> getNamesOfClass(int wow_class)  
        {
            return this.class_a[wow_class]; 
        }

        public List<string> getNamesOfClass(Wow_Class wow_class)  
        {
            return this.class_a[(int) wow_class]; 
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

        public override string ToString()
        {
            string ret = "<AssignmentReceivers>:";
            ret += "\n\t<Melee>:";
            int j = 0;
            foreach (string melee in this.melee_a)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, melee);
                j++;
            }
            ret += "\n\t<Ranged>:";
            j = 0;
            foreach (string ranged in this.ranged_a)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, ranged);
                j++;
            }
            ret += "\n\t<Interrupters>:";
            j = 0;
            foreach (string interrupter in this.interrupt_a)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, interrupter);
                j++;
            }
            ret += "\n\t<Healers>:";
            j = 0;
            foreach (string healer in this.healer_a)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, healer);
                j++;
            }
            ret += "\n\t<Tanks>:";
            j = 0;
            foreach (string tank in this.tank_a)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, tank);
                j++;
            }
            ret += "\n\t<Admins>:";
            j = 0;
            foreach (string admin in this.admin_a)
            {
                ret += String.Format("\n\t\t{0}:\t{1}", j+1, admin);
                j++;
            }
            ret += "\n\t<Players by class>:";
            j = 0;
            foreach (List<string> class_obj in this.class_a)
            {
                ret += String.Format("\n\t\t<{0}>", Strings.CLASS_TO_STR[j]);
                int k = 0;
                foreach (string name in class_obj)
                {
                    ret += String.Format("\n\t\t\t{0}:\t{1}", k+1, name);
                    k++;
                }
                j++;
            }
            return ret;
        }
    }

    // Object to hold all order stacks.
    // constructor receives Stack<string>[] priorities
    public class Priorities 
    {
        private Stack<string>[] priorities;
        // Should match the names of the priority tables.
        public static string PRIO_FLAG = "prio";
        public static string[] PRIO_STR = { "tank_" + PRIO_FLAG, "healer_" + PRIO_FLAG, "interrupt_" + PRIO_FLAG, "kiter_" + PRIO_FLAG};
 
        // receives a Stack<string> array. The index of the array should match the enum priority
        public Priorities(Stack<string>[] priorities)  
        {
            this.priorities = priorities;
            #if (DEBUG)
                Console.WriteLine(String.Format("Finished initializing {0}", typeof(Priorities).Name));
                Console.WriteLine(this.ToString());
            #endif
        }

        public Stack<string> getPriority(Priority priority) 
        {
            return this.priorities[(int) priority];
        }

        public override string ToString()
        {
            string ret = "<Priorities>:";
            for (int i=0; i < this.priorities.Length; i++) 
            {
                Stack<string> prio_stack = this.priorities[i];
                ret += String.Format("\n\t<{0}>:\n", PRIO_STR[i]);
                while (prio_stack.Count > 0)
                {
                    ret += String.Format("\t\t{0}\n", prio_stack.Pop());
                }
            }  
            return ret; 
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