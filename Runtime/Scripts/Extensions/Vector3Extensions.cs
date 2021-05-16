namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum Axis { x, y, z }
    public static class Vector3Extensions
    {
        /// <summary>
        /// Returns a vector with one of the axis flattened to 0
        /// </summary>
        /// <param name="axis">The axis to flat.</param>
        /// <param name="normalize">Wether the vector should be normalized or not.</param>
        /// <returns> The vector with one of the axis flattened to 0.</returns>
        public static Vector3 FlatOneAxis(this Vector3 v, Axis axis, bool normalize = false)
        {
            Vector3 toReturn = new Vector3(axis == Axis.x ? 0 : v.x, axis == Axis.y ? 0 : v.y, axis == Axis.z ? 0 : v.z);
            return normalize ? toReturn.normalized : toReturn;
        }

        public static float AngleBetweenTwoMorePoints(this Vector3 v, Vector3 pointA, Vector3 pointB, bool toRadians = false)
        {
            Vector3 dirA = (pointA - v).normalized;
            Vector3 dirB = (pointB - v).normalized;
            return Vector3.Angle(dirA, dirB) * (toRadians ? Mathf.Deg2Rad : 1);
        }

        /// <summary>
        /// Rotate this vector by a given angle around a given axis.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="angle">Amount of angle to rotate, in EULER.</param>
        /// <param name="axis">The axis to rotate around.</param>
        /// <returns></returns>
        public static Vector3 Rotate(this Vector3 v, float angle, Vector3 axis)
        {
            return Quaternion.Euler(axis * angle) * v;
        }
    }

}