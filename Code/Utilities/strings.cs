using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Utilities // utilities
{ 
  public static class Strings
  {
    // Returns the start index 
    // and total character count of letter substring.
    public static Tuple<int, int> FindLetterSubstring(string str)
    {

      int start = 0;
      int count = 0;
      if (!String.IsNullOrWhiteSpace(str) && str[0].IsAscii())
      {
        for (int i=0; i < str.Length; i++)
        {
          char c = str[i];
          int j = i; 
          if (Char.IsLetter(c))
          {
            start = j;
            while (Char.IsLetter(c))
            {
              count++;
              if (j >= str.Length)
              {
                break;
              }
              c = str[j];
              j++;
            }
            break;
          }
        }
        count--;
      } else {
        Exceptions.ThrowArgument("Parameter str length is 0, cannot return indexes");
      }

      return Tuple.Create(start, count);
    }

    // TO:DO THROW ERROR IF ONLY ONE LETTER SUB STRING
    public static string FindSecondLetterSubstring(string str)
    {
      string ret = "";
      if (!String.IsNullOrWhiteSpace(str) && str[0].IsAscii())
      {
        // Table name in format [tableName] so we just trim (also converts to lower-case)
        // and remove the first and last letter.
        // Find lettersubstring returns index in regard to its input. So we have to add snd.Item1 with sndSearchStart and +1 to account for 0 indexing
        Tuple<int, int> fst = Strings.FindLetterSubstring(str);
        int sndSearchStart = fst.Item1+fst.Item2;
        if (sndSearchStart > 0)
        {
          Tuple<int, int> snd = Strings.FindLetterSubstring(str.Substring(sndSearchStart));
          ret = str.Substring(snd.Item1+sndSearchStart, snd.Item2);
        }
      }
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
        ret = LookUp.ERROR;
      } 
      
      return ret;
    }
  }
}