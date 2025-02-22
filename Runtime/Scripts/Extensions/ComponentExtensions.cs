using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Morkilian
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Tries to get a component in the same object or its consecutive parent.
        /// </summary>        
        /// <param name="component">The extracted component</param>
        /// <returns>Wether a component could be extracted or not.</returns>
        public static bool TryGetComponentInParent<T>(this Component t, out T component) where T : Component
        {
            bool found = t.TryGetComponent<T>(out component);
            if(found == true)
                return true;
            else if(t.transform.parent != null)
                    return TryGetComponentInParent<T>(t.transform.parent, out component);
            return false;
        }

        public static bool TryGetComponentInChildren<T>(this Component t, out T component) where T : Component
        {
            bool found = t.TryGetComponent<T>(out component);
            if (found)
                return true;
            else
            {
                foreach (Transform child in t.transform)
                {
                    if (child.TryGetComponent<T>(out component))
                    {
                        return true;
                    }
                    else
                    {
                        if (TryGetComponentInChildren<T>(child, out component))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public static bool CompareTag(this Component component, string[] tags)
        {
            if (component == null || tags == null)
                return false;

            string componentTag = component.tag;

            for (int i = 0; i < tags.Length; i++)
            {
                if (componentTag == tags[i])
                    return true;
            }

            return false;
        }
    }
}