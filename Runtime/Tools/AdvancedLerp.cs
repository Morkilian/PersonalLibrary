using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedLerp
{

    // ------- Time ----------
    private float startTime = 0;
    private float during = 0;
    private bool trueTime = false;

    // ----- Type Lerp -----
    private AnimationCurve lerpCurve = null;

    // ---- Attribute float ------

    private float startFloat = 0;
    private float endFloat = 0;
    private float differenceFloat = 0;

    //----- Attribute Vector ------

    private Vector3 startVector;
    private Vector3 endVector;
    private Vector3 differenceVector;

    private AdvancedLerp(float startTime, float during, AnimationCurve lerpCurve)
    {
        this.startTime = startTime;
        this.during = during;
        this.lerpCurve = lerpCurve;
    }

    public AdvancedLerp(float startFloat, float endFloat, float startTime, float during, AnimationCurve lerpCurve) : this(startTime, during, lerpCurve)
    {
        this.startFloat = startFloat;
        this.endFloat = endFloat;
        this.differenceFloat = this.endFloat - this.startFloat;
    }

    public AdvancedLerp(Vector3 startVector, Vector3 endVector, float startTime, float during, AnimationCurve lerpCurve) : this(startTime, during, lerpCurve)
    {
        this.startVector = startVector;
        this.endVector = endVector;
        this.differenceVector = this.endVector - this.startVector;
    }

    public AdvancedLerp(LerpFloat struc)
    {
        this.startTime = Time.time;
        this.during = struc.during;
        if (struc.useLerpCurve)
            this.lerpCurve = struc.lerpCurve;
        this.startFloat = struc.startFloat;
        this.endFloat = struc.endFloat;
        this.differenceFloat = this.endFloat - this.startFloat;
    }

    public AdvancedLerp(LerpFloat struc, bool trueTime)
    {
        if (!trueTime) this.startTime = Time.time;
        else this.startTime = Time.unscaledTime;
        this.trueTime = trueTime;

        this.during = struc.during;
        if (struc.useLerpCurve)
            this.lerpCurve = struc.lerpCurve;
        this.startFloat = struc.startFloat;
        this.endFloat = struc.endFloat;
        this.differenceFloat = this.endFloat - this.startFloat;
    }

    public AdvancedLerp(LerpVector struc)
    {
        this.startTime = Time.time;
        this.during = struc.during;
        if (struc.useLerpCurve)
            this.lerpCurve = struc.lerpCurve;
        this.startVector = struc.startVector;
        this.endVector = struc.endVector;
        this.differenceVector = this.endVector - this.startVector;
    }


    [System.Serializable]
    public class LerpFloat
    {
        public float during = 0;
        public bool useLerpCurve = false;
        public AnimationCurve lerpCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
        public float startFloat = 0;
        public float endFloat = 1;

        public LerpFloat() {
            endFloat = 1;

        }
        public LerpFloat(LerpFloat param)
        {
            during = param.during;
            useLerpCurve = param.useLerpCurve;
            lerpCurve = new AnimationCurve(param.lerpCurve.keys);
            startFloat = param.startFloat;
            endFloat = param.endFloat;
        }
    }

    [System.Serializable]
    public class LerpVector
    {
        public float during = 0;
        public bool useLerpCurve = false;
        public AnimationCurve lerpCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
        public Vector3 startVector = Vector3.zero;
        public Vector3 endVector = Vector3.zero;
    }


    public Vector3 GetVector()
    {
        float timePourcent = this.GetPourcentage();
        if (this.lerpCurve == null)
        {
            return Vector3.Lerp(this.startVector, this.endVector, timePourcent);
        }

        float valueCurve = this.lerpCurve.Evaluate(timePourcent);
        return this.startVector + (this.differenceVector * valueCurve);
    }

    public float GetFloat()
    {
        float timePourcent = this.GetPourcentage();
        if (this.lerpCurve == null)
        {
            return Mathf.Lerp(this.startFloat, this.endFloat, timePourcent);
        }
        float valueCurve = this.lerpCurve.Evaluate(timePourcent);
        return this.startFloat + (this.differenceFloat * valueCurve);
    }
    /// <summary>
    /// Same as GetFloat except it's like we're running the values backwards
    /// </summary>
    /// <returns></returns>
    public float GetFloatBackwards()
    {
        float timePourcent = 1- this.GetPourcentage();
        if (this.lerpCurve == null)
        {
            return Mathf.Lerp(this.startFloat, this.endFloat, timePourcent);
        }
        float valueCurve = this.lerpCurve.Evaluate(timePourcent);
        return this.startFloat + (this.differenceFloat * valueCurve);
    }


	public float GetCurveValue()
	{
		if (lerpCurve == null) return 0;
		return lerpCurve.Evaluate(GetPourcentage());
	}


    public float GetPourcentage()
    {
        if (during == 0) return 1;
        if (trueTime) return Mathf.Clamp01((Time.unscaledTime - this.startTime) / this.during);
        return Mathf.Clamp01((Time.time - this.startTime) / this.during);

    }

    public float GetRemainingTime()
    {
        if (trueTime) return this.during - (Time.unscaledTime - startTime);
        else return this.during - (Time.time- startTime);

    }

    public bool IsFinish()
    {
        return this.GetPourcentage() >= 1;
    }

    public bool IsWorking => GetPourcentage() < 1;
}