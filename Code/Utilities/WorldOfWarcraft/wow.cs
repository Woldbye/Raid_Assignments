using Utilities;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Enumerator;

namespace Utilities.WorldOfWarcraft
{
  public static class Wow
  {
    // If alliance replace shaman with paladin
    public enum Class 
    {
      Druid, // 0
      Hunter, // 1
      Mage, // 2
      Priest, // 3
      Rogue, // 4
      Shaman, // 5
      Warlock, // 6
      Warrior // 7
    }; // 3 bits

    public enum Role 
    {
      Tank, // 0
      Healer, // 1
      Melee, // 2
      Ranged // 3
    }; // 2 bit

    public enum Raid
    {
      AQ20,
      AQ40,
      BWL,
      MC,
      ZG
    }

    public static readonly string[] CLASS_TO_STR = {
        "Druid", "Hunter", "Mage", "Priest", "Rogue", "Shaman", "Warlock", 
        "Warrior"
    };
    public static readonly string[] ROLE_TO_STR = {"Tank", "Healer", "Melee", "Ranged"};

    public static readonly string[] RAIDS_TO_STR = {"AQ20", "AQ40", "BWL", "MC", "ZG"};
  }
}