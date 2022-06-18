using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var transition = target as CustomButton;

        EditorGUI.BeginChangeCheck();

        transition.Transition = (Type)EditorGUILayout.EnumPopup("Transition", transition.Transition);

        if(EditorGUI.EndChangeCheck())
        {
            switch(transition.Transition)
            {
                case Type.None:
                    Debug.Log("None");
                    break;

                case Type.Color:
                    Debug.Log("Color");
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.EndVertical();
                    break;

                case Type.Sprite:
                    Debug.Log("Sprite");
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.EndVertical();
                    break;

                case Type.Animation:
                    Debug.Log("Animation");
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.EndVertical();
                    break;
            }
        }
    }
}