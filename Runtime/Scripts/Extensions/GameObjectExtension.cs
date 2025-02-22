using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian
{
    public static class GameObjectExtension
    {
        public static void SetActiveOptimized(this GameObject go, bool enable)
        {
            if (go && go.activeSelf != enable)
            {
                go.SetActive(enable);
            }
        }
        
        /// <param name="layer">Layer is NOT a bitmask, needs to be between [0,31]</param>
        public static void SetLayerRecursively(this GameObject go, int layer)
        {
            go.layer = layer;

            foreach (Transform child in go.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

        public static void SetTagRecursively(this GameObject go, string tag)
        {
            go.tag = tag;

            foreach (Transform child in go.transform)
            {
                child.gameObject.SetTagRecursively(tag);
            }
        }

        public static GameObject FindDeepChild(this GameObject fromGameObject, string withName)
        {
            Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
            return null;
        }
    }
}