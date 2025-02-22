namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using UnityEngine;

    public static class FloatExtensions
    {
        /// <summary>
        /// Returns a random value between -f and f
        /// </summary>
        public static float RandomMirror(this float f)
        {
            return Random.Range(-f, f);
        }
        /// <summary>
        /// Returns an rounded int version of this float multiplied by 100. 0.47 would return 47
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int ConvertToIntBy100(this float f)
        {
            return (int)(f * 100);
        }

        public static bool WithinUnitLength(this float f)
        {
            return (f <= 1f && f >= 0);
        }

        /// <summary>
        /// Remap a given value between 0 and 1. Based on minValue >= 0 and maxValue <= 1.
        /// </summary>
        /// <param name="value">The value that will be ramaped.</param>
        /// <param name="minValue">The minimum point of the remaping range.</param>
        /// <param name="maxValue">The maximum point of the remaping range.</param>
        /// <returns></returns>
        public static float Remap01(this float value, float minValue, float maxValue)
        {
            value = (value - minValue) / (maxValue - minValue);
            value = Mathf.Clamp01(value);
            return value;
        }
        /// <summary>
        /// Remap a given value from a minimum and a maximum to another.
        /// </summary>
        /// <param name="value"> The value that will be remaped.</param>
        /// <param name="fromMin"> The starting point for the remaping.</param>
        /// <param name="fromMax">The ending point of the remaping.</param>
        /// <param name="toMin">The minimum value the remaping will output.</param>
        /// <param name="toMax">The maximum value the remaping will output.</param>
        /// <returns></returns>
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + Remap01(value, fromMin, fromMax) * (toMax - toMin);
        }

        /// <summary>
        /// Convert a float to string with a period instead of a comma.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringWithPeriod(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

    }
}