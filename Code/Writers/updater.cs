using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Enumerator;
using System.Linq;
using Readers;
using Containers;

namespace Writers
{     
  //  Implement class Updater
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
  public class Updater
  { 
    private SignUp signUp;
    private Roster roster;

    public Roster Roster
    {
      get { return this.roster; }
      private set { this.roster = value; }
    }

    public SignUp SignUp
    {
      get { return this.signUp; }
      private set { this.signUp = value; }
    }

    public Updater()
    {
      this.Roster = new Roster();
      /*
      SignUp constuctor
                   string path, 
                   Priorities allPrios, 
                   List<string> allPlayerNames
      */
      this.SignUp = new SignUp(this.Roster.getPriorities(), 
                               this.Roster.getPlayerNames());
      // FOR EACH FILE @TEMPLATE_PATH
    }

    public void UpdateAllAssignments()
    {
      Exceptions.ThrowNotImplemented("UpdateAssignment");
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
      Exceptions.ThrowNotImplemented("UpdateAssignment");
    }
  }
}