using UnityEngine;

namespace Morkilian
{
    public static class CameraExtensions
    {
        public static bool IsVisibleInCameraView(this Camera camera, Transform objectTransform) 
        {
            return IsVisibleInCameraView(camera, objectTransform.position);
        }

        public static bool IsVisibleInCameraView(this Camera camera, Vector3 objectPosition)
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(objectPosition);

            bool isVisible = viewportPoint.x > 0.0 && viewportPoint.x < 1.0f &&
                             viewportPoint.y > 0.0 && viewportPoint.y < 1.0f &&
                             viewportPoint.z > 0.0;

            return isVisible;
        }
    }
}
