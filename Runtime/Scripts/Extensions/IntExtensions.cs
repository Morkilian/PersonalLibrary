namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class IntExtensions
    {
        public static float ConvertToFloatPercentage(this int value)
        {
            return value * 0.01f;
        }
        public static int RandomMirror(this int i)
        {
            return Random.Range(-i, i + 1);
        }
    }
}