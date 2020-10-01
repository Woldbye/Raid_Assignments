using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using Wow_Objects;
using System.Linq;
using Readers;
using Enumerator;

namespace Containers 
{ 
  // Object to hold all names of the receivers of each assignment.
  // constructor receives List<string>[] namesByClass, List<string> admins
  public class AssignmentReceivers
  {
    private List<string> admin_a; // hold name for admins whom should receive all assignments
    private List<string> interrupt_a; // hold name of interrupters whom should receive interrupt assignments
    private List<string> tank_a; // hold name of tanks whom should receive tank assignments
    private List<string> healer_a; // hold name of healers whom should receive healer assignments
    private List<string> ranged_a; // hold name of ranged whom should receive ranged assignments
    private List<string> melee_a; // hold name of melees whom should  receive melee assignments
    private List<string>[] class_a; // class_A[(int) Wow_Class.Class] will output list of all players of that class

    public AssignmentReceivers(List<string>[] namesByClass, List<string> admins) 
    {
      this.admin_a = admins;
      this.class_a = namesByClass;
      // init healers first as tank_a and ranged_a depends on healers.
      this.healer_a = this.initHealer();
      this.ranged_a = this.initRanged();
      this.interrupt_a = this.initInterrupt();
      this.melee_a = this.initMelee();
      this.tank_a = this.initTank();
      #if (DEBUG)
          Console.WriteLine("Finished Init of AssignmentReceivers");
          Console.WriteLine(this.ToString());
      #endif
    }

    private string assignmentToString(MsgType msgType)
    {
      string ret = "{p:";
      switch (msgType)
      {
        case MsgType.Interrupt:
          foreach(string interrupter in this.interrupt_a)
          {
            ret += interrupter;
            if (!(interrupter.Equals(this.interrupt_a.Last())))
            {
              ret += ","; 
            } 
          }
          break;
        case MsgType.Tank:
          foreach(string tank in this.tank_a)
          {
            ret += tank;
            if (!(tank.Equals(this.tank_a.Last())))
            {
              ret += ","; 
            } 
          }
          break;
        case MsgType.Healer:
          foreach(string healer in this.healer_a)
          {
            ret += healer;
            if (!(healer.Equals(this.healer_a.Last())))
            {
              ret += ","; 
            } 
          }
          break;
        case MsgType.Melee:
          foreach(string melee in this.melee_a)
          {
            ret += melee;
            if (!(melee.Equals(this.melee_a.Last())))
            {
              ret += ","; 
            }
          }
          break;
        case MsgType.Ranged:
          foreach(string ranged in this.ranged_a)
          {
            ret += ranged;
            if (!(ranged.Equals(this.ranged_a.Last())))
            {
              ret += ","; 
            } 
          }
          break;
        // classes
        case MsgType.Druid:
          List<string> druids = this.class_a[(int) Wow_Class.Druid];
          foreach(string druid in druids)
          {
            ret += druid;
            if (!(druid.Equals(druids.Last())))
            {
              ret += ","; 
            }
          }
          break;
        case MsgType.Hunter:
          List<string> hunters = this.class_a[(int) Wow_Class.Hunter];
          foreach(string hunter in hunters)
          {
            ret += hunter;
            if (!(hunter.Equals(hunters.Last())))
            {
              ret += ","; 
            }
          }
          break;
        case MsgType.Mage:
          List<string> mages = this.class_a[(int) Wow_Class.Mage];
          foreach(string mage in mages)
          {
            ret += mage;
            if (!(mage.Equals(mages.Last())))
            {
              ret += ","; 
            } 
          }
          break;
        case MsgType.Priest:
          List<string> priests = this.class_a[(int) Wow_Class.Priest];
          foreach(string priest in priests)
          {
            ret += priest;
            if (!(priest.Equals(priests.Last())))
            {
              ret += ","; 
            }
          }
          break;
        case MsgType.Rogue:
          List<string> rogues = this.class_a[(int) Wow_Class.Rogue];
          foreach(string rogue in rogues)
          {
            ret += rogue;
            if (!(rogue.Equals(rogues.Last())))
            {
              ret += ","; 
            } 
          }
          break;
        case MsgType.Shaman:
          List<string> shamans = this.class_a[(int) Wow_Class.Shaman];
          foreach(string shaman in shamans)
          {
            ret += shaman;
            if (!(shaman.Equals(shamans.Last())))
            {
              ret += ","; 
            } 
          }
          break;
        case MsgType.Warlock:
          List<string> warlocks = this.class_a[(int) Wow_Class.Warlock];
          foreach(string warlock in warlocks)
          {
            ret += warlock;
            if (!(warlock.Equals(warlocks.Last())))
            {
              ret += ","; 
            }
          }
          break;
        case MsgType.Warrior:
          List<string> warriors = this.class_a[(int) Wow_Class.Warrior];
          foreach(string warrior in warriors)
          {
            ret += warrior;
            if (!(warrior.Equals(warriors.Last())))
            {
              ret += ","; 
            }
          }
          break;
        case MsgType.Kiter:
          List<string> kiters = this.class_a[(int) Wow_Class.Hunter];
          foreach(string kiter in kiters)
          {
            ret += kiter;
            if (!(kiter.Equals(kiters.Last())))
            {
              ret += ","; 
            }
          }
          break;
        case MsgType.Admin:
          foreach(string admin in this.admin_a)
          {
            ret += admin;
            if (!(admin.Equals(this.admin_a.Last())))
            {
              ret += ","; 
            }
          }
          break;
        default:
          throw new ArgumentException(String.Format("Argument {0} is invalid", msgType), "argument");
      }
      ret += "}";
      return ret;
    }
    /*
    Receives a format string containing the class that needs assignments and outputs the corresponding exorsus
    string that prints the assignments to those.
    The end of the assignment string should be prepended by {/p}
    List of format strings:
        1: // for each role
            {healer}
            {/p}
        2: // for each class
            {rogue}
            {/p}
        3:  
            {admin}
            {/p}
    Also supports + and - OPs. 
        1:  // will add all healer_a and tank_a
            {healer + tank}
            {/p}
        2: // will subtract all healer_a from tank_a and output any remaining
            {tank_a - healer_a}
            {/p}
    */ 
    public string ToExorsus(MsgType msgType, string[] msg)
    {
      string ret = "";
      ret += this.assignmentToString(msgType);
      ret += "\n";
      foreach (string line in msg)
      {
        ret += line;
        ret += "\n";
      }
      ret += "{/p}";
      return ret;
    }

