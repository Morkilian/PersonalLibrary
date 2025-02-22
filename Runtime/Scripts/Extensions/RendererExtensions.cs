namespace Morkilian
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class RendererExtensions
    {
        /// <summary>
        /// Calculates the total length of the line renderer using its positions.
        /// </summary>        
        public static float GetLength(this LineRenderer lineRenderer)
        {            
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            int amount = lineRenderer.GetPositions(positions) - 1;
            float distance = 0;
            for (int i = 0; i < amount; i++)
            {
                distance += Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
            }
            return distance;
        }

        /// <summary>
        /// Calculates the total length of the trail renderer using its positions.
        /// </summary>        
        public static float GetLength(this TrailRenderer lineRenderer)
        {
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            int amount = lineRenderer.GetPositions(positions) - 1;
            float distance = 0;
            for (int i = 0; i < amount; i++)
            {
                distance += Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
            }
            return distance;
        }

        public static Vector3 GetWorldPosition(this LineRenderer lineRenderer, int index)
        {
            Debug.Assert(index < lineRenderer.positionCount, $"Index {index} is outside of range ({lineRenderer.positionCount})!");
            Vector3 pos = lineRenderer.GetPosition(index);
            if (lineRenderer.useWorldSpace)
            {
                return pos;
            }
            else
                return lineRenderer.transform.TransformPoint(pos);
        }

        /// <summary>
        /// Returns a random world position inside the bounds.
        /// </summary>
        /// <param name="bounds"></param>        
        /// <returns></returns>
        public static Vector3 GetRandomPositionInside(this Bounds bounds)
        {
            Vector3 toReturn = bounds.center;
            toReturn.x = Mathf.Lerp(bounds.min.x, bounds.max.x, Random.value);
            toReturn.y = Mathf.Lerp(bounds.min.y, bounds.max.y, Random.value);
            toReturn.z = Mathf.Lerp(bounds.min.z, bounds.max.z, Random.value);

            return toReturn;
        }

        /// <summary>
        /// Check if a renderer is visible from a camera
        /// </summary>
        /// <param name="renderer">The renderer to check visibility</param>
        /// <param name="camera">The camera to check the renderer</param>
        /// <returns>Is the renderer visible from the camera</returns>
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        public static bool InstantiateSharedMaterial(this Renderer renderer, out Material newMaterial)
        {
            newMaterial = null;
            if(renderer!= null && renderer.sharedMaterial!= null)
            {
                newMaterial = new Material(renderer.sharedMaterial);
                renderer.sharedMaterial = newMaterial;
                return true;
            }
            return false;
        }
    } 
}
