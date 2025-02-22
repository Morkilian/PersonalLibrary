using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian
{
    public class AdvancedLerpFloatData : AdvancedLerp<float>.Data
    {
        public override float GetLerpedValue(float normalizedTime)
        {
            return Mathf.Lerp(StartValue, EndValue, normalizedTime);
        }
    }
}
