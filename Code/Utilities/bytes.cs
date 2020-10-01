using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;

namespace Utilities // utilities
{	
	// Small static class for ByteOperations
	public static class Bytes
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

		// Function to extract k bits from p position 
		// and returns the extracted value as int
		// p index starts at 1 NOT 0.
		public static int bitExtracted(int number, int k, int p) 
		{ 
		    return (((1 << k) - 1) & (number >> (p - 1))); 
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
		public static int setBitsToNum(int n, int m, int i, int j)	
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
	        return (masked_n | m_shifted);  
		}

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
}