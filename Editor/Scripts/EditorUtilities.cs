namespace Morkilian.HelperEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using UnityEditor;
    using System.Reflection;
    public class EditorUtilities
    {
        public enum IconType { Label, IconName, IconDot }
        /// <summary>
        /// Draws an icon to the gameobject of the given type.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="idx"></param>
        /// <param name="type"></param>
        public static void DrawIcon(GameObject gameObject, int idx, IconType type)
        {
            string texName = "";
            switch (type)
            {
                case IconType.Label:
                    texName = "sv_label_";
                    break;
                case IconType.IconDot:
                    texName = "sv_icon_name_";
                    break;
                case IconType.IconName:
                    texName = "sv_icon_dot_pix16_gizmo_";
                    break;
            }
            var largeIcons = GetTextures(texName, string.Empty, 0, 8);
            var icon = largeIcons[idx];
            var egu = typeof(EditorGUIUtility);
            var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            var args = new object[] { gameObject, icon.image };
            var setIcon = egu.GetMethod("SetIconForObject", flags, null, new Type[] { typeof(UnityEngine.Object), typeof(Texture2D) }, null);
            setIcon.Invoke(null, args);
        }
        private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {
            GUIContent[] array = new GUIContent[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = EditorGUIUtility.IconContent(baseName + (startIndex + i) + postFix);
            }
            return array;
        }
    }

}