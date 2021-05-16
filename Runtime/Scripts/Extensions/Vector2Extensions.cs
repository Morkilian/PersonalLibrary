namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns the ratio in which the value inputted is situated between the vector values.
        /// That means the inverse lerp of 3 in a (2,5) vector would return 0.33
        /// </summary>
        public static float InverseLerp(this Vector2 v, float value, bool saturate = false)
        {
            float toReturn = Mathf.InverseLerp(v.x, v.y, value);
            return saturate ? Mathf.Clamp01(toReturn) : toReturn;
        }
        /// <summary>
        /// Returns the resultant value of a given ratio in this vector. For instance, Lerp(0.5f) of a vector2(1,3) would return 2.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="ratio">The lerp ratio.</param>
        /// <param name="clamp">Wether or not to clamp the value inside the vector members.</param>        
        public static float Lerp(this Vector2 v, float ratio, bool clamp = true)
        {

            return clamp ?
                Mathf.Lerp(v.x, v.y, ratio) :
                Mathf.LerpUnclamped(v.x, v.y, ratio);
        }
        /// <summary>
        /// Returns a random value between the x and y value of this vector2.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Random(this Vector2 v)
        {
            return v.Lerp(UnityEngine.Random.value);
        }

        public static Vector2 LimitMagnitude(this Vector2 v, float maxLength)
        {
            float squareLength = v.x * v.x + v.y * v.y;
            if ((squareLength > maxLength * maxLength) && (v.sqrMagnitude > 0))
            {
                float ratio = maxLength / Mathf.Sqrt(squareLength);
                v.x *= ratio;
                v.y *= ratio;
            }
            return v;
        }

        /// <summary>
        /// Is the value input between the x and Y value of this vector? [Extremes included]
        /// </summary>
        /// <param name="value"> The value to compare</param>
        /// <returns>Wether it's in between or not</returns>
        public static bool WithinRange(this Vector2 v, float value)
        {
            if (v.y > v.x)
                return (value >= v.x && value <= v.y);
            else return value <= v.x && value >= v.y;
        }

        public static float Length(this Vector2 v)
        {
            return Mathf.Abs(v.y - v.x);
        }

        //https://stackoverflow.com/questions/45270723/how-to-rotate-vector
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            degrees *= Mathf.Deg2Rad;
            return new Vector2(
                (float)(v.x * Mathf.Cos(degrees) - v.y * Mathf.Sin(degrees)),
                (float)(v.x * Mathf.Sin(degrees) + v.y * Mathf.Cos(degrees))
            );
        }

        public static Vector2 Rotate(this Vector2 v, float degrees, Vector2 pivot)
        {
            degrees *= Mathf.Deg2Rad;
            v -= pivot;
            return new Vector2(
                (float)(v.x * Mathf.Cos(degrees) - v.y * Mathf.Sin(degrees)),
                (float)(v.x * Mathf.Sin(degrees) + v.y * Mathf.Cos(degrees))
            ) + pivot;
        }

    }

}