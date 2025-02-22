using Morkilian;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedLerp<T>
{

    // ------- Time ----------
    private float startTime = 0;
    //public bool UseCurve = false;
    //public AnimationCurve lerpCurve = null;
    //public T startValue;
    //public T endValue;

    [System.Serializable]
    public abstract class Data
    {
        public T StartValue;
        public T EndValue;
        public float During = 0;
        public bool TrueTime = false;
        public bool UseCurve = false;
        public AnimationCurve LerpCurve;

        public abstract T GetLerpedValue(float normalizedTime);
    }
    private Data data;
    public AdvancedLerp(Data data)
    {
        this.startTime = Time.time;
        this.data = data;
    }

    /// <summary>
    /// Is this currently lerping?
    /// </summary>
    public bool IsWorking => CurrentProgress<1;
    public bool HasFinished => IsWorking == false;
    public float RemainingTime
    {
        get
        {
            if (data.TrueTime) return data.During - (Time.unscaledTime - startTime);
            else return data.During - (Time.time - startTime);
        }
    }

    /// <summary>
    /// Linear normalized value in which the lerp progression is in time (0-> 1)
    /// </summary>
    public float CurrentProgress
    {
        get
        {
            if (data.During == 0) return 1;
            if (data.TrueTime) return Mathf.Clamp01((Time.unscaledTime - this.startTime) / data.During);
            return Mathf.Clamp01((Time.time - this.startTime) / data.During);
        }
    }


    /// <summary>
    /// Current value lerped at this time (value between startFloat and endFloat)
    /// </summary>
    public T CurrentValue
    {
        get
        {
            float normalizedTime = this.CurrentProgress;
            return GetCurrentPossibleValue(normalizedTime);
        }
    }
    /// <summary>
    /// When using a LerpFloat as input, this is the current inverted value lerped, as if time was running backwards, starting from the end float
    /// </summary>
    public T CurrentReversedFloatValue
    {
        get
        {
            float normalizedTime = 1f - this.CurrentProgress;
            return GetCurrentPossibleValue(normalizedTime);
        }
    }

    protected T GetCurrentPossibleValue(float normalizedTime)
    {
        if (data.UseCurve == false || data.LerpCurve == null)
        {
            return data.GetLerpedValue(normalizedTime);
        }
        float valueCurve = data.LerpCurve.Evaluate(normalizedTime);
        return data.GetLerpedValue(valueCurve);
    }
}