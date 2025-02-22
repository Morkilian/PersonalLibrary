namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class RangedCurve
    {
        [SerializeField] private Vector2 m_MinMaxValue = new Vector2(1, 2);
        [SerializeField] private AnimationCurve m_ChanceCurve = new AnimationCurve(AnimationCurve.Linear(0f, 0f, 1f, 1f).keys);

        public RangedCurve()
        {
            m_MinMaxValue = new Vector2(0, 1);
            m_ChanceCurve = new AnimationCurve(AnimationCurve.Linear(0f, 0f, 1f, 1f).keys);

        }

        public RangedCurve(Vector2 minMax, AnimationCurve curve)
        {
            m_MinMaxValue = minMax;
            m_ChanceCurve = curve;
        }

        public RangedCurve(Vector2 minMax)
        {
            this.m_MinMaxValue = minMax;
        }

        public RangedCurve(float min, float max)
        {
            m_MinMaxValue = new Vector2(min, max);
        }

        public float RandomValue => m_MinMaxValue.Lerp(m_ChanceCurve.Evaluate(Random.value));
        public Vector2 MinMax => m_MinMaxValue;

        public static implicit operator Vector2(RangedCurve ti)
        {
            return ti.m_MinMaxValue;
        }

        public static implicit operator float(RangedCurve rc)
        {
            return rc.RandomValue;
        }
    }

}