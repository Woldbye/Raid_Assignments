using System;
using Generics;
using System.Runtime.InteropServices;
using System.Linq;

namespace Util // utilities
{	
	public static class Strings
	{
		public static int ERROR = Int32.MinValue;
		public const string DISCORD_SIGNUP_PATH = "discord_signup.txt";
		public const string RAID_ROSTER_PATH = "raid_roster.txt";
		public const string OUTPUT_PATH = "Assignments/";
		public const string TEMPLATE_PATH = "Templates/";
		public static string[] FACTION_TO_STR = {
			"Tank",
			"Hunter",
			"Druid",
			"Warrior",
			"Mage",
			"Shaman",
			"Rogue",
			"Warlock",
			"Priest"
		};

		public static string[] DATE_TO_STR = {"CMcalendar", "CMclock"}; 
		public static string[] CLASS_TO_STR = {"Druid", "Hunter", "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior"};
		public static string[] ROLE_TO_STR = {"Tank", "Healer", "Melee", "Ranged"};

		// Returns the start index 
		// and total character count of letter substring.
		public static Tuple<int, int> FindLetterSubstring(string str)
		{
			int start = 0;
			int count = 0;

			if (str.Length > 0)
			{
				for (int i=0; i < str.Length; i++)
				{
					char c = str[i];
					int j = i;
					if (Char.IsLetter(c))
					{
						start = j;
						while (Char.IsLetter(c) && j < str.Length)
						{
							count++;
							c = str[j];
							j++;
						}
					}
				}
			} else {
				Error.ThrowArgumentError("Parameter str length is 0, cannot return indexes");
			}

			return Tuple.Create(start, count);
		}

		// TO:DO THROW ERROR IF ONLY ONE LETTER SUB STRING
		public static string FindSecondLetterSubstring(string str)
		{
		  string ret = "";
      // Table name in format [tableName] so we just trim (also converts to lower-case)
      // and remove the first and last letter.
      // Find lettersubstring returns index in regard to its input. So we have to add snd.Item1 with sndSearchStart and +1 to account for 0 indexing
		  Tuple<int, int> fst = Strings.FindLetterSubstring(str);
		  int sndSearchStart = fst.Item1+fst.Item2-1;
		  Tuple<int, int> snd = Strings.FindLetterSubstring(str.Substring(sndSearchStart));
		  ret = str.Substring(snd.Item1+sndSearchStart+1, snd.Item2);

      return ret; 
		}

		// Find the first integer in the string
		// Integer can be maximum two ciphers.
		// Returns -1 if no such char
		public static int FindInt(string str)
		{
			int num = -1;
			for(int i=0; i < str.Length; i++)
			{
				char c = str[i];
				if(Char.IsNumber(c))
				{
					if (i == str.Length-1)
					{
						return ((int) c - '0');
					} else {
						string numStr = c.ToString();
						int j = i+1;
						char next = str[j];
						while (Char.IsNumber(next))
						{
							numStr += next;
							j++;
							if (j < str.Length)
							{
								next = str[j];
							} else {
								break;
							}
						}	
						num = Int32.Parse(numStr);
						break;
					}
				}
				i++;
			}
			return num;
		}

		// TO:DO Make FindIntNIndex work for numbers larger than two digits.
		// Find the index of the first integer in the string
		// Returns -1 if no such char
		public static Tuple<int, int> FindIntNIndex(string str)
		{
			int num = -1;
			int index = -1;

			for(int i=0; i < str.Length; i++)
			{
				char c = str[i];
				if(Char.IsDigit(c))
				{
					if (i == str.Length-1)
					{
						return Tuple.Create(c - '0', i);
					} else {
						char fst = c;
						char snd = str[i+1];
						if (Char.IsDigit(snd)) 
						{
							String numStr = fst.ToString() + snd.ToString();
							num = Int32.Parse(numStr);
						} else {
							num = c - '0';
						}

						index = i;
						break;
					}
				}
				i++;
			}

			return Tuple.Create(num, index);
		}
 

		// no need for constructor
		public static string ByteToStr(byte byte_to_print) {
			string byte_str = Convert.ToString(byte_to_print, 2).PadLeft(8, '0');
			return byte_str;
		}

