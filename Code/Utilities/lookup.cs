using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Utilities // utilities
{ 
  // holds lookup values arrays
  public static class LookUp
  {
    public static int ERROR = Int32.MinValue;
    public const string DISCORD_SIGNUP_PATH = "discord_signup.txt";
    public const string RAID_ROSTER_PATH = "raid_roster.txt";
    public const string OUTPUT_PATH = "Assignments";
    public const string TEMPLATE_PATH = "Templates";
    public static string[] FACTION_TO_STR = {
      "Tank", "Hunter", "Druid", "Warrior", "Mage", "Shaman", "Rogue", 
      "Warlock", "Priest"
    };
    //quick look up for priority numbers
    public static string[] NUM_TO_STR = {
      "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
      "11", "12", "13", "14", "15"
    };

    public static string[] ASS_TYPE_TO_STR = {
      "Message", "Player"
    };

    public static char[] ASS_TYPE_TO_CHAR = {
      'm', 'a'
    };
    // for more fluent comparison.
    public static string[] ASS_TYPE_TO_CHARSTR = {
      "m", "a"
    };

    public static string[] ASS_CL_TO_STR = {"None", "Druid", "Hunter",
      "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior"
    };

    public static string[] ASS_PL_TYPE_TO_STR = {
      "Tank", "Healer", "Interrupter", "Kiter"
    };

    public static string[] ASS_CLASS_TO_STR = {
      "None", "Druid", "Hunter", "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior"
    };

    public static string[] DATE_TO_STR = {"CMcalendar", "CMclock"}; 
    public static string[] CLASS_TO_STR = {
      "Druid", "Hunter", "Mage", "Priest", "Rogue", "Shaman", "Warlock", 
      "Warrior"
    };
    public static string[] ROLE_TO_STR = {"Tank", "Healer", "Melee", "Ranged"};
    public static string[] MSG_TYPE_TO_STR = {
      "Interrupter", "Tank", "Healer", "Melee", "Ranged", "Druid", "Hunter",
      "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior", "Kiter", "Admin"
    };
  }
}