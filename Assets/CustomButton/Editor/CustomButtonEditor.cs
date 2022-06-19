using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[CustomEditor(typeof(CustomButton))]
[CanEditMultipleObjects]
public class CustomButtonEditor : Editor
{
    bool _isAudio;
    private static Vector2 ImageElementSize = new Vector2(160f, 30f);

    public override void OnInspectorGUI()
    {
        var button = target as CustomButton;

        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
        //

        button.Transition = (TransitionType)EditorGUILayout.EnumPopup("Transition", button.Transition);

        switch (button.Transition)
        {
            case TransitionType.None:
                break;

            case TransitionType.Color:
                TypeColor(button);
                break;

            case TransitionType.Sprite:
                TypeSprite(button);
                break;

            case TransitionType.Animation:
                TypeAnimation(button);
                break;
        }

        EditorGUILayout.Space();

        _isAudio = EditorGUILayout.Foldout(_isAudio, "Audio");

        if (_isAudio)
        {
            EditorGUI.indentLevel++;
            button.OnPointerEnterAudio = (AudioClip)EditorGUILayout.ObjectField("OnPointerEnterAudio", button.OnPointerEnterAudio, typeof(AudioClip), false);
            button.OnPointerClickAudio = (AudioClip)EditorGUILayout.ObjectField("OnPointerEnterAudio", button.OnPointerClickAudio, typeof(AudioClip), false);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        button.OnSelectImage = (Image)EditorGUILayout.ObjectField("OnSelectImage", button.OnSelectImage, typeof(Image), true);

        EditorGUILayout.Space();

        button.Type = (Type)EditorGUILayout.EnumPopup("Type", button.Type);

        switch(button.Type)
        {
            case Type.Normal:
                break;

            case Type.SceneChangeSingle:
                EditorGUI.indentLevel++;
                button.SceneName = EditorGUILayout.TextField("SceneName", button.SceneName);
                EditorGUI.indentLevel--;
                break;

            case Type.SceneChangeAdditive:
                EditorGUI.indentLevel++;
                button.SceneName = EditorGUILayout.TextField("SceneName", button.SceneName);
                EditorGUI.indentLevel--;
                break;
        }
        //
        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck() && !EditorApplication.isPlaying)
        {
            var scene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);
        }
    }

    private void TypeAnimation(CustomButton transition)
    {
        EditorGUI.indentLevel++;

        transition.Animations[0] = EditorGUILayout.TextField("Normal Trigger", transition.Animations[0]);
        transition.Animations[1] = EditorGUILayout.TextField("Highlighted Trigger", transition.Animations[1]);
        transition.Animations[2] = EditorGUILayout.TextField("Pressed Trigger", transition.Animations[2]);
        transition.Animations[3] = EditorGUILayout.TextField("Selected Trigger", transition.Animations[3]);
        transition.Animations[4] = EditorGUILayout.TextField("Disabled Trigger", transition.Animations[4]);

        EditorGUI.indentLevel--;
    }

    private void TypeSprite(CustomButton transition)
    {
        EditorGUI.indentLevel++;

        transition.Sprites[0] = (Sprite)EditorGUILayout.ObjectField("Normal Sprite", transition.Sprites[0], typeof(Sprite), false);
        transition.Sprites[1] = (Sprite)EditorGUILayout.ObjectField("Highlighted Sprite", transition.Sprites[1], typeof(Sprite), false);
        transition.Sprites[2] = (Sprite)EditorGUILayout.ObjectField("Pressed Sprite", transition.Sprites[2], typeof(Sprite), false);
        transition.Sprites[3] = (Sprite)EditorGUILayout.ObjectField("Selected Sprite", transition.Sprites[3], typeof(Sprite), false);
        transition.Sprites[4] = (Sprite)EditorGUILayout.ObjectField("Disabled Sprite", transition.Sprites[4], typeof(Sprite), false);

        EditorGUI.indentLevel--;
    }

    private void TypeColor(CustomButton transition)
    {
        EditorGUI.indentLevel++;

        transition.Colors[0] = EditorGUILayout.ColorField("Normal Color", transition.Colors[0]);
        transition.Colors[1] = EditorGUILayout.ColorField("Highlighted Color", transition.Colors[1]);
        transition.Colors[2] = EditorGUILayout.ColorField("Pressed Color", transition.Colors[2]);
        transition.Colors[3] = EditorGUILayout.ColorField("Selected Color", transition.Colors[3]);
        transition.Colors[4] = EditorGUILayout.ColorField("Disabled Color", transition.Colors[4]);

        transition.FadeDuration = EditorGUILayout.FloatField("FadeDuration", transition.FadeDuration);

        EditorGUI.indentLevel--;
    }

    [MenuItem("GameObject/UI/CustomButton", false, 2003)]
    static public void CreateCustomButton()
    {
        // 選択状態のGameObjectを取得する
        var parent = Selection.activeGameObject?.transform;
        // 親や祖先にCanvasが存在しない場合
        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            // 新規Canvasの生成
            var canvas = new GameObject("Canvas");
            canvas.transform.SetParent(parent);
            // Canvasの初期化
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            // 親の付け替え
            parent = canvas.transform;

            var eventSystem = FindObjectOfType<EventSystem>();

            if (!eventSystem)
            {
                var newEventSystem = new GameObject("EventSystem");
                newEventSystem.AddComponent<EventSystem>();
                newEventSystem.AddComponent<StandaloneInputModule>();
            }
        }
        var go = new GameObject("CustomButton");
        // RectTransformの初期化
        var buttonRect = go.AddComponent<RectTransform>();
        
        buttonRect.SetParent(parent);
        buttonRect.sizeDelta = ImageElementSize;
        buttonRect.anchoredPosition = Vector2.zero;
        // 生成したGameObjectを選択状態にする
        Selection.activeGameObject = go;
        
        go.AddComponent<CustomButton>();

        var t = new GameObject("Text");
        t.transform.SetParent(buttonRect);

        var text = t.AddComponent<Text>();
        text.text = "New Text";
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;

        var textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = Vector3.zero;
    }
}