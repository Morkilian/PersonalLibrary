namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Data;

    public static class Collisions 
    {

        private static int groundLayerMask = 0;
        /// <summary>
        /// Returns the layer mask of the layer "Ground"
        /// </summary>
        public static int GroundLayerMask { get { if (groundLayerMask == 0) groundLayerMask = 1 << LayerMask.NameToLayer(Layers.GROUND); return groundLayerMask; } }
        private static int enemiesLayerMask = 0;
        public static int EnemiesLayerMask { get { if (enemiesLayerMask == 0) enemiesLayerMask = 1 << LayerMask.NameToLayer(Layers.ENEMY); return enemiesLayerMask; } }

        private static int blockersLayerMask = 0;
        public static int BlockersLayerMask { get { if (blockersLayerMask == 0) blockersLayerMask = 1 << LayerMask.NameToLayer(Layers.BLOCKER); return blockersLayerMask; } }

        private static int playersLayerMask = 0;
        public static int PlayerLayerMask { get { if (playersLayerMask == 0) playersLayerMask = 1 << LayerMask.NameToLayer(Layers.PLAYER); return playersLayerMask; } }


        private static int mBlockersAndGroundLayerMask = 0;
        public static int BlockersAndGroundLayerMask { get { if (mBlockersAndGroundLayerMask == 0) mBlockersAndGroundLayerMask = BlockersLayerMask | GroundLayerMask; return mBlockersAndGroundLayerMask; } }

        /// <summary>
        /// Tries to get the furthest ground point in a given direction within the distance allowed. 
        /// </summary>
        /// <param name="origin">The point in space from which we want to check if there's ground in direction </param>
        /// <param name="directionTest"> The direction, paralel to the ground</param>
        /// <param name="maxDistance">The maximum distance to check towards direction</param>
        /// <param name="steps">The number of steps to check</param>
        /// <returns> The furthest point. If none could be found, origin is returned.</returns>
        public static Vector3 TestGroundPosition(Vector3 origin, Vector3 directionTest, float maxDistance, int steps, float maxHeightTestDistance = 0.2f, int layerMask = 0)
        {
            steps = Mathf.Max(Mathf.Abs(steps), 1);
            maxDistance = Mathf.Abs(maxDistance);
            float singleStep = maxDistance / (float)steps;
            RaycastHit hitInfo;
            int layer = layerMask == 0 ? GroundLayerMask : layerMask;
            for (int i = 0; i < steps; i++)
            {
                if (Physics.Raycast(origin + directionTest * (steps - i) * singleStep, Vector3.down, out hitInfo, maxHeightTestDistance, layer)
                    || Physics.Raycast(origin + directionTest * (steps - i) * singleStep, Vector3.up, out hitInfo, maxHeightTestDistance, layer))
                {
                    Debug.LogFormat("[MathHelper] Starting point:{0}, target direction{1}, target distance{2}, target found {3}, Red line: original point, blue line: target pos, white line: pos found", origin, directionTest, maxDistance, hitInfo.point);
                    Debug.DrawLine(origin, origin + Vector3.up * 3f, Color.red, 5f);
                    Debug.DrawLine(origin + directionTest * (maxDistance + 0.1f), origin + directionTest * maxDistance + Vector3.up * 3f, Color.blue, 5f);
                    Debug.DrawLine(hitInfo.point + directionTest * (0.15f), hitInfo.point + directionTest * (0.15f) + Vector3.up * 3f, Color.white, 5f);
                    return hitInfo.point;
                }
            }
            Debug.LogFormat("[MathHelper] Bad raycasts! Starting point:{0}, target direction{1}, target distance{2},  Red line: original point, blue line: target pos", origin, directionTest, maxDistance);
            return origin;
        }

        /// <summary>
        /// Finds and returns the vertically ground point at a given position.
        /// </summary>
        /// <param name="origin">Where to look the ground position</param>
        /// <param name="offset">Vertical offset to add from the ground point.</param>
        /// <param name="layerMask">Optional layer mask. If not feeded, the GroundLayerMask will be used.</param>
        /// <returns></returns>
        public static Vector3 PlaceGroundHeight(Vector3 origin, float offset = 0f, int layerMask = 0)
        {
            int layer = layerMask == 0 ? GroundLayerMask : layerMask;
            RaycastHit hitInfo;
            if (Physics.Raycast(origin, Vector3.down, out hitInfo, 5f, GroundLayerMask) || Physics.Raycast(origin, Vector3.up, out hitInfo, 5f, layer))
            {
                return hitInfo.point + Vector3.up * offset;
            }
            else
            {
                Debug.Log("[MathHelper][PlaceGroundHeight] No ground position found.");
                return origin;
            }
        }
    }

}