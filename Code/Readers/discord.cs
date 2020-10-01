using System;
using System.IO;
using System.Collections.Generic;
using Enumerator;
using System.Collections;
using Utilities;
using Wow_Objects;
using Containers;
using System.Linq;

namespace Readers // utilities
{ 
  public class Discord : ITextReader<string[]>
  { 
    private SignUpInfo signUpInfo;
    private Priorities prios;
    private List<string> playerNames;

    public Discord(string path, 
                   Priorities allPrios, 
                   List<string> allPlayerNames)
    {
      Console.WriteLine("---Starting discord init---");
      this.signUpInfo = new SignUpInfo(path);
      this.read(this.signUpInfo.getRawLines());
      this.prios = this.adjustPriorities(allPrios, this.playerNames);
      Console.WriteLine("****Finished discord init****");
    }

    // TO:DO Fix function so it doesnt loop if a class is not present
    // Reads all player names at sign up and returns them as a List<string>
    public void read(string[] lines)
    {
      List<string> playerNames = new List<string>();
      // go to each index contained in SignUp
      int factionCount = Faction.GetNames(typeof(Faction)).Length;
      for (int i=0; i < factionCount; i++)
      {
        int faction = i;
        string factionStr = LookUp.FACTION_TO_STR[faction].ToLower();
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
        Console.WriteLine("Current priority: " + priority);
        Stack<string> adjPrio = this.adjustPrio(allPrios.getPriority(priority), names); 
        adjPrios[i] = adjPrio;
      }

      return new Priorities(adjPrios);
    }

    public Stack<string> getPriority(int priority)
    {
      return this.prios.getPriority((Priority) priority);
    }

    public Stack<string> getPriority(Priority priority)
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
      string ret = "<Discord>:";
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