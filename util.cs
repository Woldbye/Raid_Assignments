using System;
using Generics;
using System.Runtime.InteropServices;
using System.Linq;

namespace Util // utilities
{	
	public static class Strings
	{
		public const string RAID_ROSTER_PATH = "raid_roster.txt";
		public static string[] CLASS_TO_STR = {"Druid", "Hunter", "Mage", "Priest", "Rogue", "Shaman", "Warlock", "Warrior"};
		public static string[] ROLE_TO_STR = {"Tank", "Healer", "Melee", "Ranged"};
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
	}

	public static class Error 
	{
		public const int UNKNOWN_ERR = -1;

		public static void ThrowRosterError() 
		{
			throw new FormatException(String.Format("Invalid format of {0}", Strings.RAID_ROSTER_PATH));	
		}

		public static void NotImplemented(string funcName) 
		{
			throw new Exception(String.Format("Function {0} is not implemented yet", funcName));
		}

		public static void Exception(string msg) 
		{
			throw new Exception(msg);
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