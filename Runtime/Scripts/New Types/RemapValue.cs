using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Morkilian
{
    /// <summary>
    /// Class to handle remapping of given floats
    /// </summary>
    [System.Serializable]
    public class RemapValue 
    {
        /// <summary>
        /// The minimun and maximum starting values
        /// </summary>
        [field: SerializeField] public Vector2 MinMaxFrom { get; set; } = new Vector2(0, 1);
        /// <summary>
        /// The new minimum and maximum values
        /// </summary>
        [field: SerializeField] public Vector2 MinMaxTo { get; set; } = new Vector2(0, 1);
        /// <summary>
        /// The normalized remapping value curve
        /// </summary>
        [field: SerializeField] public AnimationCurve MinToCurve { get; set; } = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
        /// <summary>
        /// Remaps the given value to the new min and max to.
        /// If it needs to be unclamped, it won't be evaluated with the curve.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="clamp"></param>
        /// <returns></returns>
        public float Evaluate(float from, bool clamp = true)
        {
            float normalizedValue = MinMaxFrom.InverseLerp(from, clamp);
            float evaluatedValue = MinToCurve.Evaluate(normalizedValue);
            return MinMaxTo.Lerp(evaluatedValue, clamp);
        }
    }
}