using UnityEngine;

namespace Morkilian
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Reset the world position of the transform to (0,0,0)
        /// </summary>
        public static void ResetPosition(this Transform t)
        {
            t.position = Vector3.zero;
        }

        /// <summary>
        /// Reset the local position of the transform to (0,0,0)
        /// </summary>
        public static void ResetLocalPosition(this Transform t)
        {
            t.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Reset the world rotation of the transform to (0,0,0)
        /// </summary>
        public static void ResetRotation (this Transform t)
        {
            t.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Reset the local rotation of the transform to (0,0,0)
        /// </summary>
        public static void ResetLocalRotation(this Transform t)
        {
            t.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Reset the local scale of the transform to (1,1,1)
        /// </summary>
        public static void ResetLocalScale(this Transform t)
        {
            t.localScale = Vector3.one;
        }

        public static void SetLocalPositionAndRotation(this Transform t, Vector3 localPosition, Quaternion localRotation)
        {
            t.localPosition = localPosition;
            t.localRotation = localRotation;
        }

        /// <summary>
        /// Reset the world position and rotation of the transform to (0,0,0)
        /// </summary>
        public static void ResetPositionAndRotation(this Transform t)
        {
            t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Reset the local position and rotation of the transform to (0,0,0)
        /// </summary>
        public static void ResetLocalPositionAndRotation(this Transform t)
        {
            t.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Reset the world position and rotation of the transform to (0,0,0) and the local scale to (1,1,1)
        /// </summary>
        public static void ResetAll(this Transform t)
        {
            t.ResetPositionAndRotation();
            t.ResetLocalScale();
        }

        /// <summary>
        /// Reset the local position and rotation of the transform to (0,0,0) and the local scale to (1,1,1)
        /// </summary>
        public static void ResetAllLocal(this Transform t)
        {
            t.ResetLocalPositionAndRotation();
            t.ResetLocalScale();
        }

        /// <summary>
        /// Change Layer and Child Layer
        /// </summary>
        public static void ChangeLayersRecursively(this Transform trans, string name)
        {
            trans.gameObject.layer = LayerMask.NameToLayer(name);
            foreach (Transform child in trans)
            {
                child.ChangeLayersRecursively(name);
            }
        }

        /// <summary>
        /// Get a random child from this Transform
        /// </summary>
        /// <param name="t"></param>
        /// <returns>A random child or null if childcount is equal to 0</returns>
        public static Transform GetRandomChild(this Transform t)
        {
            int childCount = t.childCount;
            if (childCount == 0)
            {
                return null;
            }
            else
            {
                int rnd = Random.Range(0, childCount);
                return t.GetChild(rnd);
            }
        }
        /// <summary>
        /// Recursively searches for the first child with the given name.
        /// </summary>
        public static Transform FindChildDeep(this Transform transform, string name)
        {
            foreach (Transform child in transform)
            {
                if (child.name == name)
                    return child;
                else
                {
                    Transform found = FindChildDeep(child, name); //Attempt to find subsequent
                    if (found != null)
                        return found;
                }
            }
            return null;
        }
    }
}