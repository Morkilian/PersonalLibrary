namespace Morkilian.Helper
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
    } 
}
