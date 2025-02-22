using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace Morkilian
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

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }
        public static Color ToColor(this string color)
        {
            if (color.StartsWith("#", StringComparison.InvariantCulture))
            {
                color = color.Substring(1); // strip #
            }

            if (color.Length == 6)
            {
                color += "FF"; // add alpha if missing
            }

            var hex = Convert.ToUInt32(color, 16);
            var r = ((hex & 0xff000000) >> 0x18) / 255f;
            var g = ((hex & 0xff0000) >> 0x10) / 255f;
            var b = ((hex & 0xff00) >> 8) / 255f;
            var a = ((hex & 0xff)) / 255f;

            return new Color(r, g, b, a);
        }
    } 
}
