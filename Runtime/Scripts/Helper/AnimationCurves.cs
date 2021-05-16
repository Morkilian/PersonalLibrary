namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class AnimationCurves
    {
        public static readonly AnimationCurve LinearCurve = new AnimationCurve(new Keyframe(0f, 0f, 1, 1), new Keyframe(1f, 1f, 1, 1));
        public static readonly AnimationCurve SmoothDefaultCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        public static readonly AnimationCurve HardInDefaultCurve = new AnimationCurve(new Keyframe(0f, 0f, 2, 2), new Keyframe(1f, 1f));
        public static readonly AnimationCurve EasyInDefaultCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f, 0, 0));

        /// <summary>
        /// Outputs a log with the information of every keyframe of an animation curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="debug">Wether or not to output a Debug.Log</param>
        public static string DebugAnimationCurveInfo(AnimationCurve curve, bool debug = true)
        {
            string toShow = "List of keyframes: ";
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe key = curve.keys[i];
                toShow += "\n Point 0:";
                toShow += "Pos: " + key.time;
                toShow += ", Value: " + key.value;
                toShow += ", InTan: " + key.inTangent;
                toShow += ", OutTan: " + key.outTangent;
                toShow += ", InWeight: " + key.inWeight;
                toShow += ", OutWeight: " + key.outWeight;
            }
            if (debug)
            {
                Debug.Log(toShow); 
            }
            return toShow;
        }
    }

}