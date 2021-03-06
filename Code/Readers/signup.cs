using System;
using System.IO;
using System.Collections.Generic;
using Enumerator;
using System.Collections;
using Utilities;
using Utilities.WorldOfWarcraft;
using Containers;
using System.Linq;
using Templates.Tasks.Assignments;

namespace Readers // utilities
{ 
  // types the sign ups are grouped up under. Basicly wow_class + Tank
  public enum SignUpType
  {
    Tank, // 0
    Hunter, // 1
    Druid, // 2
    Warrior, // 3
    Mage, // 4
    Shaman, // 5
    Rogue, // 6
    Warlock, // 7
    Priest // 8// 4 bits
  }
  // TO:DO REWRITE THSI FUCKING MESS :)
  public class SignUp : ITextReader<string[]>
  { 
    private SignUpInfo signUpInfo;
    private Priorities prios;
    private List<string> playerNames;
    public const string PATH = "discord_signup.txt";

    // calender is date, clock is time.
    public static readonly string[] DATE_TO_STR = {"CMcalendar", "CMclock"};

    public static readonly string[] TYPE_TO_STR = {
      "Tank", "Hunter", "Druid", "Warrior", "Mage", "Shaman", "Rogue", 
      "Warlock", "Priest"
    };

    public SignUp(Priorities allPrios, 
                  List<string> allPlayerNames)
    {
      Console.WriteLine("---Starting SignUp init---");
      this.signUpInfo = new SignUpInfo();
      this.read(this.signUpInfo.getRawLines());
      this.prios = this.adjustPriorities(allPrios, this.playerNames);
      Console.WriteLine("****Finished Sign Up init****");
    }

    // TO:DO Fix function so it doesnt loop if a class is not present
    // Reads all player names at sign up and returns them as a List<string>
    public void read(string[] lines)
    {
      List<string> playerNames = new List<string>();
      // go to each index contained in SignUp
      int factionCount = SignUpType.GetNames(typeof(SignUpType)).Length;
      for (int i=0; i < factionCount; i++)
      {
        int faction = i;
        string factionStr = SignUp.TYPE_TO_STR[faction].ToLower();
        int index = this.signUpInfo.getIndex(faction);
        string line = lines[index];
        string headline = this.signUpInfo.extractHeadline(line);
        if (headline.Equals("shadow"))
        {
          headline = "priest";
        }
        while(headline.Contains(factionStr) && index < lines.Length)
        {
          line = lines[index];
          headline = this.signUpInfo.extractHeadline(line);
          if (headline.Equals("shadow"))
          {
            headline = "priest";
          }
          string name = this.readName(line);
          if (!String.IsNullOrEmpty(name))
          { 
            playerNames.Add(name.ToLower());
          }
          index++;
        }
      }

      this.playerNames = playerNames;   
    }

    private Priorities adjustPriorities(Priorities allPrios, List<string> names)
    {
      string[] prios = Enum.GetNames(typeof(AssignmentType));
      int stackCount = prios.Length;
      Stack<string>[] adjPrios = new Stack<string>[stackCount];
      for (int i=0; i < stackCount; i++)
      {
        adjPrios[i] = new Stack<string>();
      }

      for (int i=0; i < stackCount; i++)
      {
        AssignmentType assType = (AssignmentType) i;
        Console.WriteLine("Current priority: " + assType);
        Stack<string> adjPrio = this.adjustPrio(allPrios.getPriority(assType), names); 
        adjPrios[i] = adjPrio;
      }

      return new Priorities(adjPrios);
    }

    public Stack<string> getPriority(int priority)
    {
      return this.prios.getPriority((AssignmentType) priority);
    }

    public Stack<string> getPriority(AssignmentType priority)
    {
      return this.prios.getPriority(priority);
    }

    public Priorities getPriorities()
    {
      return this.prios;
    }

    private Stack<string> adjustPrio(Stack<string> prio, List<string> names)
    {
      Stack<string> ret = new Stack<string>();
      List<string> stackInitList = new List<string>();
      #if (DEBUG)
        Console.WriteLine("----adjustPrio DEBUG info----");
        Console.WriteLine("Printing all received names:");
        foreach (string name in names)
        {
          Console.WriteLine("\t" + name);
        }
        Console.WriteLine("and received prio:");
        foreach (string name in prio)
        {
          Console.WriteLine("\t" + name);
        }
      #endif
      foreach (string name in prio)
      {
        Console.WriteLine("Searching for " + name);
        if (names.Contains(name))
        {
          stackInitList.Add(name);
        }
      }
      int i = 0;
      int length = stackInitList.Count;
      string[] stackInitArray = new string[length];
        
      // push in reverse order so first we convert to array.
      // fst in stackInitList is last to be pushed
      foreach(string name in stackInitList) 
      {
        stackInitArray[i] = name;
        i++;
      }

      for (int j=0; j < length; j++)
      {
        string name = stackInitArray[j];
        ret.Push(name);
      }

      return ret;
    }

    private string readName(string line)
    {
      return Strings.FindSecondLetterSubstring(line);
    }

    public override string ToString()
    {
      string ret = "<Sign Up>:";
      ret += "\n  <Signed Players:>";
      foreach (string name in this.playerNames)
      {
        ret += "\n\t\t" + name;
      }
      ret += "\n  " + this.prios.ToString();
      return ret;
    }
  }
}