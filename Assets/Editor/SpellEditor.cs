using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpellBase))]
public class SpellEditor : Editor
{
    private SpellBase SpellBase;
    private void Awake()
    {
        SpellBase = (SpellBase)target;
    }
    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("New Item"))
        {
            SpellBase.CreateSpell();
        }
        if (GUILayout.Button("Remove"))
        {
            SpellBase.RemoveSpell();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<"))
        {
            SpellBase.PrevSpell();
        }
        if (GUILayout.Button(">"))
        {
            SpellBase.NextSpell();
        }
        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
