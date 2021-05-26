namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [ExecuteInEditMode]
    public class PauseEditor : MonoBehaviour
    {
        public KeyCode PauseKey = KeyCode.P;



        void Update()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying && Input.GetKeyDown(PauseKey))
            {
                EditorApplication.isPaused = !EditorApplication.isPaused;
            } 
#endif
        }
    }

}