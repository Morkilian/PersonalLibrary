using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian
{
    /// <summary>
    /// Utility class with a value that lerps automatically towards a given target
    /// </summary>
    [System.Serializable]
    public class LerpedValue 
    {
        public enum Lerping 
        { 
            /// <summary>
            /// The current value lerps towards the target. This creates a hard-in/easy-out lerping towards the target value
            /// </summary>
            CurrentTargetLerp, 
            /// <summary>
            /// A linear lerping towards target value
            /// </summary>
            Linear 
        }

        #region Variables and Properties
        /// <summary>
        /// Method by which the current value lerps towards target value
        /// </summary>
        [SerializeField] private Lerping m_lerpingMethod;

        [SerializeField][Min(0f)] private float m_lerpingSpeed = 1f;

        [SerializeField] private float currentValue = 0;
        [SerializeField] private float targetValue = 1;
        private float lastTimeValueWasAccessed;
        /// <summary>
        /// The speed at which the value lerps towards the target. Behaviour changes depending on the Lerping Method.
        /// </summary>
        public float LerpingSpeed
        {
            get
            {
                if (m_lerpingSpeed < 0)
                {
                    Debug.LogError("[LerpedValue]: You've assigned a negative lerping speed! This is not allower, value has been set to positive.");
                    m_lerpingSpeed = Mathf.Abs(m_lerpingSpeed);
                }
                return m_lerpingSpeed;
            }
            set
            {
                m_lerpingSpeed = value;
            }
        }
        /// <summary>
        /// The current lerped value.
        /// </summary>
        public float CurrentValue
        {
            get
            {
                UpdateCurrentValue();
                return currentValue;
            }
            set
            {
                currentValue = value;
                lastTimeValueWasAccessed = Time.time;
            }
        }
        /// <summary>
        /// The target value to progress towards to
        /// </summary>
        public float TargetValue
        {
            //Auto get for now
            get
            {
                return targetValue;
            }
            set
            {
                UpdateCurrentValue(); //To prevent a big gap from affecting the newer target value
                targetValue = value;
                lastTimeValueWasAccessed = Time.time;
            }
        }

        public Lerping LerpingMethod
        {
            get => m_lerpingMethod;
            set { m_lerpingMethod = value; }
        }
        /// <summary>
        /// Has this value changed since the last time it was accessed.
        /// </summary>
        public bool NeedsToUpdateValue => currentValue != targetValue;

        #endregion

        #region Constructors
        public LerpedValue()
        {
        }
        public LerpedValue(float currentValue, float targetValue, float lerpingSpeed, Lerping lerpingMethod)
        {
            this.currentValue = currentValue;
            this.targetValue = targetValue;
            m_lerpingSpeed = Mathf.Abs(lerpingSpeed);
            m_lerpingMethod = lerpingMethod;
        }
        #endregion

        #region Private methods
        private void UpdateCurrentValue()
        {
            if (NeedsToUpdateValue == false || lastTimeValueWasAccessed == Time.time) //Has already accessed and updated the value{
                return;
            
            float timeElapsed = Time.time - lastTimeValueWasAccessed; //Most of the time it's just delta time
            switch (m_lerpingMethod)
            {
                case Lerping.CurrentTargetLerp: //Apply delta time
                    currentValue = Mathf.Lerp(currentValue, targetValue, timeElapsed * LerpingSpeed);
                    break;
                case Lerping.Linear: //Linearly progress towards target value
                    float progress = LerpingSpeed * timeElapsed;
                    float currentDifference = targetValue - currentValue;
                    float sign = Mathf.Sign(currentDifference);
                    currentDifference = Mathf.Abs(currentDifference);
                    if (progress > currentDifference) // Overshooted the target, therefore just clamp to value
                    {
                        currentValue = targetValue;
                    }
                    else
                    {
                        currentValue += progress * sign;
                    }
                    break;
            }
            //If it's close enough, clamp it
            if (currentValue != targetValue && Mathf.Abs(targetValue - currentValue) < 0.00001f)
            {
                currentValue = targetValue;
            }

            lastTimeValueWasAccessed = Time.time;
        } 
        #endregion
    }
}