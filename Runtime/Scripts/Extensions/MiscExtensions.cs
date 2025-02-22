using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Morkilian
{
    public static class MiscExtensions 
    {
        /// <summary>
        /// Shortcut to play or stop a particle system.
        /// </summary>
        /// <param name="ps">This particle system.</param>
        /// <param name="value">Whether to play or to stop the particle system.</param>
        /// <param name="clearIfStopping">If stopping, whether to clear the current particles or not.</param>
        public static void SetPlaying(this ParticleSystem ps, bool value, bool clearIfStopping = false)
        {
            if (ps == null) return;
            if (value && ps.isPlaying == false)
                ps.Play();
            else if(value == false && ps.isPlaying == true)
                ps.Stop(true, clearIfStopping ? ParticleSystemStopBehavior.StopEmittingAndClear : ParticleSystemStopBehavior.StopEmitting);
        }

#if VISUAL_EFFECT_GRAPH
        /// <summary>
        /// Shortcut to play or stop a visual effect.
        /// </summary>
        /// <param name="ps">This visual effect.</param>
        /// <param name="value">Whether to play or to stop the visual effect.</param>
        /// <param name="clearIfStopping">If stopping, whether to clear the current particles or not.</param>
        public static void SetPlaying(this VisualEffect ve, bool value, bool clearIfStopping = false)
        {
            if (value && ve.HasAnySystemAwake() == false)
                ve.Play();
            else if (value == false && ve.HasAnySystemAwake())
            {
                if (clearIfStopping)
                    ve.Reinit();
                ve.Stop();
            }
        } 
#endif

        public static string Log(this System.Exception e)
        {
            return $"{e.Message}+\n\n+{e.InnerException}+{e.ToString()}";
        }
    }
}