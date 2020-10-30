using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Utilities.WorldOfWarcraft;
using System.Linq;
using Containers;
using Enumerator;
// template info will loop file. It will then discover all assignments and remove duplicates.
// we then sort assignments such that top is class assignments - Tanks - Healers - msg
// For each assignment we do Template.Replace(assignment), and it will replace the assignment 
// each time we remove from class assignments we will remove same player from assignments
// 



// TO:DO
// IMPLEMENT TEMPLATE AS TWO CLASSES:
//    one in Readers/File_info/templateinfo.cs
//    another that utilizes templateinfo called Readers/template.cs
namespace Readers
{   
  /*
    Receives path to template
    Template is used to convert the template assignment into the raw exorsus string
    The exorsus class will then load the exorsus string into the appropriate file.
  */
  /*
  public class Template
  {
    private TemplateInfo tempInfo;

    public Template(string path)
    {
      this.tempInfo = new TemplateInfo(path);
      #if (DEBUG)
        Console.WriteLine(tempInfo.ToString());
      #endif
    }

    public string toAssignment(AssignmentReceivers receivers, Stack<string>[] prioStack)
    {
      return "UNFINISHED";
      // find all Assignment strings
      // sort them
      // 
    }
  }
  */
}