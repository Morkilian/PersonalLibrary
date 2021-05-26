namespace Morkilian.Helper
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;

    public class UnityExtededShortKeys : ScriptableObject
    {
        //https://answers.unity.com/questions/1204247/intercept-ctrl-s-keyboard-shortcut-in-editor-for-c.html?childToView=1204307#answer-1204307
        //[MenuItem("HotKey/Run _F5")]
        //[MenuItem("HotKey/Run2 _%H")]
        //static void PlayGame()
        //{
        //    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
        //    EditorApplication.ExecuteMenuItem("Edit/Play");
        //}

#if UNITY_EDITOR
        //https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f9
        [MenuItem("HotKey/Pause or Unpause _p")]
        static void PauseAndUnpausePlayMode()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPaused = !EditorApplication.isPaused;
            }
        } 
#endif
    } 
}