    // Function ToExorsus should only be called with a valid MsgType enum, 
    //  only for convenience we also implement function to allow for integer input.
    public string ToExorsus(int type, string[] msg)
    {
      if (type > MsgType.Interrupt.Max())
      {
        throw new ArgumentException(String.Format("Argument {0} is invalid", type), "argument");
      }
      MsgType msgType = (MsgType) type;
      return this.ToExorsus(msgType, msg);
    }

    // healer = Priests, Druids, Shamans, Admins
    private List<string> initHealer() 
    {
      List<string> healers = new List<string>();
      int[] healerClasses = {(int) Wow_Class.Druid, (int) Wow_Class.Priest, (int) Wow_Class.Shaman};
      for (int i=0; i < healerClasses.Length; i++) 
      {
        int healerClass = healerClasses[i];
        healers.AddRange(this.class_a[healerClass]);
      } 
      healers.AddRange(this.admin_a);
      return healers.Distinct().ToList();
    }
    // ranged = healer_a, Warlocks, Mages, Hunters,
    private List<string> initRanged() 
    {
      List<string> ranged = new List<string>();
      int[] rangedClasses = { (int) Wow_Class.Warlock, (int) Wow_Class.Mage, (int) Wow_Class.Hunter};
      for (int i=0; i < rangedClasses.Length; i++) 
      {
        int rangedClass = rangedClasses[i];
        ranged.AddRange(this.class_a[rangedClass]);
      } 
      ranged.AddRange(this.admin_a);
      ranged.AddRange(this.healer_a);
      return ranged.Distinct().ToList();
    }

