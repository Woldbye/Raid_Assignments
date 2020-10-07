using System;

namespace Enumerator {
	public enum MsgType
	{
	  Interrupt, // 0 
	  Tank, // 1
	  Healer, // 2
	  Melee, // 3
	  Ranged, // 4
	  Druid, // 5
	  Hunter, // 6
	  Mage, // 7
	  Priest, // 8
	  Rogue, // 9
	  Shaman, // 10
	  Warlock, // 11
	  Warrior, // 12
	  Kiter, // 13
	  Admin // 14
	};

	public enum AssClass
	{
		None, //0
		Druid, //1
		Hunter, // 2
		Mage, // 3
		Priest, // 4 11
		Rogue, // 5
		Shaman, // 6
		Warlock, // 7
		Warrior // 8 111
	}

	// If alliance replace shaman with paladin
	public enum Wow_Class 
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

	public enum Faction
	{
		Tank, // 0
		Hunter, // 1
		Druid, // 2
		Warrior, // 3
		Mage, // 4
		Shaman, // 5
		Rogue, // 6
		Warlock, // 7
		Priest // 8
	}; // 4 bits

	public enum Role 
	{
		Tank, // 0
		Healer, // 1
		Melee, // 2
		Ranged // 3
	}; // 2 bit

	public enum Date
	{
		Error = -1,
		Calender = 0,
		Clock = 1
	};
}