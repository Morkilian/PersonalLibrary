using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Morkilian.Helper
{
    [CustomEditor(typeof(MultiObjectsEditing), true)]
    [CanEditMultipleObjects]
    public class MultiObjectsEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }
    } 
}
