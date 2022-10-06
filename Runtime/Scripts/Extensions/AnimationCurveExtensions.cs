using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian.Helper
{
    public static class AnimationCurveExtensions
    {
        /// <summary>
        /// Horizontaly reverses the curve
        /// </summary>
        /// <param name="curve"></param>
        public static void ReverseCurve(this AnimationCurve curve)
        {
            float longestX = curve[curve.length-1].time;
            float shortestX = curve[0].time;
            Keyframe[] newKeys = new Keyframe[curve.length];
            
            for (int i = 0; i < newKeys.Length; i++)
            {
                Keyframe refKey = curve[curve.length - i - 1];
                //Inverse in and out
                //Negate tangents (from left to right, what was going up by 2 should going down by 2 if inverted)
                Keyframe newKey = new Keyframe(longestX - refKey.time + shortestX, refKey.value, -refKey.outTangent, -refKey.inTangent, refKey.outWeight, refKey.inWeight);
                newKey.weightedMode = refKey.weightedMode;
                newKeys[i] = newKey;
            }

            curve.keys = newKeys;
        }
    } 
}
