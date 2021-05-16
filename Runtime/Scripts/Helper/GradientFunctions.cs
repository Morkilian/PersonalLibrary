namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public enum FunctionsCurves { Xexp2, Xexp3, Xexp5, linear }
    public static class GradientFunctions 
    {
        /// <summary>
        /// Transforms a [0,1] value into a smooth one with an smooth in (value goes from 0 to 1).
        /// </summary>
        /// <param name="t">The [0,1] value to smooth. </param>
        /// <param name="function">Kind of curve to smooth the value.</param>
        /// <returns>The smoothed value.</returns>
        public static float EasyIn(float t, FunctionsCurves function)
        {
            switch (function)
            {
                case FunctionsCurves.Xexp2:
                    return t * t;
                case FunctionsCurves.Xexp3:
                    return t * t * t;
                case FunctionsCurves.Xexp5:
                    return t * t * t * t * t;
                case FunctionsCurves.linear:
                    return t;
            }
            return 42f;
        }

        /// <summary>
        /// Transforms a [0,1] value into a smooth one with an smooth out (value goes from 1 to 0).
        /// </summary>
        /// <param name="t">The [0,1] value to smooth. </param>
        /// <param name="function">Kind of curve to smooth the value.</param>
        /// <returns>The smoothed value.</returns>
        public static float EasyOut(float t, FunctionsCurves function)
        {
            switch (function)
            {
                case FunctionsCurves.Xexp2:
                    return Flip(Mathf.Sqrt(t));
                case FunctionsCurves.Xexp3:
                    return Flip(Mathf.Pow(t, 0.333f));
                case FunctionsCurves.Xexp5:
                    return Flip(Mathf.Pow(t, 0.2f));
                case FunctionsCurves.linear:
                    return Flip(t);
            }
            return 42f;
        }

        /// <summary>
        /// Transforms a [0,1] value into a smooth one with a hard in (value goes from 0 to 1).
        /// </summary>
        /// <param name="t">The [0,1] value to smooth. </param>
        /// <param name="function">Kind of curve to smooth the value.</param>
        /// <returns>The smoothed value.</returns>
        public static float HardIn(float t, FunctionsCurves function)
        {
            switch (function)
            {
                case FunctionsCurves.Xexp2:
                    return (Mathf.Sqrt(t));
                case FunctionsCurves.Xexp3:
                    return (Mathf.Pow(t, 0.333f));
                case FunctionsCurves.Xexp5:
                    return (Mathf.Pow(t, 0.2f));
                case FunctionsCurves.linear:
                    return (t);
            }
            return 42f;
        }

        /// <summary>
        /// Transforms a [0,1] value into a smooth one with a hard out (value goes from 1 to 0).
        /// </summary>
        /// <param name="t">The [0,1] value to smooth. </param>
        /// <param name="function">Kind of curve to smooth the value.</param>
        /// <returns>The smoothed value.</returns>
        public static float HardOut(float t, FunctionsCurves function)
        {
            switch (function)
            {
                case FunctionsCurves.Xexp2:
                    return Flip(t * t);
                case FunctionsCurves.Xexp3:
                    return Flip(t * t * t);
                case FunctionsCurves.Xexp5:
                    return Flip(t * t * t * t * t);
                case FunctionsCurves.linear:
                    return Flip(t);
            }
            return 42f;
        }
        /// <summary>
        /// Lerps a smooth value between 0 and 1 with the given min and max
        /// </summary>
        /// <param name="t">The value to lerp</param>
        /// <param name="min">The step at which the value will return 0</param>
        /// <param name="max">The step at which the value will return 1</param>
        /// <returns>The smoothed value</returns>
        public static float SmoothStep(float t, float min, float max)
        {
            t = (t - min) / (1 - max);
            float smooth = Mathf.Clamp01(3 * t * t - 2 * t * t * t);
            if (max > min)
                return smooth;
            else return 1 - smooth;
        }

        /// <summary>
        /// Lerps a smooth value between 0 and 1 with the given min max using the cosinus function 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>The smoothed value</returns>
        public static float CosStep(float t, float min, float max)
        {
            float direction = max > min ? -1 : 1;
            float scale = Mathf.Abs(max - min) * 0.5f;
            return direction * Mathf.Cos(t * Mathf.PI) * scale - scale + Mathf.Max(min, max);
        }
        /// <summary>
        /// Flips the value with a one minus.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Flip(float t)
        { return 1 - t; }
    }

}