		public static Byte[] Hash(string str) {
			return System.Text.Encoding.UTF8.GetBytes(str);
		}

		public static string HashToString(Byte[] str_id) {
			return System.Text.Encoding.UTF8.GetString(str_id);
		}

		// remove all white space in string and convert all chars to lowercase. 
		public static string Trim(this string input)
		{
		    return new string(input.ToCharArray()
		        .Where(c => !Char.IsWhiteSpace(c))
		        .ToArray()).ToLower();
		}

		// Returns Int32.MinValue on error
		public static int ConvertToInt(string str)
		{
			int ret;

			if (!Int32.TryParse(str, out ret))
			{
				ret = Strings.ERROR;
			} 
			
			return ret;
		}
	}

	public static class Error 
	{
		public const int UNKNOWN_ERR = -1;

		public static void ThrowRosterError() 
		{
			throw new FormatException(String.Format("Invalid format of {0}", Strings.RAID_ROSTER_PATH));	
		}

		public static void ThrowSignUpError() 
		{
			throw new FormatException(String.Format("Invalid format of {0}", Strings.DISCORD_SIGNUP_PATH));	
		}

		public static void NotImplemented(string funcName) 
		{
			throw new Exception(String.Format("Function {0} is not implemented yet", funcName));
		}

		public static void Exception(string msg) 
		{
			throw new Exception(msg);
		}

		public static void ThrowArgumentError(string msg)
		{
			throw new ArgumentException(msg);
		}
	}

	// Small static class for ByteOperations
	public static class ByteOP
	{
		// Returns modified n. 
		public static byte modifyBit(byte n, int p, bool b) 
		{ 
		    int mask = 1 << p; 
		    int return_byte = ((int) n & ~mask) |  
		           			  ((Convert.ToInt32(b) << p) & mask); 
		    return (byte) return_byte;
		}

		// Function to extract k bits from p position 
		// and returns the extracted value as byte 
		public static byte bitExtracted(byte number, int k, int p) 
		{ 
		    return (byte) (((1 << k) - 1) & (number >> (p - 1))); 
		} 

		[DllImport("msvcrt.dll", CallingConvention=CallingConvention.Cdecl)]
		static extern int memcmp(byte[] b1, byte[] b2, long count);

		// compare for bytearrays
		public static bool ByteArrCompare(byte[] b1, byte[] b2)
		{
		    // Validate buffers are the same length.
		    // This also ensures that the count does not exceed the length of either buffer.  
		    return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
		}
		/*
		// Extracts a bit from a byte b
		// Can receive any number :)
		public static bool GetBit<T>(ref T b, ref T bitNumber)
		{
			int bitN = bitNumber as int;
			if (bitN == null) 
			{
				Error.Exception(String.Format("Invalid type {0}", typeof(bitN)));
			}
			long b_l = b as long;
			if (b_l == null) 
			{
				Error.Exception(String.Format("Invalid type {0}", typeof(b_l)));
			}
			return ((b_l & (long) (1 << bitN-1)) != 0);
		}
		*/
		/*
		setBitsToNum
		set bits in N equal to M in the range i-j and return value
		start from 0
		*/
		public static byte setBitsToNum(byte n, byte m, int i, int j)	
		{
	        // number with all 1's 
	        int allOnes = ~0; 
	           
	        // Set all the bits in the left of j 
	        int left = allOnes << (j + 1); 
	           
	        // Set all the bits in the right of j 
	        int right = ((1 << i) - 1); 
	           
	        // Do Bitwise OR to get all the bits  
	        // set except in the range from i to j 
	        int mask = left | right; 
	           
	        // clear bits j through i 
	        int masked_n = n & mask; 
	           
	        // move m into the correct position 
	        int m_shifted = m << i; 
	           
	        // return the Bitwise OR of masked_n  
	        // and shifted_m 
	        // we know we will stay within bounds, so we can explicit cast.
	        return (byte) (masked_n | m_shifted);  
		}

	}

	public static class Printer
	{
		// no need for constructor
		public static void PrintByte(byte byte_to_print) {
			string byte_str = Convert.ToString(byte_to_print, 2).PadLeft(8, '0');
			Console.WriteLine(byte_str);
		}
	}
}