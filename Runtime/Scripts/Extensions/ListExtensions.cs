namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public static class ListExtensions 
    {
        public static void GetRandomElement<T,X>(this Dictionary<T,X> dict, out T key, out X value)
        {
            if (dict.Count == 0)
            {
                key = default(T);
                value = default(X);
                return;
            }
            int randomIndex = Random.Range(0, dict.Count);
            key = dict.ElementAt(randomIndex).Key;
            value = dict[key];
        }
        public static void GetRandomElementAndRemoveFromList<T, X>(this Dictionary<T, X> dict, out T key, out X value)
        {
            if (dict.Count == 0)
            {
                key = default(T);
                value = default(X);
                return;
            }
            int randomIndex = Random.Range(0, dict.Count);
            key = dict.ElementAt(randomIndex).Key;
            value = dict[key];
            dict.Remove(key);
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T GetRandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static T GetRandomElementAndRemoveFromList<T>(this List<T> list)
        {
            int index = Random.Range(0, list.Count);
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }

        public static T GetLastElement<T>(this IList<T> array)
        {
            if (array.Count > 0)
                return array[array.Count - 1];
            return default;
        }
        /// <summary>
        /// Get the closest component (transform position)
        /// </summary>        
        public static T GetClosestObject<T>(this IList<T> array, Vector3 refPoint) where T : Component
        {
            if (array == null || array.Count == 0) return null;
            else if (array.Count == 1) return array[0];
            else
            {
                T closest = array[0];
                float closestDistance = Vector3.SqrMagnitude(refPoint - array[0].transform.position);
                for (int i = 1; i < array.Count; i++)
                {
                    float newCloseDistance = Vector3.SqrMagnitude(refPoint - array[i].transform.position);
                    if (newCloseDistance < closestDistance)
                    {
                        closestDistance = newCloseDistance;
                        closest = array[1];
                    }
                }
                return closest;
            }            
        }

        /// <summary>
        /// Get the closest component (transform position), but does not take into account the Y position
        /// </summary>
        /// <param name="minDistance"> The minimal distance for the item to be returned. </param>
        public static T GetClosestGroundObject<T>(this IList<T> array, Vector3 refPoint, float minDistance) where T : Component
        {
            float closestDistance = minDistance*minDistance;
            Vector3 actualRefPoint = refPoint.FlatOneAxis(Axis.y);
            if (array == null || array.Count == 0) return null;
            else if (array.Count == 1) return array[0];
            else
            {
                T closest = array[0];
                for (int i = 0; i < array.Count; i++)
                {
                    float newCloseDistance = Vector3.SqrMagnitude(actualRefPoint - array[i].transform.position.FlatOneAxis(Axis.y));
                    if (newCloseDistance < closestDistance)
                    {
                        closestDistance = newCloseDistance;
                        closest = array[1];
                    }
                }
                return closest;
            }
        }

        /// <summary>
        /// Get the closest component (transform position), but does not take into account the Y position, regardless of distance
        /// </summary>
        public static T GetClosestGroundObject<T>(this IList<T> array, Vector3 refPoint) where T : Component
        {
            return array.GetClosestGroundObject(refPoint, Mathf.Infinity);            
        }

        /// <summary>
        /// Adds item to the list if the list doesn't contain the item already.
        /// </summary>        
        public static void AddIfNotIncluded<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == false)
                list.Add(item);
        }
        /// <summary>
        /// Remove the item from the list if the list contains the item already
        /// </summary>
        public static void RemoveIfIncluded<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
                list.Remove(item);
        }
    }

}