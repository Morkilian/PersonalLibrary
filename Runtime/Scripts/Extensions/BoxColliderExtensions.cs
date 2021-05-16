namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class BoxColliderExtensions
    {
        public static bool Overlap(this BoxCollider box, int layerMask = 1)
        {
            return Physics.OverlapBox(box.transform.TransformPoint(box.center),
                box.size * 0.5f,
                box.transform.rotation, layerMask).Length > 0;
        }

    }

}