using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Utilities // utilities
{ 
  namespace LookUp
  {
    public enum SortOrder
    {
      Precede = -1,
      Same = 0,
      Follow = 1,
    }

    // holds lookup values arrays
    public static class StringLookUp
    {
      public const int ERROR = Int32.MinValue;
      public const string RAID_ROSTER_PATH = "raid_roster.txt";
      public static readonly string OUTPUT_PATH = "Assignments";
      public static readonly string TEMPLATE_PATH = "Templates";
      //quick look up for priority numbers
      public static readonly string[] NUM_TO_STR = {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
        "11", "12", "13", "14", "15"
      };
      public static readonly string[] ASS_TYPE_TO_STR = {
        "Message", "Player"
      };
      public static readonly char[] ASS_TYPE_TO_CHAR = {
        'm', 'a'
      };
      // for more fluent comparison.
      public static readonly string[] ASS_TYPE_TO_CHARSTR = {
        "m", "a"
      };
      public static readonly string[] ASS_CL_TO_STR = {"None", "Druid", "Hunter",
        "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior"
      };
      public static readonly string[] ASS_PL_TYPE_TO_STR = {
        "Tank", "Healer", "Interrupter", "Kiter"
      };
      public static readonly string[] ASS_CLASS_TO_STR = {
        "None", "Druid", "Hunter", "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior"
      };
      public static readonly string[] MSG_TYPE_TO_STR = {
        "Interrupter", "Tank", "Healer", "Melee", "Ranged", "Druid", "Hunter",
        "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior", "Kiter", "Admin"
      };
    }
  }
}