using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Morkilian
{
    /// <summary>
    /// Class handling an incremental or decremental flote within a minimum and maximal value.
    /// The only input needed at runtime is either injecting the current delta time, or switching whether it's incrementing or decrementing.
    /// Can be used for instance to have an object's height travel between 0 or 3, at a certain speed one way, and another speed the other, maybe slowing down when it's about to reach the ground, or when it's about to reach the ceiling value, without having to calculate it ourselves.
    /// </summary>
    [System.Serializable]
    public class IncrementalValue
    {        
        public enum StartingValueOptions { Minimal, Maximal, Middle}
        /// <summary>
        /// The minimal and maximal values of the value. 
        /// </summary>
        [field: SerializeField] public Vector2 MinMaxValues { get; set; } = new Vector2(0, 1);
        /// <summary>
        /// The value at which the value will be sampled. For a linear sampling, keep the curve linear.
        /// </summary>
        [field: SerializeField] public AnimationCurve CurveValue { get; private set; } = new AnimationCurve(new Keyframe(0, 0, 1,1), new Keyframe(1, 1f,1,1));        

        [SerializeField] [Header("Incrementing rates")]private bool m_isIncrementing;
        /// <summary>
        /// The rate at which the value increases.
        /// </summary>
        [field: SerializeField] [field:Min(0)] public float IncrementalRate { get; set; } = 0.2f;
        /// <summary>
        /// The rate at which the value decreases.
        /// </summary>
        [field: SerializeField] [field: Min(0)] public float DecrementalRate { get; set; } = 0.1f;
        /// <summary>
        /// Wether the value's incremental and decremental rate will be affected by rates.
        /// </summary>
        [SerializeField] [Header("Rate curves")]private bool m_useCurves = false;
        /// <summary>
        /// The curve affecting the incremental rate
        /// </summary>
        [field: SerializeField] public AnimationCurve IncrementalCurve { get; set; } = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, .2f));
        /// <summary>
        /// The curve affecting the decremental rate.
        /// </summary>
        [field: SerializeField]public AnimationCurve DecrementalCurve { get; set; } = new AnimationCurve(new Keyframe(0, .2f), new Keyframe(1, 1));
        private float lastDirectionalChangeTime;
        private float lastDirectionalChangeNormalizedValue;
        private float currentNormalizedValue;
        private float valueLastCheck;

        /// <summary>
        /// Wether to use the incremental curves or not.
        /// Remember to use the UpdateValue method every frame if you're using the incremental ratio curve.
        /// </summary>
        public bool UseIncrementalCurves 
        { get => m_useCurves; set { m_useCurves = value; } }

        /// <summary>
        /// Whether the value is incrementing towards the maximal value or decrementing towards the minimal value.
        /// </summary>
        public bool IsIncrementing { get => m_isIncrementing;
            set
            {
                if(UseIncrementalCurves == false && m_isIncrementing != value)
                {
                    lastDirectionalChangeTime = Time.time;
                    lastDirectionalChangeNormalizedValue = currentNormalizedValue;
                }
                m_isIncrementing = value;
            }
        }
        /// <summary>
        /// The current value between the min and max value.
        /// Remember to use the UpdateValue method every frame if you're using the incremental ratio curve.
        /// </summary>
        public float CurrentValue
        {
            get
            {
                if (UseIncrementalCurves)
                    return EvaluateValue(currentNormalizedValue);
                else
                {
                    if ((IsIncrementing == true && currentNormalizedValue != 1) || (IsIncrementing == false && currentNormalizedValue != 0))
                    {
                        float timeSpent = Time.time - lastDirectionalChangeTime;
                        currentNormalizedValue = Mathf.Clamp01(lastDirectionalChangeNormalizedValue + timeSpent * (IsIncrementing ? IncrementalRate : -DecrementalRate));
                        return EvaluateValue(currentNormalizedValue);
                    }
                    return IsIncrementing ? MinMaxValues.y : MinMaxValues.x;
                }                
            }
            set
            {                
                currentNormalizedValue = Mathf.Clamp01(Mathf.InverseLerp(MinMaxValues.x, MinMaxValues.y, value));
            }
        }
        /// <summary>
        /// The current value in 0-1 range.
        /// </summary>
        public float CurrentNormalizedValue
        {
            get => currentNormalizedValue;
            set
            {
                currentNormalizedValue = Mathf.Clamp01(value);
                lastDirectionalChangeNormalizedValue = currentNormalizedValue;
                lastDirectionalChangeTime = Time.time;
            }
        }
        /// <summary>
        /// Checks whether the value has changed since the last time this has been checked
        /// </summary>
        public bool HasValueChanged
        {
            get
            {
                bool hasChanged = CurrentValue != valueLastCheck;
                valueLastCheck = CurrentValue;
                return hasChanged;
            }
        }

        #region Constructors
        public IncrementalValue()
        {
            m_isIncrementing = false;
            UseIncrementalCurves = false;
        }        
        public IncrementalValue(Vector2 minMax, float incrementalRate = 1f, float decrementalRate = 1f, bool isIncrementing = true, StartingValueOptions startingValueOption = StartingValueOptions.Minimal)
        {
            MinMaxValues = minMax;
            m_isIncrementing = isIncrementing;
            UseIncrementalCurves = false;
            IncrementalRate = incrementalRate;
            DecrementalRate = decrementalRate;
            switch (startingValueOption)
            {
                case StartingValueOptions.Minimal:
                    CurrentNormalizedValue = minMax.x;
                    break;
                case StartingValueOptions.Maximal:
                    CurrentNormalizedValue = minMax.y;
                    break;
                case StartingValueOptions.Middle:
                    CurrentNormalizedValue = minMax.Lerp(0.5f);
                    break;
            }            
        }

        public IncrementalValue(Vector2 minMax, float incrementalRate = 1f, float decrementalRate = 1f, bool isIncrementing = true, float startingNormalizedValue = 0.5f)
        {
            MinMaxValues = minMax;
            m_isIncrementing = isIncrementing;
            UseIncrementalCurves = false;
            IncrementalRate = incrementalRate;
            DecrementalRate = decrementalRate;
            currentNormalizedValue = startingNormalizedValue;            
        }
        public IncrementalValue(Vector2 minMax, float incrementalRate = 1f, float decrementalRate = 1f, float startingValue = 0.5f, bool isIncrementing = true)
        {
            MinMaxValues = minMax;
            m_isIncrementing = isIncrementing;
            UseIncrementalCurves = false;
            IncrementalRate = incrementalRate;
            DecrementalRate = decrementalRate;
            currentNormalizedValue = Mathf.Clamp01(minMax.InverseLerp(startingValue));
        }
        #endregion

        private float EvaluateValue(float currentRatioValue)
        {            
            return MinMaxValues.Lerp(CurveValue.Evaluate(currentRatioValue));
        }
        public void UpdateValue(float deltaTime)
        {
            if (UseIncrementalCurves == false)
                return;
            //Update value if it needs being applied
            if ((IsIncrementing == true && currentNormalizedValue != MinMaxValues.y)
                || (IsIncrementing == false && currentNormalizedValue != MinMaxValues.x))
            {                
                if (IsIncrementing)
                    deltaTime *= IncrementalRate * IncrementalCurve.Evaluate(currentNormalizedValue);
                else
                    deltaTime *= DecrementalRate * IncrementalCurve.Evaluate(currentNormalizedValue)*(-1);

                currentNormalizedValue = Mathf.Clamp01(currentNormalizedValue + deltaTime);
                //currentValue = Mathf.Clamp(currentValue + deltaTime, MinMaxValues.x, MinMaxValues.y);                
            }
            
        }
    }
}