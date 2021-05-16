namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class TexturesAndColors
    {
        /// <summary>
        /// Converts a 6 digits hexadecimal code into a color and returns it. \n
        /// If the hexCode's length is different than 6, it returns a black color automatically.
        /// </summary>
        public static Color HexToRGB(string hexCode, float alpha = 1f)
        {
            if (hexCode.Length != 6) return Color.black;

            Vector3 vec = Vector3.zero;
            for (int i = 0; i < 3; i++)
            {
                string hexFraction = hexCode;

                hexFraction = hexFraction.Substring(i * 2, 2);
                vec[i] = System.Convert.ToInt32(hexFraction, 16) / 255f;
            }


            return new Color(vec[0], vec[1], vec[2], alpha);
        }

        /// <summary>
        /// Returns a 2x2 texture of the given color.
        /// </summary>
        /// <param name="backgroundColor">The color to fill the texture with.</param>        
        public static Texture2D BackgroundColor(Color backgroundColor)
        {
            Texture2D texToReturn = new Texture2D(2, 2);
            Color[] pix = new Color[2 * 2];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = backgroundColor;
            }
            texToReturn.SetPixels(pix);
            texToReturn.Apply();
            return texToReturn;
        }
    }

}