    // melee = Warriors, Rogues, Druids, Admins
    private List<string> initMelee() 
    {
      List<string> melee = new List<string>();
      int[] meleeClasses = { (int) Wow_Class.Warrior, (int) Wow_Class.Rogue, (int) Wow_Class.Druid};
      for (int i=0; i < meleeClasses.Length; i++) 
      {
        int meleeClass = meleeClasses[i];
        melee.AddRange(this.class_a[meleeClass]);
      } 
      melee.AddRange(this.admin_a);
      return melee.Distinct().ToList();
    }

    // tanks = Warriors, Druids, Admins, healer_a
    private List<string> initTank() 
    {
      List<string> tanks = new List<string>();
      int[] tankClasses = { (int) Wow_Class.Druid, (int) Wow_Class.Warrior};
      for (int i=0; i < tankClasses.Length; i++) 
      {
        int tankClass = tankClasses[i];
        tanks.AddRange(this.class_a[tankClass]);
      } 
      tanks.AddRange(this.admin_a);
      tanks.AddRange(this.healer_a);
      return tanks.Distinct().ToList();
    }

    // interrupt = Rogues, Warriors, Mages, Shamans, Admins
    private List<string> initInterrupt() 
    {
      List<string> interrupters = new List<string>();
      int[] interruptClasses = { (int) Wow_Class.Rogue, (int) Wow_Class.Mage, (int) Wow_Class.Warrior, (int) Wow_Class.Shaman};
      for (int i=0; i < interruptClasses.Length; i++) 
      {
        int interruptClass = interruptClasses[i];
        interrupters.AddRange(this.class_a[interruptClass]);
      } 
      interrupters.AddRange(this.admin_a);
      return interrupters.Distinct().ToList();
    }

    public List<string> getNamesOfClass(int wow_class)  
    {
      return this.class_a[wow_class]; 
    }

    public List<string> getNamesOfClass(Wow_Class wow_class)  
    {
      return this.class_a[(int) wow_class]; 
    }

    public List<string> getTanks() 
    {
      return this.tank_a;
    }

    public List<string> getHealers() 
    {
      return this.healer_a;
    }

    public List<string> getInterrupters() 
    {
      return this.interrupt_a;
    }

    public List<string> getMelees()
    {
      return this.melee_a;
    }
        
    public List<string> getRanged() 
    {
      return this.ranged_a;
    }

    public override string ToString()
    {
      string ret = "<AssignmentReceivers>:";
      ret += "\n\t<Melee>:";
      int j = 0;
      foreach (string melee in this.melee_a)
      {
        ret += String.Format("\n\t\t{0}:\t{1}", j+1, melee);
        j++;
      }
      ret += "\n\t<Ranged>:";
      j = 0;
      foreach (string ranged in this.ranged_a)
      {
        ret += String.Format("\n\t\t{0}:\t{1}", j+1, ranged);
        j++;
      }
      ret += "\n\t<Interrupters>:";
      j = 0;
      foreach (string interrupter in this.interrupt_a)
      {
        ret += String.Format("\n\t\t{0}:\t{1}", j+1, interrupter);
        j++;
      }
      ret += "\n\t<Healers>:";
      j = 0;
      foreach (string healer in this.healer_a)
      {
        ret += String.Format("\n\t\t{0}:\t{1}", j+1, healer);
        j++;
      }
      ret += "\n\t<Tanks>:";
      j = 0;
      foreach (string tank in this.tank_a)
      {
        ret += String.Format("\n\t\t{0}:\t{1}", j+1, tank);
        j++;
      }
      ret += "\n\t<Admins>:";
      j = 0;
      foreach (string admin in this.admin_a)
      {
        ret += String.Format("\n\t\t{0}:\t{1}", j+1, admin);
        j++;
      }
      ret += "\n\t<Players by class>:";
      j = 0;
      foreach (List<string> class_obj in this.class_a)
      {
        ret += String.Format("\n\t\t<{0}>", LookUp.CLASS_TO_STR[j]);
        int k = 0;
        foreach (string name in class_obj)
        {
          ret += String.Format("\n\t\t\t{0}:\t{1}", k+1, name);
          k++;
        }
        j++;
      }
      return ret;
    }
  }
}