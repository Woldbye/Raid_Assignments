using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Util;
using Generics;
using System.Linq;
using Translate;

namespace Assignments // utilities
{ 
//  Implement class Exorsus
    //    Contains Sign up
    //    Contains Reader
    //    Function void UpdateAssignments()
    //    {
    //      loop through all templates
    //        For each template found it will: write output of assignPlayers(templates/Path) to appropriate Assignment/path
    //    }
    //    Function string assignPlayers(string path)??
    //      Should read file of exorsus format (but with my own injection i.e. {tank0} or something. 
    //      It will replace each assignment in the file with the appropriate player. 
    //      Further, it will replace the receiver strings {healers}, {rogue} etc. by calling AssignmentReceivers.ToExorsus()
  public class Exorsus
  { 
    private Discord discord;
    private Reader reader;
    private List<string> raids;


    public Exorsus()
    {
      this.reader = new Reader();
      this.raids = {"AQ20", "AQ40", "BWL", "MC", "ZG"};
      /*
      Discord constuctor
                   string path, 
                   Priorities allPrios, 
                   List<string> allPlayerNames
      */
      this.discord = new Discord(Strings.DISCORD_SIGNUP_PATH, this.reader.getPriorities(), this.reader.getPlayerNames());
      // FOR EACH FILE @TEMPLATE_PATH
    }

    public void UpdateAllAssignments()
    {
      Error.NotImplemented("UpdateAssignment");
    }
    /*
    public void UpdateAssignments(string raid)
    {
      string raidUpper = raid.ToUpper();
      if (!this.raids.Contains(raidUpper))
      {
        Error.ThrowAssignmentError(lowCaseRaid);
      }
      string path = String.Format("{0}/{0}", Strings.TEMPLATE_PATH, raidUpper);
      #if (DEBUG)
        Console.WriteLine("----UpdateAssignments debug info----");
        Console.WriteLine(String.Format("\t\tTrying to update assignments at path: <{0}>", path));
      #endif
      string[] templateFiles = Directory.GetFiles(path); 
      for (int i=0; i < templateFiles.Length; i++)
      { 
        string templateFile = templateFiles[i];
        string filePath = String.Format("{0}/{0}", path, templateFile);
        string[] rawLines = File.ReadAllText(filePath).Split("\n");
        string[] assignment = this.UpdateAssignment(rawLines);
        
        // Now we put assignment[] to file at Path /TEMPLATE_PATH/templateFile
      }
    }
    */
    private void UpdateAssignment(string[] lines)
    {
      Error.NotImplemented("UpdateAssignment");
    }
  }

  /*
    Receives path to template
  */
  public class Template
  {
    private string[] rawLines;
    private string path;
    private string assignFlag = "a:";
    private char startFlag = '{';
    private char endFlag = '}';
    private string typeEndFlag = ':';
    private string msgFlag = "msg";
    private string msgEndFlag = "{/p}";
    private string roleFlag = "role";

    public Template(string path)
    {
      this.path = path;
      try
      {
        this.rawLines = File.ReadAllText(path).Split("\n");
      } catch (Exception e)
      {
        Error.ThrowTemplateError(e, path);
      }

      string tester = "okayokayokayokaoykaoyk   {tank1:something}";
      Tuple<string, int> flagInfo = this.getFlag(tester);
      Console.WriteLine(String.Format("Flag is {0} and index is {1}, which equals char {2}"), flagInfo.Item1, flagInfo.Item2, tester[flagInfo.Item2]); 
    }
    // If receive a string of type:
    // {smthing:info1}
    // it will output smthing and index of :.
    private Tuple<string, int> getFlag(string str)
    {
       // loop through str, when : is found, start recording letters till this.endFlag is found
      string ret = "";
      int retI = Strings.ERROR;
      int i = 0;
      char c = '\0';
      // first we find startFlag
      while (true)
      {
        if (i >= str.Length)
        {
          break;
        }
        c = str[i];
        i++;
        if(c.Equals(this.startFlag))
        {
          break;
        }
      }
      if (!c.Equals('\0'))
      {
        // i is now index after start flag
        while (true)
        {
          if (i >= str.Length)
          {
            break;
          } else if (c.Equals(this.typeEndFlag)) {
            retI = i;
            break;
          }
          c = str[i];
          ret += c;
          i++;
        }
      }
      return Tuple.Create(ret, retI);
    }

    // Converts the current template to the assignment
    public string[] ToAssignment(Priority prios, AssignmentReceivers receivers)
    {
      string[] ret = new string[this.rawLines.Length];
      // idea:
      // loop through template from start to finish
      // look for flags
      // if msgStartFlag is found, look for msgFlag match then do AssignmentReceivers.ToExorsus(msgFlag) 
      return ret; 
    }

  }
}