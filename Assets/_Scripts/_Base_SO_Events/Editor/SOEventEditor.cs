using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SOEvents;

/*
[CustomEditor(typeof(SOEvent)), CanEditMultipleObjects]
public class SOEventEditor : Editor
{
    public SerializedProperty listeners_Prop;

    void OnEnable()
    {
        listeners_Prop = serializedObject.FindProperty("listeners");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(listeners_Prop, new GUIContent("Listeners"));

        serializedObject.ApplyModifiedProperties();
    }
}
*/