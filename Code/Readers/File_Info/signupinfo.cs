using Utilities;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Enumerator;
using Utilities.WorldOfWarcraft;

namespace Readers
{
  // TO:DO Rewrite thsi class.. its a fucking mess
  public class SignUpInfo : FileReader, ITextReader<string[]>, ITextInfo<int>
  {
    private string[] rawLines;
    // total count of signed up players.
    private int count;
    // count for each role
    private int[] rolesCount;
    // Index to each of the "factions".
    private int[] indexToFaction;
    // Any line that terminates with ":", corresponds to a new group that needs to be read.
    public const char NEW_FLAG = ':';
    private int GMTOffset;
    // date of event.
    private DateTime date;
    // headline name for each of the info regarding dates.
    // Maximum amount of factions
    public static int MAX_FACTION_COUNT = SignUpType.GetNames(typeof(SignUpType)).Length;
    // Use Role lookup to find info
    public SignUpInfo() : base(SignUp.PATH)
    {
      #if (DEBUG)
        Console.WriteLine("---Starting SignUp init---");
      #endif
      this.rawLines = base.readFile(SignUp.PATH).Split("\n");
      this.rolesCount = new int[Wow.Role.GetNames(typeof(Wow.Role)).Length];
      this.read(this.getRawLines());
      #if (DEBUG)
        Console.WriteLine("----Finished init of SignUp----");
        Console.WriteLine(String.Format("{0}", this.ToString()));
      #endif
    }

    // set rolesCount, date, indexToFaction
    public void read(string[] lines)
    {
      int i = this.setCount(lines);
      Tuple<int[], DateTime, int> roleDateExtract = this.extractRolesCountNDate(this.getRawLines(), i);
      this.rolesCount = roleDateExtract.Item1;
      this.date = roleDateExtract.Item2;
      #if (DEBUG)
        Console.WriteLine(String.Format("Succesfully set date to {0}", date.ToString()));
      #endif
      i = roleDateExtract.Item3;
      this.indexToFaction = this.extractFactionIndexes(this.getRawLines(), i);
    }

    public int[] extractFactionIndexes(string[] lines, int start)
    {
      int i = start;  
      int[] ret = new int[SignUpInfo.MAX_FACTION_COUNT];
      #if (DEBUG)
        int counter = 0;
        Console.WriteLine("----Extract Faction Indexes debug info----");
        Console.WriteLine(String.Format("\t\tReceived i: <{0}>", start));
        Console.WriteLine(String.Format("\t\tMaximum i of lines is: <{0}>", lines.Length-1));
        Console.WriteLine(String.Format("\t\tLine at start index equals:\n\t\t\t{0}", lines[start]));
      #endif
      while (i < lines.Length-1)
      {
        Tuple<int, int, int> signUpTypeInfo = this.nextSignUpTypeIndex(lines, i);
        int signUpType = signUpTypeInfo.Item3;
        i = signUpTypeInfo.Item2;
        int startIndex = signUpTypeInfo.Item1;
        #if (DEBUG)
          counter++;
          Console.WriteLine(String.Format("\t\tRead new faction: <{0}>", SignUp.TYPE_TO_STR[signUpType]));
          Console.WriteLine("\t\t\t\t\t<Indexes>:");
          Console.WriteLine(String.Format("\t\t\t\t\t\t<Start>: {0}", startIndex));
          Console.WriteLine(String.Format("\t\t\t\t\t\t<End>: {0}", i));
        #endif
        ret[signUpType] = startIndex;
      }

      return ret;
    }

