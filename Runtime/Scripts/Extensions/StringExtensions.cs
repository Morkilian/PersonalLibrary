using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Morkilian.Helper
{
    public static class StringExtensions 
    {
        /// <summary>
        /// Returns a substring from the "lastIndex - <paramref name="startIndexFromEnd"/>" to the last char. \n
        /// For instance, "Friendship" with a <paramref name="startIndexFromEnd"/> of 3 would return "ship".        
        /// </summary>
        /// <param name="startIndexFromEnd"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static string SubstringFromEnd(this string s, int startIndexFromEnd)
        {            
            if (string.IsNullOrEmpty(s) || s.Length < startIndexFromEnd)
            {
                Debug.LogError("[SubstringFromEnd] Couldn't process the string, returning null.");
                return null;
            }

            return s.Substring(s.Length - startIndexFromEnd - 1, startIndexFromEnd + 1);
        }
        /// <summary>
        /// Reverse the string order. "Hello world!" becomes "!dlrow olleH"
        /// </summary>        
        public static string Reverse(this string s)
        {
            string newString = "";
            for (int i = s.Length; i >=0 ; --i)
            {
                newString += s[i];
            }
            return newString;
        }

        /// <summary>
        /// Replaces a character in a string with another. \n
        /// <!> WARNING <!> Replacing many characters may require a different, less expensive method .
        /// </summary>        
        public static string ReplaceChar(this string s, int index, char newChar)
        {
            if (string.IsNullOrEmpty(s) || s.Length < index)
                return null;
            char[] sb = s.ToCharArray();
            sb[index] = newChar;
            return sb.ToString();
        }
    } 
}
