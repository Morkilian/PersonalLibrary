using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(Morkilian.LerpedValue),true)]
public class LerpedValueEditor : PropertyDrawer
{
    private const float MAINLABEL_WIDTH = 0.1f;
    //private const float LABELWIDTH_WIDTH = 0.45f; //1f-this is the fieldwidth
    private const float SEPARATION_WIDTH = 6f;
    private const float FIELD_HEIGHT = 20f;
    private const float SEPARATION_HEIGHT = 3f;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty currentProp = property.FindPropertyRelative("currentValue");
        SerializedProperty targetProp = property.FindPropertyRelative("targetValue");
        SerializedProperty speedProp = property.FindPropertyRelative("m_lerpingSpeed");
        SerializedProperty methodProp = property.FindPropertyRelative("m_lerpingMethod");

        float width = position.width;
        Rect labelRect = new Rect(position);
        labelRect.width = MAINLABEL_WIDTH * width;
        labelRect.y -= FIELD_HEIGHT -2;

        float singleFieldWidth = (width - labelRect.width-SEPARATION_WIDTH*3f)*0.5f;

        Rect currentRect = new Rect(position);
        currentRect.x += labelRect.width + SEPARATION_WIDTH;
        currentRect.width = singleFieldWidth;
        currentRect.height = FIELD_HEIGHT;

        Rect targetRect = new Rect(currentRect);
        targetRect.x += singleFieldWidth + SEPARATION_WIDTH;

        Rect speedRect = new Rect(currentRect);
        speedRect.y += FIELD_HEIGHT + SEPARATION_HEIGHT;

        Rect methodRect = new Rect(targetRect);
        methodRect.y += FIELD_HEIGHT + SEPARATION_HEIGHT;
        EditorGUIUtility.labelWidth = 50f;
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.LabelField(labelRect, property.displayName);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(currentRect, currentProp, new GUIContent("Current"));
        EditorGUI.EndDisabledGroup();
        EditorGUI.PropertyField(targetRect, targetProp, new GUIContent("Target"));
        EditorGUI.PropertyField(methodRect, methodProp, new GUIContent("Method"));
        EditorGUI.PropertyField(speedRect, speedProp, new GUIContent("Speed"));
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return FIELD_HEIGHT*2 + SEPARATION_HEIGHT*3f;
    }

    //private void PrepareRects(Rect inRect, SerializedProperty propA, SerializedProperty propB, string nameA, string nameB)
    //{
    //    float totalSpaceLeft = 1f - MAINLABEL_WIDTH; //%
    //    totalSpaceLeft = inRect.width* totalSpaceLeft - SEPARATION_WIDTH * 3f;
    //    float spaceByFullLabelField = totalSpaceLeft * 0.5f;
    //    Rect labelAndFieldRect = new Rect(inRect);
    //    labelAndFieldRect.x += inRect.width*MAINLABEL_WIDTH + SEPARATION_WIDTH;
    //    labelAndFieldRect.width = spaceByFullLabelField;
    //    //Prop A
    //    Rect LabelA = new Rect(labelAndFieldRect) { width = spaceByFullLabelField * LABELWIDTH_WIDTH };
    //    Rect FieldA = new Rect(labelAndFieldRect) { width = spaceByFullLabelField * (1 - LABELWIDTH_WIDTH) };

    //    Rect LabelB = new Rect(labelAndFieldRect)
    //    {
    //        width = spaceByFullLabelField * LABELWIDTH_WIDTH,
    //        x = labelAndFieldRect.x +spaceByFullLabelField + SEPARATION_WIDTH
    //    };
    //    Rect FieldB = new Rect(labelAndFieldRect)
    //    {
    //        width = spaceByFullLabelField * (1 - LABELWIDTH_WIDTH),
    //        x = labelAndFieldRect.x + spaceByFullLabelField * (1 + (1 - LABELWIDTH_WIDTH)) + SEPARATION_WIDTH
    //    };

    //    EditorGUI.LabelField(LabelA, nameA);
    //    EditorGUI.PropertyField(FieldA,propA);
    //    EditorGUI.LabelField(LabelB, nameB);
    //    EditorGUI.PropertyField(FieldB, propB);

    //}
}
