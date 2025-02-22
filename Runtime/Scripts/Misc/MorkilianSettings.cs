using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Morkilian
{
    /// <summary>
    /// Enum for the Logger or other debugging masks
    /// </summary>
    [System.Flags]
    public enum eLoggerTypes
    {
        None = 0,
        Types = 1 << 1,
        Extensions = 1 << 2,
        Editor = 1 << 3,
        Misc = 1 << 64 //Max enum flags
    }

    public class MorkilianSettings : ScriptableObject
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitInstance()
        {
            Logger.Log(Instance.ToString(), additionalCondition: false); //Force init
        } 
#endif
        private static MorkilianSettings _instance;
        public static MorkilianSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    //Find a way to find this instance in a build yourself
#if UNITY_EDITOR
                    string[] guids = AssetDatabase.FindAssets($"t:{nameof(MorkilianSettings)}");
                    if (guids.Length > 0)
                    {
                        string guid = AssetDatabase.FindAssets($"t:{nameof(MorkilianSettings)}")[0];
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        _instance = AssetDatabase.LoadAssetAtPath<MorkilianSettings>(path);
                    }
                    if(_instance == null)
                    {
                        MorkilianSettings settings = new MorkilianSettings();
                        if (AssetDatabase.IsValidFolder("Assets/AssetsSettings") == false)
                            AssetDatabase.CreateFolder("Assets", "AssetsSettings");
                        AssetDatabase.CreateAsset(settings, path: "Assets/AssetsSettings/MorkilianSettings.asset");
                        _instance = settings;
                        AssetDatabase.SaveAssets();
                    }
#endif
                }
                return _instance;
            }
        }
        public static bool HasInstance = _instance != null;

        public eLoggerTypes DebugMask = (eLoggerTypes)(-1);
    } 
}
