using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Morkilian
{
    [CustomPropertyDrawer(typeof(MinMaxCurved), true)]
    public class MinMaxCurvedDrawer : PropertyDrawer
    {
        private const float MINMAX_WIDTH = 0.25f;
        private const float MINMAX_START = 0.3f;
        private const float CURVE_WIDTH = 0.45f;
        private const float CURVE_START = 0.55f;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float width = position.width;
            Rect labelRect = new Rect(position);
            labelRect.width = (1f - CURVE_WIDTH - MINMAX_WIDTH) * width;

            Rect minmaxRect = new Rect(position);
            minmaxRect.x = MINMAX_START * width;
            minmaxRect.width = MINMAX_WIDTH * width;

            Rect curveRect = new Rect(position);
            curveRect.x = CURVE_START * width + 3;
            curveRect.width = CURVE_WIDTH * width - 6;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(labelRect, property.displayName);
            EditorGUI.PropertyField(minmaxRect, property.FindPropertyRelative("minMax"), GUIContent.none);
            EditorGUI.PropertyField(curveRect, property.FindPropertyRelative("curve"), GUIContent.none);
            EditorGUI.EndProperty();
        }
        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    return 20f;
        //}
    } 
}
