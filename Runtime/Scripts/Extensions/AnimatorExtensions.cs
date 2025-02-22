using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Morkilian
{
    public static class AnimatorExtensions
    {
        public static void AddAnimationEvent(this Animator animator, string clipName, AnimationEvent animationEvent, bool isTimeNormalized = false, bool checkAllClips = false)
        {
            float tempTime = animationEvent.time;
            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

                if (clip.name.Contains(clipName))
                {
                    if(isTimeNormalized == true)
                        animationEvent.time = tempTime*clip.length;
                    if (!ContainsEvent(clip, animationEvent))
                    {
                        clip.AddEvent(animationEvent);
                    }

                    if (!checkAllClips)
                        return;
                }
            }
            Debug.LogError($"[AddAnimationEvent] : The clip that contains {clipName} does not exist in the current animator");
        }
        public static void AddAnimationEvent(this Animator animator, float time, string functionName, string clipName, bool isTimeNormalized = false, bool checkAllClips = false)
        {
            AnimationEvent animationEvent = new AnimationEvent()
            {
                time = time,
                functionName = functionName
            };
            AddAnimationEvent(animator, clipName, animationEvent,isTimeNormalized, checkAllClips);
        }

        public static void AddAnimationEvent(this Animator animator, float time, string functionName, string clipName, int intParam, bool isTimeNormalized = false, bool checkAllClips = false)
        {
            AnimationEvent animationEvent = new AnimationEvent()
            {
                time = time,
                functionName = functionName,
                intParameter = intParam
            };
            AddAnimationEvent(animator, clipName, animationEvent,isTimeNormalized, checkAllClips);
        }

        public static void AddAnimationEvent(this Animator animator, float time, string functionName, string clipName, float floatParam, bool isTimeNormalized = false, bool checkAllClips = false)
        {
            AnimationEvent animationEvent = new AnimationEvent()
            {
                time = time,
                functionName = functionName,
                floatParameter = floatParam
            };
            AddAnimationEvent(animator, clipName, animationEvent,isTimeNormalized, checkAllClips);
        }

        public static void AddAnimationEvent(this Animator animator, float time, string functionName, string clipName, string stringParam, bool isTimeNormalized = false, bool checkAllClips = false)
        {
            AnimationEvent animationEvent = new AnimationEvent()
            {
                time = time,
                functionName = functionName,
                stringParameter = stringParam
            };
            AddAnimationEvent(animator, clipName, animationEvent,isTimeNormalized, checkAllClips);
        }

        public static void AddAnimationEvent(this Animator animator, float time, string functionName, string clipName, Object objectParam, bool isTimeNormalized = false, bool checkAllClips = false)
        {
            AnimationEvent animationEvent = new AnimationEvent()
            {
                time = time,
                functionName = functionName,
                objectReferenceParameter = objectParam
            };
            AddAnimationEvent(animator, clipName, animationEvent,isTimeNormalized, checkAllClips);
        }

        private static bool ContainsEvent(AnimationClip animationClip, AnimationEvent animationEvent)
        {
            foreach (AnimationEvent animEvent in animationClip.events)
            {
                if (animEvent.time == animationEvent.time && animEvent.functionName.Equals(animationEvent.functionName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Reset all trigger parameters on this Animator
        /// </summary>
        public static void ResetAllTriggers(this Animator animator)
        {
            foreach (var trigger in animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(trigger.name);
                }
            }
        }

        /// <summary>
        /// Reset all bool parameters on this Animator to false
        /// </summary>
        public static void ResetAllBool(this Animator animator)
        {
            foreach (var boolean in animator.parameters)
            {
                if (boolean.type == AnimatorControllerParameterType.Bool)
                {
                    animator.SetBool(boolean.name, false);
                }
            }
        }

        /// <summary>
        /// Reset all int parameters on this Animator to 0
        /// </summary>
        public static void ResetAllInt(this Animator animator)
        {
            foreach (var integer in animator.parameters)
            {
                if (integer.type == AnimatorControllerParameterType.Int)
                {
                    animator.SetInteger(integer.name, 0);
                }
            }
        }

        /// <summary>
        /// Reset all float parameters on this Animator to 0.0f
        /// </summary>
        public static void ResetAllFloat(this Animator animator)
        {
            foreach (var f in animator.parameters)
            {
                if (f.type == AnimatorControllerParameterType.Float)
                {
                    animator.SetFloat(f.name, 0.0f);
                }
            }
        }

        /// <summary>
        /// Reset all parameters on this Animator to their default value (trigger to default, float to 0.0f, int to 0, bool to false)
        /// </summary>
        public static void ResetAllParameters(this  Animator animator)
        {
            ResetAllTriggers(animator);
            ResetAllFloat(animator);
            ResetAllInt(animator);
            ResetAllBool(animator);
        }
    }
}