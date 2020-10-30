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

	public enum Date
	{
		Error = -1,
		Calender = 0,
		Clock = 1
	};
}