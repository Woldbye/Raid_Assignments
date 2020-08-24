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

    public Exorsus()
    {
      this.reader = new Reader();
      /*
      Discord constuctor
                   string path, 
                   Priorities allPrios, 
                   List<string> allPlayerNames
      */
      this.discord = new Discord(Strings.DISCORD_SIGNUP_PATH, this.reader.getPriorities(), this.reader.getPlayerNames());
      // FOR EACH FILE @TEMPLATE_PATH

    }

    public void UpdateAssignments()
    {
      string[] templateFiles = Directory.GetFiles(Strings.TEMPLATE_PATH); 
      for (int i=0; i < templateFiles.Length; i++)
      {
        string path = String.Format("{0}{0}",
                                     Strings.TEMPLATE_PATH, 
                                     templateFiles[i]);
        string[] rawLines = File.ReadAllText(path).Split("\n");

      }
          //        For each template found it will: 
          //  write output of assignPlayers(templates/Path) to appropriate Assignment/path
    }

    private void UpdateAssignment(string[] lines)
    {
      Error.NotImplemented("UpdateAssignment");
    }
  }
}