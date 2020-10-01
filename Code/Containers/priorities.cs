using Utilities;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Enumerator;

namespace Containers {
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

      public Stack<string> getNewPriority(Priority priority)
      {
        return new Stack<string>(this.priorities[(int) priority]);
      }

      public Stack<string> getNewPriority(int priority)
      {
        return new Stack<string>(this.priorities[priority]);
      }

      public Stack<string> getPriority(Priority priority) 
      {
        return this.priorities[(int) priority];
      }

      public Stack<string> getPriority(int priority) 
      {
        if (!(priority < this.priorities.Length))
        {
          Exceptions.ThrowArgument(String.Format("The index {0} is out of range. Max is {1}", priority, this.priorities.Length));
        }
        return this.priorities[priority];
      }



      public override string ToString()
      {
        string ret = "<Priorities>:";
        for (int i=0; i < this.priorities.Length; i++) 
        {
          Stack<string> prio_stack = this.getNewPriority(i);
          ret += String.Format("\n\t<{0}>:\n", PRIO_STR[i]);
          while (prio_stack.Count > 0)
          {
            ret += String.Format("\t\t{0}\n", prio_stack.Pop());
          }
        }  
        return ret; 
      }

      public string popFromPriority(Priority priority)
      {
        string name = this.getPriority(priority).Pop();
        Console.WriteLine(
          String.Format("Popped name {0} from priority {1}: ",
            name,
            priority));
        return name;
      }

      public string popFromPriority(int priority)
      {
        string name = this.getPriority(priority).Pop();
        Console.WriteLine(
            String.Format("Popped name {0} from priority {1}: ",
              name,
              (Priority) priority));
        return name;
      }

      public Stack<string>[] getPriorities()
      {
          return this.priorities;
      }

      public Stack<string>[] getNewPriorities()
      {
        Stack<string>[] ret = new Stack<string>[this.priorities.Length];
        for (int i=0; i < this.priorities.Length; i++)
        {
          ret[i] = this.getNewPriority(i);
        }
        return ret;
      }        
    }
}