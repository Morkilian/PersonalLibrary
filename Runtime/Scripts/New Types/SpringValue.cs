using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian 
{
    /******************************************************************************
    Copyright (c) 2008-2012 Ryan Juckett
    http://www.ryanjuckett.com/
 
    This software is provided 'as-is', without any express or implied
    warranty. In no event will the authors be held liable for any damages
    arising from the use of this software.
 
    Permission is granted to anyone to use this software for any purpose,
    including commercial applications, and to alter it and redistribute it
    freely, subject to the following restrictions:
 
    1. The origin of this software must not be misrepresented; you must not
        claim that you wrote the original software. If you use this software
        in a product, an acknowledgment in the product documentation would be
        appreciated but is not required.
 
    2. Altered source versions must be plainly marked as such, and must not be
        misrepresented as being the original software.
 
    3. This notice may not be removed or altered from any source
        distribution.
******************************************************************************/
    /*KMS: Changed naming and structure of code to match C# and code convention somewhat*/
    /// <summary>
    /// Float that behaves like a spring when changing the value target.
    /// Method <see cref="UpdateCoefficients"/> must be called on Awake or Start once for it to work.
    /// </summary>
    [System.Serializable]
    public struct SpringValue
    {
        #region Instance
        [SerializeField][Min(0)] private float angularFrequency;
        [SerializeField][Min(0)] private float dampingRatio;

        private float currentValue;
        private float currentVelocity;
        private float valueTarget;
        [SerializeField] [HideInInspector] private DampedSpringMotionParams motionParams;
        public bool NeedsToUpdateValue => currentValue != valueTarget;
        public const float DELTA_TIME = 0.016f;
        #region Properties
        public float CurrentValue { get => currentValue; set { currentValue = value; } }
        public float ValueTarget { get => valueTarget; set { valueTarget = value; } }

        public float AngularFrequency
        {
            get => angularFrequency;
            set { angularFrequency = value; UpdateCoefficients(); }
        }
        public float DampingRatio
        {
            get => dampingRatio;
            set { dampingRatio = value; UpdateCoefficients(); }
        } 
        #endregion
        public SpringValue(float angularFrequency, float dampingRatio, float currentValue = 0)
        {
            this.angularFrequency = angularFrequency;
            this.dampingRatio = dampingRatio;
            this.currentValue = this.valueTarget = currentValue;
            motionParams = default;
            currentVelocity = 0;
            UpdateCoefficients();
        }
        /// <summary>
        /// Calculates the coefficients needed to update this spring value correctly.
        /// </summary>
        public void UpdateCoefficients()
        {
            CalcDampedSpringMotionParams(ref motionParams, DELTA_TIME, angularFrequency, dampingRatio);
        }
        /// <summary>
        /// Updates the current value spring.
        /// Must be called only once on update by the property holder.
        /// </summary>
        public void UpdateValue()
        {
            UpdateDampedSpringMotion(ref currentValue, ref currentVelocity, valueTarget, motionParams);
        }
        #endregion

        #region Base
        //******************************************************************************
        // Cached set of motion parameters that can be used to efficiently update
        // multiple springs using the same time step, angular frequency and damping
        // ratio.
        //******************************************************************************
        public struct DampedSpringMotionParams
        {
            // newPos = posPosCoef*oldPos + posVelCoef*oldVel
            public float m_posPosCoef, m_posVelCoef;
            // newVel = velPosCoef*oldPos + velVelCoef*oldVel
            public float m_velPosCoef, m_velVelCoef;
        };

        //******************************************************************************
        // This function will compute the parameters needed to simulate a damped spring
        // over a given period of time.
        // - An angular frequency is given to control how fast the spring oscillates.
        // - A damping ratio is given to control how fast the motion decays.
        //     damping ratio > 1: over damped
        //     damping ratio = 1: critically damped
        //     damping ratio < 1: under damped
        //******************************************************************************
        public static void CalcDampedSpringMotionParams(
            ref DampedSpringMotionParams springParams,       // motion parameters result
            float deltaTime,        // time step to advance
            float angularFrequency, // angular frequency of motion
            float dampingRatio)     // damping ratio of motion
        {
            const float epsilon = 0.0001f;

            // force values into legal range
            if (dampingRatio < 0.0f) dampingRatio = 0.0f;
            if (angularFrequency < 0.0f) angularFrequency = 0.0f;

            // if there is no angular frequency, the spring will not move and we can
            // return identity
            if (angularFrequency < epsilon)
            {
                springParams.m_posPosCoef = 1.0f;
                springParams.m_posVelCoef = 0.0f;
                springParams.m_velPosCoef = 0.0f;
                springParams.m_velVelCoef = 1.0f;
                return;
            }

            if (dampingRatio > 1.0f + epsilon)
            {
                // over-damped
                float za = -angularFrequency * dampingRatio;
                float zb = angularFrequency * Mathf.Sqrt(dampingRatio * dampingRatio - 1.0f);
                float z1 = za - zb;
                float z2 = za + zb;

                float e1 = Mathf.Exp(z1 * deltaTime);
                float e2 = Mathf.Exp(z2 * deltaTime);

                float invTwoZb = 1.0f / (2.0f * zb); // = 1 / (z2 - z1)

                float e1_Over_TwoZb = e1 * invTwoZb;
                float e2_Over_TwoZb = e2 * invTwoZb;

                float z1e1_Over_TwoZb = z1 * e1_Over_TwoZb;
                float z2e2_Over_TwoZb = z2 * e2_Over_TwoZb;

                springParams.m_posPosCoef = e1_Over_TwoZb * z2 - z2e2_Over_TwoZb + e2;
                springParams.m_posVelCoef = -e1_Over_TwoZb + e2_Over_TwoZb;

                springParams.m_velPosCoef = (z1e1_Over_TwoZb - z2e2_Over_TwoZb + e2) * z2;
                springParams.m_velVelCoef = -z1e1_Over_TwoZb + z2e2_Over_TwoZb;
            }
            else if (dampingRatio < 1.0f - epsilon)
            {
                // under-damped
                float omegaZeta = angularFrequency * dampingRatio;
                float alpha = angularFrequency * Mathf.Sqrt(1.0f - dampingRatio * dampingRatio);

                float expTerm = Mathf.Exp(-omegaZeta * deltaTime);
                float cosTerm = Mathf.Cos(alpha * deltaTime);
                float sinTerm = Mathf.Sin(alpha * deltaTime);

                float invAlpha = 1.0f / alpha;

                float expSin = expTerm * sinTerm;
                float expCos = expTerm * cosTerm;
                float expOmegaZetaSin_Over_Alpha = expTerm * omegaZeta * sinTerm * invAlpha;

                springParams.m_posPosCoef = expCos + expOmegaZetaSin_Over_Alpha;
                springParams.m_posVelCoef = expSin * invAlpha;

                springParams.m_velPosCoef = -expSin * alpha - omegaZeta * expOmegaZetaSin_Over_Alpha;
                springParams.m_velVelCoef = expCos - expOmegaZetaSin_Over_Alpha;
            }
            else
            {
                // critically damped
                float expTerm = Mathf.Exp(-angularFrequency * deltaTime);
                float timeExp = deltaTime * expTerm;
                float timeExpFreq = timeExp * angularFrequency;

                springParams.m_posPosCoef = timeExpFreq + expTerm;
                springParams.m_posVelCoef = timeExp;

                springParams.m_velPosCoef = -angularFrequency * timeExpFreq;
                springParams.m_velVelCoef = -timeExpFreq + expTerm;
            }
        }

        //******************************************************************************
        // This function will update the supplied position and velocity values over
        // according to the motion parameters.
        //******************************************************************************
        /// <summary>
        /// This function will update the supplied position and velocity values over according to the motion parameters.
        /// </summary>
        /// <param name="pPos">Position value to update.</param>
        /// <param name="pVel">Velocity value to update.</param>
        /// <param name="equilibriumPos"></param>
        /// <param name="param"></param>
        public static void UpdateDampedSpringMotion(
            ref float pPos,           // position value to update
            ref float pVel,           // velocity value to update
            float equilibriumPos, // position to approach
            DampedSpringMotionParams param)         // motion parameters to use
        {
            float oldPos = pPos - equilibriumPos; // update in equilibrium relative space
            float oldVel = pVel;

            pPos = oldPos * param.m_posPosCoef + oldVel * param.m_posVelCoef + equilibriumPos;
            pVel = oldPos * param.m_velPosCoef + oldVel * param.m_velVelCoef;

            if (Mathf.Abs(pPos - equilibriumPos) < 0.001f) //Snap to target if the difference is minimal
                pPos = equilibriumPos;
        }    
        #endregion
    }
}