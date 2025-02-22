using UnityEngine;

namespace Morkilian
{
    public static class LineRendererExtensions
    {
        public static void Clear(this LineRenderer lineRenderer)
        {
            lineRenderer.positionCount = 0;
        }
    }
}