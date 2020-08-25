using Util;
using System;
using Generics;
using Translate;

class Raid_Assigner
	{
		// TO:DO - 
		// Convert all data_table files to .csv
		//	implement reading of .csv files


		// Make all dictionaries be:
		// name.hash -> Player

		// 	Implement class SignUp
		//			Should read a file which contains copy paste of discord sign up
		// 		  Constructor receives path to file and look up roster dictionary.
		//  	  For each signed member it should look up in dictionary and add the appropriate player.
		//	
		//  Lets say we need interrupters. We will now be able to:
		//		cp interrupt stack
		//		pop player from stack
		//		if player in sign up
		//			assign player to interrupt
		//	  Same idea for healers
		//	Implement class Exorsus
		//	  Contains Sign up
		//		Contains Reader
		//		Function void UpdateAssignments()
		//		{
		//			loop through all templates
		//				For each template found it will: write output of assignPlayers(templates/Path) to appropriate Assignment/path
		//		}
		//		Function string assignPlayers(string path)??
		// 			Should read file of exorsus format (but with my own injection i.e. {tank0} or something. 
		// 			It will replace each assignment in the file with the appropriate player. 
		//			Further, it will replace the receiver strings {healers}, {rogue} etc. by calling AssignmentReceivers.ToExorsus()
		//  Main here simply executes updateassignments.
		//	After its finished I simply need to copy paste disscord sign up -> run raid_assign.cs -> copy paste each Assignment/Path into exorsus
		public static int Main() 
		{	
			Reader reader_test = new Reader();
			string[] msg = new string[2];
			msg[0] = "do this";
			msg[1] = "do that";
			Console.WriteLine("---Healers---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("healer", msg));
			Console.WriteLine("---Tanks---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("tank", msg));
			Console.WriteLine("---Melee---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("melee", msg));
			Console.WriteLine("---Ranged---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("ranged", msg));
			Console.WriteLine("---Warlocks---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("warlock", msg));
			Console.WriteLine("---Interrupters---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("interrupt", msg));
			Console.WriteLine("---Mages---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("mage", msg));
			Console.WriteLine("---Shamans---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("shaman", msg));
			Console.WriteLine("---Hunters---");
			Console.WriteLine(reader_test.getReceivers().ToExorsus("hunter", msg));
			// string path = "discord_signup.txt";
			// SignUp signup = new SignUp(path);
			return 0;
		}
	}