namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Vector2IntExtensions 
    {
        public enum InterpolationMode { Floor, Round, Ceil}
        public static int Lerp(this Vector2Int v, float ratio, bool clamp = true, InterpolationMode interMode = InterpolationMode.Round)
        {
            float temp = Mathf.Lerp(v.x, v.y, ratio);
            int toReturn;
            switch (interMode)
            {
                case InterpolationMode.Floor:
                    toReturn = Mathf.FloorToInt(temp);
                    break;
                case InterpolationMode.Round:
                    toReturn = Mathf.RoundToInt(temp);
                    break;
                case InterpolationMode.Ceil:
                    toReturn = Mathf.CeilToInt(temp);
                    break;
                default:
                    //Logger.DebugError("Tried to use a different interpolation mode.");
                    return -1;                    
            }
            if (clamp)
            {
                return Mathf.Clamp(toReturn, v.x, v.y);
            }
            else return toReturn;
        }

        //TO DO: take into account the case when we don't saturate AND bigger isn't Y.
        public static float InverseLerp(this Vector2Int v, int value, bool saturate = false)
        {
            int bigger = Mathf.Max(v.x, v.y);
            int smaller = bigger == v.x ? v.y : v.x;
            bigger -= smaller;
            float toReturn = (float)value / bigger;
            if (saturate) toReturn = Mathf.Clamp01(toReturn);
            if (smaller == v.y) toReturn = 1 - toReturn;
            return toReturn;
        
        }

        public static int Random(this Vector2Int v)
        {
            return UnityEngine.Random.Range(v.x, v.y);
        }
    }

}