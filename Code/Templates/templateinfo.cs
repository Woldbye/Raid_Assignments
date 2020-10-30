using System;
using System.IO;
using System.Text;
using System.Collections.Generic;   
using System.Collections;
using Templates.Tasks.Assignments;
using Utilities;
using Utilities.WorldOfWarcraft;
using System.Linq;
using Containers;
using Enumerator;
using Indexes;

// TO:DO
// IMPLEMENT TEMPLATE AS TWO CLASSES:
//    one in Readers/File_info/templateinfo.cs
//    another that utilizes templateinfo called Readers/template.cs
namespace Templates.Tasks
{   
  /*
    Receives path to template
    Templateinfo is an auxilliary class used to read information regarding assignments
    It will keep track each Assignment, and init an Assignment object
    Idea is that the Template class, will compose a TemplateInfo object and a TemplateHandler
    The handler will have the function HandleAssignments, which will handle assignments accordingly.
  */
    /*
  public class TemplateInfo : FileReader, ITextReader<string>, ITextInfo<Assignment>
  {
    private static string MsgEndFlag = "{/p}";
    private List<Assignment> assignments;
    private string rawLines;

    // Validates whether a given string is an assignment
    // An assignment string must:
    //   start with {
    //   letter 2 should be Assignment type char
    //   letter 3 should be seperator :
    //   end with }
    //   atleast length 5
    //  Throws error if string has an assignment signature, but invalid syntax
    public static bool IsAssignment(string str)
    {
      char start = TemplateInfo.GetSeperator(Seperator.Start);
      char end = TemplateInfo.GetSeperator(Seperator.End);
      char mid = TemplateInfo.GetSeperator(Seperator.Mid); 

      if (str.Length < 5)
      {
        return false;
      }
      // check start and end
      if (!str[0].Equals(start) |
          !str[str.Length-1].Equals(end))
      {
        return false;
      }
      
      int type = TemplateTask.TASK_TO_RAW.IndexOfIgnoreCase(str[1]);
      if (type < 0)
      {
        return false;
      }

      if (!str[2].Equals(mid))
      {
        return false;
      }
      return true;
    }

    public TemplateInfo(string path) : base(path)
    {
      this.rawLines = base.readFile(base.getPath());
      if (this.rawLines.Length/2 > StringIndex.Max)
      {
        Exceptions.ThrowArgument(String.Format("Template file size at path {0} is too large",
                                                base.getPath()));
      }
      this.assignments = new List<Assignment>();
      this.read(this.rawLines);
      this.assignments.Sort();
    }

    public void read(string lines)
    {
        int index = 0;
        #if (DEBUG)
          int i = 0;
        #endif
        while (index != (int) Search.None)
        {
          index = this.readAssignment(index);
          #if (DEBUG)
            if (index != (int) Search.None)
            {
              Console.WriteLine("Current assignments: " + i);
              foreach (Assignment ass in this.assignments)
              {
                Console.WriteLine("\t" + ass.ToString());
              }
            }
            i++; 
          #endif
        }
    }

    // Receives a string and returns the appropriate assignment the string represents
    // Should only be called on objects deemed to be an assignment by IsAssignment
    public Assignment interpretAssignment(StringIndex strIndex)
    {
      int start = strIndex.getStart();
      int end = strIndex.getEnd();
      string assStr = this.rawLines.Substring(start, end - start);
      string[] splitStr = assStr.Split(TemplateInfo.Seperators, StringSplitOptions.RemoveEmptyEntries);
      string typeStr = splitStr[0];
      int type = Assignment.StringToAssType(typeStr);
      // {assType:msgType}
      if (type == (int) AssType.Message)
      {
        // Search for message type
        string msgTypeStr = splitStr[(int) AssMsgInfo.MessageType];
        string[][] msgTypesToStr = new string[(int) AssMsgInfo.Type.Max()+1][];
        msgTypesToStr[(int) AssMsgInfo.MessageType] = LookUp.MSG_TYPE_TO_STR;
        int msgType = Assignment.StringToType((int) AssMsgInfo.MessageType, msgTypeStr, msgTypesToStr);
        return new AssMessage(msgType);
      } else if (type == (int) AssType.Player)
      {
        if (splitStr.Length <= AssPlayerInfo.PlayerType.Max())
        {
          Exceptions.ThrowAssignment(assStr);
        } 
        string[][] playerTypesToStr = new string[AssPlayerInfo.Type.Max()+1][];
        playerTypesToStr[(int) AssPlayerInfo.PlayerType] = LookUp.ASS_PL_TYPE_TO_STR;
        playerTypesToStr[(int) AssPlayerInfo.Class] = LookUp.ASS_CL_TO_STR;
        playerTypesToStr[(int) AssPlayerInfo.PriorityNumber] = LookUp.NUM_TO_STR;
        int playerType = Assignment.StringToType((int) AssPlayerInfo.PlayerType,
                                                  splitStr[(int) AssPlayerInfo.PlayerType], 
                                                  playerTypesToStr);
        int classType = Assignment.StringToType((int) AssPlayerInfo.Class, 
                                                splitStr[(int) AssPlayerInfo.Class], 
                                                playerTypesToStr);
        int priorityNumber = Assignment.StringToType((int) AssPlayerInfo.PriorityNumber, 
                                                splitStr[(int) AssPlayerInfo.PriorityNumber], 
                                                playerTypesToStr);
        return new AssPlayer(playerType, classType, priorityNumber);
      }
      return null;
    }

    // TO:DO FIX
    // searches rawLines for the next assignment starting at index start
    // returns Search.None if no more assignments from start position.
    public int readAssignment(int start = 0)
    {
      StringIndex assIndex = this.indexOfAssignment(start);
      int endIndex = assIndex.getEnd();
      if (endIndex != 0)
      {
        Assignment assignment = this.extractAssignment(assIndex);
        #if (DEBUG)
          Console.WriteLine("---ReadAssignment debug info:---");
          Console.WriteLine("\tAdding the following assignment to assignments:");
          Console.WriteLine(String.Format("\t{0}", assignment.ToString().Replace("\t", "\t\t")));
        #endif
        bool isAssPlayer = assignment is AssPlayer ? true : false;
        int prioNumber = isAssPlayer ? (assignment as AssPlayer).getPriorityNumber() : 1; 
        if (prioNumber == 0 | !this.assignments.Contains(assignment))
        {
          this.assignments.Add(assignment);
        }
      } else {
        endIndex = (int) Search.None;
      }
      return endIndex;
    }

    // finds the next assignment starting from int start
    // if not more assignments from start position, it will return stringIndex(0,0)
    private StringIndex indexOfAssignment(int start = 0)
    {
      bool isAssignment = false;
      StringIndex assignmentIndex = new StringIndex();
      int startCandidate = start;
      int endCandidate = 0;

      while (!isAssignment & startCandidate <= this.rawLines.Length)
      {
        // possible assignment start index
        startCandidate = this.rawLines.IndexOf(TemplateInfo.GetSeperator(Seperator.Start), 
                                               startCandidate);
        // Will throw out of bounds error if the templatefile ends with {
        endCandidate = (startCandidate == (int) Search.None) ?
                           (int) Search.None : 
                           this.rawLines.IndexOf(TemplateInfo.GetSeperator(Seperator.End),
                                                 startCandidate);
        if (startCandidate == (int) Search.None | endCandidate == (int) Search.None)
        {
          break;
        } 
        endCandidate++;
        isAssignment = TemplateInfo.IsAssignment(this.rawLines.Substring(startCandidate, 
                                                                         endCandidate-startCandidate));
        if (isAssignment)
        {
          assignmentIndex.setStart(startCandidate);
          assignmentIndex.setEnd(endCandidate);
        } else {
          startCandidate++;
        }
      }
      return assignmentIndex;
    }

    public string getRawLines()
    {
      return this.rawLines;
    }

    // return assignment arr
    public IList<Assignment> getAllInfo()
    {
      return this.assignments;
    }

    // return assignment obj
    public Assignment getInfo(int index)
    {
      return this.assignments[index];
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder("<TemplateInfo>:\n", 1000);
      sb.Append("\t<Template Path>\n");
      sb.Append(String.Format("\t    {0}\n", base.getPath())); 
      sb.Append("\t<Read Assignments>\n");
      foreach (Assignment ass in this.assignments)
      {
        sb.Append(String.Format("\t    {0}\n", ass.ToString()));
      }
      return sb.ToString();
    }
  }
  */
}