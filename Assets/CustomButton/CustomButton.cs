using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AudioSource))]
/// <summary>
/// カスタムボタンクラス
/// </summary>
public class CustomButton : Selectable, ISubmitHandler, IPointerClickHandler
{
    Image _buttonImage;
    AudioSource _audioSource;
    Animator _anim;
    bool _isSelect;

    public Image OnSelectImage;

    public AudioClip OnPointerEnterAudio;

    public AudioClip OnPointerClickAudio;

    public TransitionType Transition;

    public Type Type;

    public Color[] Colors = new Color[5];

    public float FadeDuration = 0.1f;

    public Sprite[] Sprites = new Sprite[5];

    public string[] Animations = new string[5];

    public string SceneName;

    /// <summary>
    /// クリックした時にさせたい処理
    /// </summary>
    public event Action OnClickCallback;

    private void Start()
    {
        _buttonImage = GetComponent<Image>();
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }
    /// <summary>
    /// クリック
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) 
    {
        //Debug.Log("クリック");

        if (OnPointerClickAudio)
        {
            _audioSource.PlayOneShot(OnPointerClickAudio);
        }

        Press();
        ChangeTransition(1);
    }
    /// <summary>
    /// タップダウン
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData) 
    {
        ChangeTransition(2);
    }
    /// <summary>
    /// タップアップ
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerUp(PointerEventData eventData) 
    {
        if (!_isSelect) return;

        ChangeTransition(1);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (OnSelectImage)
        {
            OnSelectImage.enabled = true;
        }

        if (OnPointerEnterAudio)
        {
            _audioSource.PlayOneShot(OnPointerEnterAudio);
        }

        ChangeTransition(1);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (OnSelectImage)
        {
            OnSelectImage.enabled = false;
        }

        ChangeTransition(0);
    }
    /// <summary>
    /// クリック
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSubmit(BaseEventData eventData)
    {
        //Debug.Log("クリック");

        if (OnPointerClickAudio)
        {
            _audioSource.PlayOneShot(OnPointerClickAudio);
        }

        Press();
        ChangeTransition(2);

        StartCoroutine(OnFinishSubmit());
    }
    /// <summary>
    /// 選択
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnSelect(BaseEventData eventData)
    {
        _isSelect = true;

        if (OnSelectImage)
        {
            OnSelectImage.enabled = true;
        }

        if (OnPointerEnterAudio)
        {
            _audioSource.PlayOneShot(OnPointerEnterAudio);
        }

        ChangeTransition(3);
    }
    /// <summary>
    /// 選択解除
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDeselect(BaseEventData eventData)
    {
        _isSelect = false;

        if (OnSelectImage)
        {
            OnSelectImage.enabled = false;
        }

        ChangeTransition(4);
    }
    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = FadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        if (_isSelect)
            ChangeTransition(1);
    }
    private void ChangeTransition(int i)
    {
        switch (Transition)
        {
            case TransitionType.None:
                break;

            case TransitionType.Color:
                _buttonImage.color = Colors[i];
                break;

            case TransitionType.Sprite:
                _buttonImage.sprite = Sprites[i];
                break;

            case TransitionType.Animation:
                _anim.SetTrigger(Animations[i]);
                break;
        }
    }
    private void Press()
    {
        switch(Type)
        {
            case Type.Normal:
                OnClickCallback?.Invoke();
                break;

            case Type.SceneChangeSingle:
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
                break;

            case Type.SceneChangeAdditive:
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
                break;
        }
    }
}
public enum TransitionType
{
    None,
    Color,
    Sprite,
    Animation,
}
public enum Type
{
    Normal,
    SceneChangeSingle,
    SceneChangeAdditive,
}