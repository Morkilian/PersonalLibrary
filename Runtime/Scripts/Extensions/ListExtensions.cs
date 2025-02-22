namespace Morkilian
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine; 
    using System.Linq;

    public static class ListExtensions 
    {
        #region Dictionary
        public static void GetRandomElement<T, X>(this Dictionary<T, X> dict, out T key, out X value)
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
        #endregion

        #region Arrays and Lists
        /// <summary>
        /// Iterates through the array of the given type with an action on every non-null member
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="array"></param>
        /// <param name="action">The action to perform </param>
        public static void Iterate<T>(this IReadOnlyList<T> array, System.Action<T> action) //where T : notnull
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] != null) 
                { 
                    action(array[i]); 
                }
            }
        }
        /// <summary>
        /// Returns a random item from the list/array. It may be null.
        /// </summary>
        public static T GetRandomElement<T>(this IReadOnlyList<T> array) 
        {
            if (array == null || array.Count == 0)
            {
                Debug.LogWarning("Cannot get a random element as the array is null or empty! ");
                return default;
            }
            return array[Random.Range(0, array.Count)];
        }

        /// <summary>
        /// Returns a random item from the list/array, and also its index in said collection
        /// </summary>
        public static T GetRandomElement<T>(this IReadOnlyList<T> array, out int index)
        {
            index = -1;
            if (array == null || array.Count == 0)
            {
                Debug.LogWarning("Cannot get a random element as the array is null or empty! ");
                return default;
            }
            index = Random.Range(0, array.Count);
            return array[index];
        }

        public static T GetLastElement<T>(this IReadOnlyList<T> array)
        {
            if (array == null || array.Count == 0)
            {
                Debug.LogWarning("Cannot get the last element as the array is null or empty! ");
                return default;
            }
            else
                return array[array.Count - 1];
        }

        /// <summary>
        /// Returns a random item from the list/array, with exclusion rules
        /// </summary>
        /// 
        public static T GetRandomElementWithExclusion<T>(this IReadOnlyList<T> array,int indexToExclude)
        {
            int index = -1;
            if (array == null || array.Count == 0)
            {
                Debug.LogWarning("Cannot get a random element as the array is null or empty! ");
                return default;
            }

            do
            {
                index = Random.Range(0, array.Count);
            } while (index == indexToExclude);

            return array[index];
        }

        /// <summary>
        /// Get the closest component (transform position).
        /// If the given array is not empty, an item will be returned no matter what.
        /// </summary>        
        public static T GetClosestObject<T>(this IReadOnlyList<T> array, Vector3 refPoint) where T : Component
        {
            if (array == null || array.Count == 0) return null;
            else if (array.Count == 1) return array[0];
            else
            {
                //Find the first usable element
                int startingIndex = -1;
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i] != null)
                        startingIndex = i;
                }
                if (startingIndex == -1)
                    return null;

                T closest = array[startingIndex];
                float closestDistance = Vector3.SqrMagnitude(refPoint - array[startingIndex].transform.position);
                for (int i = startingIndex + 1; i < array.Count; i++)
                {
                    if (array[i] != null)
                    {
                        float newCloseDistance = Vector3.SqrMagnitude(refPoint - array[i].transform.position);
                        if (newCloseDistance < closestDistance)
                        {
                            closestDistance = newCloseDistance;
                            closest = array[i];
                        }
                    }
                }
                return closest;
            }
        }

        /// <summary>
        /// Get the closest component (transform position), but does not take into account the Y position.
        /// Returns null if there's no item within the minimal distance.
        /// </summary>
        /// <param name="minDistance"> The minimal distance for the item to be returned. </param>
        /// <param name="furtherFilterObjects">Func to further filter objects. With this, you can skip items in the given array. If the func returns true, the item will be skipped. If no func is added, no filtering will be done.</param>
        public static T GetClosestGroundObject<T>(this IReadOnlyList<T> array, Vector3 refPoint, float minDistance, System.Func<T, bool> furtherFilterObjects = null) where T : Component
        {
            float closestDistance = minDistance * minDistance;
            Vector3 actualRefPoint = refPoint.FlatOneAxis(Axis.y);
            if (array == null || array.Count == 0) 
                return null;
            else if (array.Count == 1)
            {
                if (array[0] != null && Vector3.SqrMagnitude(array[0].transform.position.FlatOneAxis(Axis.y) - actualRefPoint) < closestDistance)
                {
                    if(furtherFilterObjects!=null && furtherFilterObjects(array[0]) == false)
                        return array[0];
                }
                return null;
            }
            else
            {
                //Find the first usable element
                int startingIndex = -1;
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i] != null)
                    {
                        startingIndex = i;
                        break;
                    }
                }
                if (startingIndex == -1)
                    return null;

                T closest = null;
                for (int i = startingIndex; i < array.Count; i++)
                {
                    if (array[i] != null)
                    {
                        if (furtherFilterObjects != null && furtherFilterObjects(array[i]) == true)
                            continue;
                        float newCloseDistance = Vector3.SqrMagnitude(actualRefPoint - array[i].transform.position.FlatOneAxis(Axis.y));
                        if (newCloseDistance < closestDistance)
                        {
                            closestDistance = newCloseDistance;
                            closest = array[i];
                        }
                    }
                }
                return closest;
            }
        }

        /// <summary>
        /// Get the closest component (transform position), but does not take into account the Y position, regardless of distance
        /// </summary>
        public static T GetClosestGroundObject<T>(this IReadOnlyList<T> array, Vector3 refPoint) where T : Component
        {
            return array.GetClosestGroundObject(refPoint, Mathf.Infinity);
        }
        /// <summary>
        /// Selects the closest component in the list within the given radius and angle. If there's none, returns null.
        /// </summary>
        public static T GetClosestGroundObjectWithinAngle<T>(this IReadOnlyList<T> array, Vector3 refPoint, float minDistance, Vector3 refForward, float halfAngleDegrees, params T[] toAvoid) where T : Component
        {
            if(refForward == Vector3.zero)
            {
                Debug.Log("refForward can't be Vector3.zero!");
                return null;
            }
            bool IsWithinParameters(Vector3 point, float closestDistance)
            {
                Vector3 dir = (point - refPoint).FlatOneAxis(Axis.y);
                float sqr = dir.sqrMagnitude;
                if(sqr < closestDistance)
                {
                    float dot = Vector3.Dot(dir.normalized, refForward);
                    return Mathf.Acos(dot) * Mathf.Rad2Deg < halfAngleDegrees;
                }
                return false;
            }
            

            Vector3 actualRefPoint = refPoint.FlatOneAxis(Axis.y);
            if (array == null || array.Count == 0)
                return null;
            else if (array.Count == 1)
                return array[0];
            else
            {
                //Find the first usable element
                int startingIndex = -1;
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i] != null)
                    {
                        startingIndex = i;
                        break;
                    }
                }
                if (startingIndex == -1)
                    return null;
                float closestDistance = minDistance * minDistance;
                T closest = null;
                if (toAvoid.Contains(array[startingIndex]) == false && IsWithinParameters(array[startingIndex].transform.position, closestDistance))
                 closest = array[startingIndex];
                for (int i = startingIndex; i < array.Count; i++)
                {
                    if (array[i] != null)
                    {
                        if (toAvoid.Contains(array[i]) == false && IsWithinParameters(array[i].transform.position, closestDistance))
                        {
                            float newCloseDistance = Vector3.SqrMagnitude(actualRefPoint - array[i].transform.position.FlatOneAxis(Axis.y));
                            closestDistance = newCloseDistance;
                            closest = array[i];
                        }
                    }
                }
                return closest;
            }
        }
        /// <summary>
        /// Invokes the action for every item in the ienumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, System.Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
        #endregion

        #region Arrays

        public static void Clear<T>(this T[] array)
        {
            System.Array.Clear(array, 0, array.Length);
        }

        #endregion

        #region Lists
        public static T GetRandomElementAndRemoveFromList<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                int index = Random.Range(0, list.Count);
                T element = list[index];
                list.RemoveAt(index);
                return element;
            }
            else
            {
                Debug.LogError("[GetRandomElementAndRemoveFromList] List was empty! Returning default.");
                return default;
            }
        }

        /// <summary>
        /// Adds item to the list if the list doesn't contain the item already.
        /// </summary>        
        public static bool AddIfNotIncluded<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == false)
            {
                list.Add(item);
                return true;
            }
            return false;

        }
        /// <summary>
        /// Remove the item from the list if the list contains the item already
        /// </summary>
        public static bool RemoveIfIncluded<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                list.Remove(item);
                return true;
            }
            return false;

        }

        /// <summary>
        /// Removes the last item of the list if it has one
        /// </summary>                
        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Count > 0)
                list.RemoveAt(list.Count - 1);
        }
        /// <summary>
        /// Removes the item if the functions returns true
        /// </summary>
        public static void RemoveIf<T>(this List<T> list, System.Func<T, bool> func, bool skipAtFirst = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (func(list[i]))
                {
                    list.RemoveAt(i);
                    if (skipAtFirst == true)
                        return;
                    else
                        i--; //Since we've removed an entry, go back a step to prevent skipping the next index // ex: 13 -> (14) -> 15
                }

            }
        }

        /// <summary>
        /// Randomize the content of the list
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
        #endregion
    }

}