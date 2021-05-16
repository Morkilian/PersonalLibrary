namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
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

    }
}