    // Here index would correspond to index to the first line.
    // startIndex is index to ":Tank: 9 Vogn/Slæde"
    // endIndex is index to ":Tank: 27 Yrotaris"
    // extractIndexToFaction(string lines[], int index)
    //     returns Tuple<startIndex, endIndex, faction>   
    private Tuple<int, int, int> nextSignUpTypeIndex(string[] lines, int start)
    {
      int startIndex = -1;
      int endIndex = -1;
      int faction = -1;
      int i = start;

      while (true)
      {
          if (i >= lines.Length)
          {
              Exceptions.ThrowSignUp();
          }
          string line = lines[i];
          string headline = this.extractHeadline(line); 
          // array index
          int j = Array.FindIndex(SignUp.TYPE_TO_STR, 
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
      int ROLE_COUNT = Wow.Role.GetNames(typeof(Wow.Role)).Length;
      int i = startIndex;
      int[] ret = new int[ROLE_COUNT];
      const int DATE_ARR_SIZE = 4;
      int[] calender = new int[DATE_ARR_SIZE];
      int[] clock = new int[DATE_ARR_SIZE];
      int count = 0;
      int[] roleOrder = {(int) Wow.Role.Tank, (int) Wow.Role.Melee, 
                         (int) Wow.Role.Ranged, (int) Wow.Role.Healer};        
      while (count <= 3) 
      {
        int role = roleOrder[count];
        string line = lines[i];
        string roleStr = Wow.ROLE_TO_STR[role]; 
        // if line_array find index can find rolestr 
        if (line.Contains(roleStr))
        {
          // role is int value of string so we know already based on match what string it is.
          // it can be tanks, ranged or healers. Ranged and healers should be handled the same,
          // tanks and melees together
          if (role == (int) Wow.Role.Tank) 
          {
            Tuple<int, int> numNIndex = Strings.FindIntNIndex(line);
            ret[(int) Wow.Role.Tank] = numNIndex.Item1;
            // Max int size is 2, so we simply do +2 and we will always
            //  move past first integer.
            int lineIndex = numNIndex.Item2 + 2;
            // Read melee now
            numNIndex = Strings.FindIntNIndex(line.Substring(lineIndex, line.Length-lineIndex));
            ret[(int) Wow.Role.Melee] = numNIndex.Item1;
            count++;
          } else if (role != (int) Wow.Role.Melee)
          {
            // Ranged, Healer
            int num = Strings.FindInt(line);
            ret[role] = num;  
          }
          count++;  
        } else if (line.Contains(SignUp.DATE_TO_STR[(int) Date.Calender])) {
          calender = this.readDate(line);
          if (calender[0] == (int) Date.Error) 
          {
            Exceptions.ThrowSignUp();
          }
        } else if (line.Contains(SignUp.DATE_TO_STR[(int) Date.Clock])) {
          clock = this.readDate(line);
          if (clock[0] == (int) Date.Error) 
          {
            Exceptions.ThrowSignUp();
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
        if (signUpFlag.Equals(headline))
        {
            int imCount = Strings.ConvertToInt(line.Substring(line.Length-2, 2));
            if (imCount == Errors.ERROR_CODE)
            {
              Exceptions.ThrowSignUp();
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
      int i = SignUp.DATE_TO_STR[(int) Date.Clock].Length;
      // find first number
      while (true)
      {
        if (line_cp.Length <= i)
        {
          throw new ArgumentOutOfRangeException(
              String.Format("The length of the line is {0} and the index is {1}", line_cp.Length, i));
        }
        char c = line_cp[i];
        if (Char.IsNumber(c)) 
        {
          break;
        }
        i++;
      }
      // Contains would be safer, but this is much faster :)
      if (SignUp.DATE_TO_STR[(int) Date.Calender].Equals(line_cp.Substring(0, SignUp.DATE_TO_STR[(int) Date.Calender].Length)))
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
      } else if (SignUp.DATE_TO_STR[(int) Date.Clock].Equals(line_cp.Substring(0, SignUp.DATE_TO_STR[(int) Date.Clock].Length)))
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
      if (ret[1] == Errors.ERROR_CODE | ret[2] == Errors.ERROR_CODE | ret[3] == Errors.ERROR_CODE)
      {
        ret[0] = (int) Date.Error;
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

    public int[] getIndexes()
    {
      return this.indexToFaction;
    }

    public IList<int> getAllInfo()
    {
      return this.indexToFaction;
    }

    public int getInfo(int index)
    {
      return this.indexToFaction[index];
    }

    public int getIndex(int faction) 
    {
      return this.indexToFaction[faction];
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
        string role = Wow.ROLE_TO_STR[i];
        ret += String.Format("\n\t\t<{0} count>", role);                
        ret += String.Format("\n\t\t\t{0}", this.rolesCount[i]);
      }
      ret += "\n\t<Indexes to sign up groups>";
      for (int i=0; i < this.indexToFaction.Length; i++) 
      {
        string faction = SignUp.TYPE_TO_STR[i];
        ret += String.Format("\n\t\t<{0}>", faction);
        ret += String.Format("\n\t\t\t{0}", indexToFaction[i]);
        #if (DEBUG)
          ret += String.Format("\n\t\t\tLine at index:");
          ret += String.Format("\n\t\t\t\t{0}", this.getRawLines()[indexToFaction[i]]);
        #endif
      }
      return ret;
    }
  }
}