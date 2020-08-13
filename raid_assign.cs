using Util;
using System;
using Generics;
using Translate;

namespace Assignments {
	class Raid_Assigner
	{
		// TO:DO - 
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
			return 0;
		}
	}
}