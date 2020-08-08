using Util;
using System;
using Generics;
using Translate;

namespace Assignments {
	class Raid_Assigner
	{
		public static int Main() 
		{	
			Wow_Class class_ = Wow_Class.Shaman;
			Role role_ = Role.Tank;
			string name = "Jourbah";
			bool isOT = true;
			bool isInterrupter = false;
			string stringToTrim = "	hello QweaTable_EO \n";
			Console.WriteLine(Strings.Trim(stringToTrim));
			Player test_pl = new Player(name, class_, role_, isOT, isInterrupter);
			// Console.WriteLine(test_pl);
			Console.WriteLine(test_pl.Equals(name));
			return 0;
		}
	}
}