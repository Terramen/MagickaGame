using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(CustomJoystick))]
public class CustomJoystickEditor : JoystickEditor
{
    private SerializedProperty moveThreshold;
    private SerializedProperty joystickType;
    
    private SerializedProperty spellTargetRaduis;
    private SerializedProperty spellHandle;
    private SerializedProperty spellIcon;

    protected override void OnEnable()
    {
        base.OnEnable();
        moveThreshold = serializedObject.FindProperty("moveThreshold");
        joystickType = serializedObject.FindProperty("joystickType");
        spellTargetRaduis = serializedObject.FindProperty("spellTargetRaduis");
        spellHandle = serializedObject.FindProperty("spellHandle");
        spellIcon = serializedObject.FindProperty("spellIcon");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (background != null)
        {
            RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
            backgroundRect.pivot = center;
        }
    }

    protected override void DrawValues()
    {
        base.DrawValues();
        EditorGUILayout.PropertyField(moveThreshold, new GUIContent("Move Threshold", "The distance away from the center input has to be before the joystick begins to move."));
        EditorGUILayout.PropertyField(joystickType, new GUIContent("Joystick Type", "The type of joystick the variable joystick is current using."));
        EditorGUILayout.PropertyField(spellTargetRaduis, new GUIContent("SpellTargetRaduis", "The distance away from the center input has to be before the joystick begins to move."));
        EditorGUILayout.PropertyField(spellIcon, new GUIContent("SpellIcon", "The type of joystick the variable joystick is current using."));
        EditorGUILayout.PropertyField(spellHandle, new GUIContent("SpellHandle", "The type of joystick the variable joystick is current using."));

        
    }
}
