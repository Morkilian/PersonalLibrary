using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian
{
    /// <summary>
    /// Implicit cast returns a random value between min and max, evaluated on the curve.
    /// This is useful when we've got a minimum and maximum, but we want values to converge towards the max, or the min, or the middle
    /// </summary>
    [System.Serializable]
    public class MinMaxCurved
    {
        [SerializeField] private Vector2 minMax;// { get; private set; }
        public Vector2 MinMax { get => minMax; set { minMax = value; } }
        public float y { get => minMax.y; set { minMax.y = value; } }
        public float x { get => minMax.x; set { minMax.x = value; } }
        /// <summary>
        /// Default keys are (0,0) and (1,1)
        /// </summary>
        [SerializeField] private AnimationCurve curve;// { get; private set; }
        public AnimationCurve Curve => curve;
        /// <summary>
        /// A random value between min and max, evaluated with the curve.
        /// </summary>
        public float RandomValue => Mathf.Lerp(MinMax.x, MinMax.y, curve.Evaluate(Random.value));
        public static AnimationCurve DEFAULT_CURVE => new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        #region Constructors
        public MinMaxCurved()
        {
            minMax = new Vector2(0, 1);
            curve = DEFAULT_CURVE;
        }
        public MinMaxCurved(float min, float max)
        {
            minMax = new Vector2(min, max);
            curve = DEFAULT_CURVE;
        }
        public MinMaxCurved(float min, float max, AnimationCurve curve)
        {
            minMax = new Vector2(min, max);
            this.curve = new AnimationCurve(curve.keys);
        }

        public MinMaxCurved(Vector2 minMax)
        {
            this.minMax = minMax;
            curve = DEFAULT_CURVE;
        }
        public MinMaxCurved(Vector2 minMax, AnimationCurve curve)
        {
            this.minMax = minMax;
            this.curve = new AnimationCurve(curve.keys);
        }
        #endregion

        public float Evaluate(float t, bool clamp = true)
        {
            if (clamp)
                return Mathf.Lerp(MinMax.x, MinMax.y, Mathf.Clamp01(curve.Evaluate(t)));
            else
                return Mathf.LerpUnclamped(MinMax.x, MinMax.y, curve.Evaluate(t));
        }

        /// <summary>
        /// Calculates the Y curve value with the given input.
        /// \n e.g. if this cas a minmax value of (1,5), and the curve is a normalized easy in hard out (exponential), and you give 3 as input, it'd return something around 0.35f. If the curve was linear, it'd return 0.5
        /// </summary>
        public float InverseLerp(float t)
        {
            float normalizedInput = Mathf.InverseLerp(MinMax.x, MinMax.y, t);
            return Mathf.Clamp01(curve.Evaluate(normalizedInput));
        }

        public static implicit operator float(MinMaxCurved mm) => mm.RandomValue;
    } 
}

