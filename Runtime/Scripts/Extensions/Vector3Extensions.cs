namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Flags]
    public enum Axis { x=1, y=2, z=4 }
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

        /// <summary>
        /// Multiply vectorA by vectorB and store it in vectorA.
        /// </summary>
        /// <param name="vectorA">The vector that will be multiplied by vectorB. (Also store the result.°</param>
        /// <param name="vectorB">The vector that will multiply the vectorA </param>
        /// <returns></returns>
        public static Vector3 Mul(this Vector3 vectorA, Vector3 vectorB)
        {
            float x = vectorA.x * vectorB.x;
            float y = vectorA.y * vectorB.y;
            float z = vectorA.z * vectorB.z;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Rotates this vector around another given point at given rotation
        /// </summary>        
        public static Vector3 RotateAroundPivot(this Vector3 vector, Quaternion rotation, Vector3 pivot = default(Vector3))
        {
            return rotation * (vector - pivot) + pivot;
        }

        /// <summary>
        /// Rotates this vector around another given point at given rotation
        /// </summary>
        public static Vector3 RotateAroundPivot(this Vector3 vector, Vector3 rotation, Vector3 pivot = default(Vector3))
        {
            return RotateAroundPivot(vector, Quaternion.Euler(rotation), pivot);
        }

        /// <summary>
        /// Rotates this vector around another given point at given rotation
        /// </summary>
        public static Vector3 RotateAroundPivot(this Vector3 vector, float x, float y, float z, Vector3 pivot = default(Vector3))
        {
            return RotateAroundPivot(vector, Quaternion.Euler(x, y, z), pivot);
        }

        public static float InverseLerp(Vector3 from, Vector3 to, Vector3 current)
        {
            Vector3 AB = to - from;
            Vector3 AV = current - from;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
    }

}