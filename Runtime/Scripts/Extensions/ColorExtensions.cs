using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Morkilian
{
    public static class ColorExtensions 
    {
        /// <summary>
        /// Offsets the current color by the given hue, saturation and value.
        /// Hue is looped between 0-1, saturation and value are saturated.
        /// </summary>
        public static Color OffsetHSV(this Color color, float hue, float saturation, float value)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            h = (h + hue) % 1;
            s = Mathf.Clamp01(s + saturation);
            v = Mathf.Clamp01(v + value);
            return Color.HSVToRGB(h, v, s);
        }
    }
}