using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Morkilian
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Similar to an Invoke, executes the given action after the given delay.
        /// </summary>
        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Action action, float delay)
        {
            return behaviour.StartCoroutine(InvokeDelayed(delay, action));
        }
        /// <summary>
        /// Similar to an Invoke, executes the given unity event after the given delay.
        /// </summary>
        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, UnityEvent uEvent, float delay)
        {
            return behaviour.StartCoroutine(InvokeDelayed(delay, uEvent));
        }
        /// <summary>
        /// Similar to an Invoke, executes the given action with parameter after the given delay.
        /// </summary>
        public static Coroutine StartCoroutine<T>(this MonoBehaviour behaviour, System.Action<T> action, T input, float delay)
        {
            return behaviour.StartCoroutine(InvokeDelayed(delay, action, input));
        }

        /// <summary>
        /// Similar to an Invoke, executes the given action after the given delay.
        /// This overload saves memory heap by giving a prepared WaitForSeconds.
        /// </summary>
        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Action action, WaitForSeconds waitForSeconds)
        {
            return behaviour.StartCoroutine(InvokeDelayed(waitForSeconds, action));
        }
        /// <summary>
        /// Similar to an Invoke, executes the given action with parameter after the given delay.
        /// This overload saves memory heap by giving a prepared WaitForSeconds.
        /// </summary>
        public static Coroutine StartCoroutine<T>(this MonoBehaviour behaviour, System.Action<T> action, T input, WaitForSeconds waitForSeconds)
        {
            return behaviour.StartCoroutine(InvokeDelayed(waitForSeconds, action, input));
        }

        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Action action, WaitUntil waitUntil)
        {
            return behaviour.StartCoroutine(InvokeDelayed(waitUntil, action));
        }

        private static IEnumerator InvokeDelayed(float time, System.Action action)
        {
            if (time > 0)
            {
                yield return new WaitForSeconds(time);
                action();
            }
            else
                Debug.LogWarning("Can't start a delayed coroutine with a negative or 0 time!");
        }

        private static IEnumerator InvokeDelayed(float time, UnityEvent uEvent)
        {
            if (time > 0)
            {
                yield return new WaitForSeconds(time);
                uEvent?.Invoke();
            }
            else
                Debug.LogWarning("Can't start a delayed coroutine with a negative or 0 time!");
        }

        private static IEnumerator InvokeDelayed<T>(float time, System.Action<T> action, T input)
        {
            yield return new WaitForSeconds(time);
            action(input);
        }

        private static IEnumerator InvokeDelayed(WaitForSeconds waitForSeconds, System.Action action)
        {
            yield return waitForSeconds;
            action();
        }

        private static IEnumerator InvokeDelayed<T>(WaitForSeconds waitForSeconds, System.Action<T> action, T input)
        {
            yield return waitForSeconds;
            action(input);
        }

        private static IEnumerator InvokeDelayed(WaitUntil waitUntil, System.Action action)
        {
            yield return waitUntil;
            action();
        }

        /// <summary>
        /// Launches a coroutine while also stopping the potential coroutine already going on.
        /// Note: you still need to set the coroutine to null at the end of the coroutine.
        /// </summary>
        /// <returns>Whether a coroutine has been stopped or not.</returns>
        public static bool RestartCoroutine(this MonoBehaviour behaviour, ref Coroutine coroutineVar, IEnumerator coroutineMethod)
        {
            bool thereWas = coroutineVar != null;
            if (thereWas)
                behaviour.StopCoroutine(coroutineVar);
            coroutineVar = behaviour.StartCoroutine(coroutineMethod);
            return thereWas;
        }

        /// <summary>
        /// Launches a coroutine while also stopping the potential coroutine already going on.
        /// Note: you still need to set the coroutine to null at the end of the coroutine.
        /// </summary>
        /// <returns>Whether a coroutine has been stopped or not.</returns>
        public static bool RestartCoroutine(this MonoBehaviour behaviour, ref Coroutine coroutineVar, System.Action action, float delay)
        {
            bool thereWas = coroutineVar != null;
            if (thereWas)
                behaviour.StopCoroutine(coroutineVar);
            coroutineVar = behaviour.StartCoroutine(InvokeDelayed(delay, action));
            return thereWas;
        }

        /// <summary>
        /// Launches a coroutine with the given AdvancedLerp.LerpFloat <see cref="AdvancedLerp.LerpFloat"/> for how ot use it.
        /// It is supported with different actions thorough the lerping.
        /// </summary>
        public static Coroutine StartCoroutineLerpFloat(this MonoBehaviour behaviour,
            AdvancedLerpFloatData structToUse, 
            System.Action<float> onNormalizedValueAction = null, 
            System.Action<float> onValueAction = null, 
            System.Action<float> onReversedValueAction = null,
            System.Action onFinish = null)
        {
            return behaviour.StartCoroutine(CoroutineLerp(structToUse,onNormalizedValueAction,onValueAction, onReversedValueAction,onFinish));
        }

        private static IEnumerator CoroutineLerp(AdvancedLerpFloatData structToUse, 
            System.Action<float> onNormalizedValueAction = null, 
            System.Action<float> onValueAction = null, 
            System.Action<float> onReversedValueAction = null, 
            System.Action onFinish = null)
        {
            AdvancedLerp<float> lerp = new AdvancedLerp<float>(structToUse);
            while (lerp.IsWorking)
            {
                yield return null;
                onNormalizedValueAction?.Invoke(lerp.CurrentProgress);
                onValueAction?.Invoke(lerp.CurrentValue);
                onReversedValueAction?.Invoke(lerp.CurrentReversedFloatValue);
            }
            onFinish?.Invoke();
        }
    }
}