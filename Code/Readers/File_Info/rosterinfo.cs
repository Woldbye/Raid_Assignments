using Utilities;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Enumerator;
using Containers;
using Assignments;

namespace Readers {
  // Constructor receives path to raid_roster.txt, and the table object 
  // will hold index to each of the tables to read, and the name of the tables.
  public class RosterInfo : FileReader, ITextReader<string[]>, ITextInfo<int>
  {
    private string[] rawLines;
    // Holds the name of each table
    private readonly List<string> tableNames;
    // Holds the index to each of the priority tables
    private readonly List<int> indexToPrios;
    /*
     Holds the index to all of the tablethis.getRawLines().
     If tableA is contained at index 2 in tableNames, its index in rawLines will similarily 
     be contained at indexToTables[2]
     TO:DO Convert type to List<int> for simplification.
    */
    private int[] indexToTables;
    private readonly List<int> playerInfoIndexes;
    // The index where all the tables start
    private int startIndex;
    // The index where all the tables end
    private int endIndex;
    private const string ROSTER_TABLE_NAME = "roster";
    private const string INIT_TABLE_NAME = "tables_list";
    public const char FINISH_FLAG = '}'; 
    public const char BEGIN_FLAG = '{';
    public const char NAME_FLAG = '[';
    public const char NAME_END_FLAG = ']';
    public const string START_FLAG = "#START";
    public const string END_FLAG = "#END";
    private int rosterCount;

    public RosterInfo() : base(Roster.PATH) 
    {
      this.playerInfoIndexes = new List<int>();
      this.tableNames = new List<string>();
      this.rawLines = base.readFile(base.getPath()).Split("\n");
      // head is the init table name
      this.tableNames.Add(INIT_TABLE_NAME);
      // we init this as a list since we dont know beforehand how many prio tables are present
      this.indexToPrios = new List<int>(); 
      this.read(this.getRawLines());

      #if (DEBUG)
        Console.WriteLine("Finished initializing <Table>:");
        Console.WriteLine(this.ToString());
      #endif 
    }
    
    // ITextReader method
    public void read(string[] lines)
    {
      // extract tables and send them to their respective handlers
      int i = 0;    
      string aLine = "";
      int j = 0; // table index

      while(true)
      {
        aLine = this.getRawLines()[i];
        if (aLine == null)
        {
          Exceptions.ThrowRoster();
        } else if (aLine.Contains(RosterInfo.START_FLAG)) 
        {
          break;
        }
        i++;
      }
      this.startIndex = i;
      i++;
     
      // loop through entire file, till the end is reached.
      // For each table found, set the index to the Table and extract tableNames if its the first table.
      while (!aLine.Contains(RosterInfo.END_FLAG)) 
      {
        aLine = this.getRawLines()[i];
        if (aLine[0].Equals(RosterInfo.NAME_FLAG)) 
        {
          bool isPrio;
          int indexToTable = -1;
          int endIndex = -1;
          string tableName = this.readTableName(aLine);
          Tuple<bool, int, int> tableInfo = this.readTable(i, this.getRawLines(), tableName); 
          // extract tuple info
          isPrio = tableInfo.Item1; 
          i = tableInfo.Item2; // i is now equal to the line with },
          indexToTable = tableInfo.Item3;
          
          if (tableName.Equals(RosterInfo.INIT_TABLE_NAME)) 
          {
            this.indexToTables = new int[this.tableNames.Count];
          } else if (tableName.Equals(RosterInfo.ROSTER_TABLE_NAME)) {
            this.rosterCount = i - indexToTable;
          } else if (!this.tableNames.Contains(tableName)) {
            Exceptions.ThrowException(String.Format("Invalid name: <{0}>\n\ttableNames doesn't contain the object. tableNames is equal to: <{2}>",
                     tableName, 
                     this.tableNames));
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
      if (this.indexToPrios.Count != AssignmentType.GetNames(typeof(AssignmentType)).Length)
      {
        Exceptions.ThrowRoster();
      }
      this.endIndex = i-1; 
    }

    public int getRosterCount()
    {
      return this.rosterCount;
    }

    public IList<int> getAllInfo()
    {
      return this.indexToTables;
    }

    public int getInfo(int i)
    {
      return this.indexToTables[i];
    }
    // TO:DO CLEANUP
    public int[] getIndexes()
    {
      return this.indexToTables;
    }
    public int getIndex(int i)
    {
      return this.indexToTables[i];
    }
    public List<int> getPrioIndexes()
    {
      return this.indexToPrios;
    }

    public string[] getRawLines()
    {
      return this.rawLines;
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
      if (name.Equals(RosterInfo.INIT_TABLE_NAME)) 
      {
        beginIndex = this.findStartIndex(lines, j);
        j = beginIndex;
        while(true) 
        {
          aLine = lines[j];
          if (aLine == null)
          {
            Exceptions.ThrowRoster();
          } 
          if (aLine[0].Equals(RosterInfo.FINISH_FLAG)) 
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
        } else if (name.Equals(RosterInfo.ROSTER_TABLE_NAME)) 
        {    
          j++;
          // index into the line
          int k = 0;
          aLine = lines[j];
          foreach (char c in aLine)
          {
            if (c.Equals(RosterInfo.NAME_FLAG))
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
            Exceptions.ThrowRoster();
          } 
          if (aLine[0].Equals(RosterInfo.FINISH_FLAG)) 
          {
            break;
          }
          j++;
        }       
      } 
      if (j < 0 | beginIndex < 0)
      {
        Exceptions.ThrowRoster();
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
      while(!aLine.Contains(RosterInfo.BEGIN_FLAG))
      {
        aLine = lines[i];
        i++;
      }
      return i;
    }

    // Checks the format for the beginning two lines of a table
    public bool checkTableStartFormat(string[] fileLines, int index, string tbName) 
    {
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
      ret += String.Format("\n\t<INIT_TABLE_NAME>:\t{0}", RosterInfo.INIT_TABLE_NAME);
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
}
