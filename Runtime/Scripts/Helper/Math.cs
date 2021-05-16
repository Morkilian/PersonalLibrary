namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Math 
    {
        public enum SphericalDirections { Outwards, Inwards }
        public enum OrthogonalDirections { Right, Left, Up, Down, Foward, Backward }
        public static Vector3 SpiralLerp(float value, int ToursAmount, Vector3 startingPoint, Vector3 endingPoint, SphericalDirections direction, Vector3 GlobalUpAxis)
        {
            value = Mathf.Clamp01(value);
            ToursAmount = Mathf.Max(1, Mathf.Abs(ToursAmount));
            GlobalUpAxis.Normalize();
            //General equation goes outwards, but if it's inwards, we inverse starting and ending point, and one-minus the value 
            if (direction == SphericalDirections.Inwards)
            {
                Vector3 temporalStart = endingPoint;
                endingPoint = startingPoint;
                startingPoint = temporalStart;
                value = 1 - value;
            }

            Vector3 VectorForward = endingPoint - startingPoint;
            VectorForward.y = 0;
            float distance = VectorForward.magnitude;
            VectorForward.Normalize();
            Vector3 VectorRight = Vector3.Cross(VectorForward, GlobalUpAxis);
            VectorRight.y = 0;
            VectorRight.Normalize();
            Vector3 newPos = startingPoint
                + (VectorRight * (value * Mathf.Sin(ToursAmount * value * 2f * Mathf.PI))
                + VectorForward * (value * Mathf.Cos(ToursAmount * value * 2f * Mathf.PI))
                ) * distance + (endingPoint.y - startingPoint.y) * value * GlobalUpAxis;
            return newPos;
        }




        public static float GetDistanceToPlane(Vector3 pointToGetDistance, Vector3 pointInPlane, Vector3 normalPlane)
        {

            float forwardPlane = -Vector3.Dot(pointToGetDistance, normalPlane);
            float dot = Vector3.Dot(normalPlane, pointInPlane);
            return forwardPlane + dot;
        }




        public static float PulsatingValue(int toursPerSecond, int frequency, float timeDelay = 0)
        {
            float time = Time.time + timeDelay;
            time *= toursPerSecond;
            float underRemainder = frequency * 2 * Mathf.PI;
            time %= underRemainder;
            float firstStep = time < 2 * Mathf.PI ? 1 : 0;
            time = Mathf.Abs(Mathf.Sin(time));
            return firstStep * time;
        }

        public static bool IsWithinAngle(Vector3 posToCheckFrom, Vector3 postToCheckTo, Vector3 forward, float eulerAngle)
        {
            Vector3 dirToPosToCheckTo = (postToCheckTo - posToCheckFrom).normalized;
            return (Mathf.Acos(Vector3.Dot(dirToPosToCheckTo, forward)) < eulerAngle * Mathf.Deg2Rad);
        }

        /// <summary>
        /// From an array of transforms, computes which ones are within the given range and returns and array with those.
        /// </summary>
        /// <param name="toCheck"> The array of transforms to check.</param>
        /// <param name="position">The starting position.</param>
        /// <param name="direction">The facing direction.</param>
        /// <param name="angle">The amplitude of the angle (total, not half).</param>
        /// <param name="maxDistance">The maximal distance to check.</param>
        /// <returns></returns>
        public static Transform[] WithinRange(Transform[] toCheck, Vector3 position, Vector3 direction, float angle, float maxDistance)
        {
            List<Transform> toReturn = new List<Transform>();
            for (int i = 0; i < toCheck.Length; i++)
            {
                Transform target = toCheck[i];
                if (target == null) continue;
                Vector3 dirToTarget = target.position - position;
                //Is it further than required
                float distance = (dirToTarget).sqrMagnitude;
                if (distance > maxDistance) continue;

                //Is it outside "view"
                dirToTarget = dirToTarget.normalized;
                float dot = Vector3.Dot(dirToTarget, direction);
                if (Mathf.Acos(dot) > angle * 0.5f) continue;

                //Else it's within range
                toReturn.Add(target);
            }
            return toReturn.ToArray();
        }

        private static Color[] DebugColors = new Color[4] { Color.yellow, Color.red, Color.green, Color.blue };
        /// <summary>
        /// From an array of transforms, computes which ones are within the given range and returns and array with those.
        /// </summary>
        /// <param name="toCheck"> The array of transforms to check.</param>
        /// <param name="position">The starting position.</param>
        /// <param name="direction">The facing direction.</param>
        /// <param name="angle">The amplitude of the angle (total, not half).</param>
        /// <param name="maxDistance">The maximal distance to check.</param>
        /// <param name="rayDebug">Wether to shoot a debug ray or not (with a duration of 2 seconds).</param>
        /// <returns></returns>
        public static Transform ClosestWithinRange(Collider[] toCheck, Vector3 position, Vector3 direction, float angle, float maxDistance, bool rayDebug = false)
        {
            Transform closest = null;
            float closestDistance = 0f;
#if UNITY_EDITOR
            if (rayDebug)
            {
                Debug.DrawLine(position, position + direction * maxDistance, Color.white, 2f);
                Debug.DrawLine(position, position + Quaternion.Euler(Vector3.up * angle * 0.5f) * direction * maxDistance, Color.white, 2f);
                Debug.DrawLine(position, position + Quaternion.Euler(-Vector3.up * angle * 0.5f) * direction * maxDistance, Color.white, 2f);
            } 
#endif
            for (int i = 0; i < toCheck.Length; i++)
            {
                if (toCheck[i] == null) continue;
                Color chosenColor = DebugColors[i % 4];
                Transform target = toCheck[i].transform;
                if (target == null) continue;
                Vector3 vectorToTarget = (target.position - position);
#if UNITY_EDITOR
                if (rayDebug) Debug.DrawLine(position, target.position, chosenColor, 2f); 
#endif

                //Is it outside "view"
                float angleWithTarget = Vector3.Angle(direction, vectorToTarget.normalized);
                if (angleWithTarget > angle * 0.5f) continue;

                //Is it further than required
                float distance = (vectorToTarget).magnitude;
                if (distance > maxDistance) continue;
                //Else it's within range
                if (closest == null)
                {
                    closest = target;
                    closestDistance = distance;
                }
                else if (distance < closestDistance)
                {
                    closest = target;
                    closestDistance = distance;
                }
            }
            return closest;
        }
    }

}