using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CustomButton))]
[CanEditMultipleObjects]
public class CustomButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var transition = target as CustomButton;

        EditorGUI.BeginChangeCheck();



        serializedObject.Update();

        transition.Transition = (TransitionType)EditorGUILayout.EnumPopup("Transition", transition.Transition);

        switch (transition.Transition)
        {
            case TransitionType.None:
                break;

            case TransitionType.Color:
                TypeColor(transition);
                break;

            case TransitionType.Sprite:
                TypeSprite(transition);
                break;

            case TransitionType.Animation:
                TypeAnimation(transition);
                break;
        }

        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
        {
            var scene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);
        }
    }

    private static void TypeAnimation(CustomButton transition)
    {
        GUILayout.BeginVertical(GUI.skin.box);

        transition.Animations[0] = EditorGUILayout.TextField("Normal Trigger", transition.Animations[0]);
        transition.Animations[1] = EditorGUILayout.TextField("Highlighted Trigger", transition.Animations[1]);
        transition.Animations[2] = EditorGUILayout.TextField("Pressed Trigger", transition.Animations[2]);
        transition.Animations[3] = EditorGUILayout.TextField("Selected Trigger", transition.Animations[3]);
        transition.Animations[4] = EditorGUILayout.TextField("Disabled Trigger", transition.Animations[4]);

        GUILayout.EndVertical();
    }

    private void TypeSprite(CustomButton transition)
    {
        GUILayout.BeginVertical(GUI.skin.box);

        transition.Sprites[0] = (Sprite)EditorGUILayout.ObjectField("Normal Sprite", transition.Sprites[0], typeof(Sprite), false);
        transition.Sprites[1] = (Sprite)EditorGUILayout.ObjectField("Highlighted Sprite", transition.Sprites[1], typeof(Sprite), false);
        transition.Sprites[2] = (Sprite)EditorGUILayout.ObjectField("Pressed Sprite", transition.Sprites[2], typeof(Sprite), false);
        transition.Sprites[3] = (Sprite)EditorGUILayout.ObjectField("Selected Sprite", transition.Sprites[3], typeof(Sprite), false);
        transition.Sprites[4] = (Sprite)EditorGUILayout.ObjectField("Disabled Sprite", transition.Sprites[4], typeof(Sprite), false);

        GUILayout.EndVertical();
    }

    private void TypeColor(CustomButton transition)
    {
        GUILayout.BeginVertical(GUI.skin.box);

        transition.Colors[0] = EditorGUILayout.ColorField("Normal Color", transition.Colors[0]);
        transition.Colors[1] = EditorGUILayout.ColorField("Highlighted Color", transition.Colors[1]);
        transition.Colors[2] = EditorGUILayout.ColorField("Pressed Color", transition.Colors[2]);
        transition.Colors[3] = EditorGUILayout.ColorField("Selected Color", transition.Colors[3]);
        transition.Colors[4] = EditorGUILayout.ColorField("Disabled Color", transition.Colors[4]);

        GUILayout.EndVertical();
    }
}