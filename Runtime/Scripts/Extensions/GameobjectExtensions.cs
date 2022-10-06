using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian.Helper
{
    public static class GameobjectExtensions 
    {
        static public GameObject GetChildGameObject(this GameObject fromGameObject, string withName)
        {
            Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
            return null;
        }

    }

}