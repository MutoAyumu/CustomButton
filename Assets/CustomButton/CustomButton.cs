using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AudioSource))]
/// <summary>
/// �J�X�^���{�^���N���X
/// </summary>
public class CustomButton : Selectable, ISubmitHandler, IPointerClickHandler
{
    Image _buttonImage;
    AudioSource _audioSource;
    Animator _anim;
    bool _isSelect;
    bool _isPointer;

    public Image OnSelectImage;

    public AudioClip OnPointerEnterAudio;

    public AudioClip OnPointerClickAudio;

    public TransitionType Transition;
    public ColorBlock test;

    public Type Type;

    public Color[] Colors = new Color[5];

    public float FadeDuration = 0.1f;

    public Sprite[] Sprites = new Sprite[5];

    public string[] Animations = new string[5];

    public string SceneName;

    /// <summary>
    /// �N���b�N�������ɂ�����������(�����Ȃ�)
    /// </summary>
    public ClickCallback OnClickCallback = new ClickCallback();

    private void Start()
    {
        _buttonImage = GetComponent<Image>();
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }
    /// <summary>
    /// �N���b�N
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) 
    {
        //Debug.Log("�N���b�N");

        if (OnPointerClickAudio)
        {
            _audioSource.PlayOneShot(OnPointerClickAudio);
        }

        Press();
        ChangeTransition(1);
    }
    /// <summary>
    /// �^�b�v�_�E��
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData) 
    {
        if (IsInteractable() && navigation.mode != Navigation.Mode.None)
            EventSystem.current.SetSelectedGameObject(gameObject, eventData);

        ChangeTransition(2);
    }
    /// <summary>
    /// �^�b�v�A�b�v
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerUp(PointerEventData eventData) 
    {
        if (!_isSelect) return;

        ChangeTransition(1);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        _isPointer = true;

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
        _isPointer = false;

        if (_isSelect) return;

        if (OnSelectImage)
        {
            OnSelectImage.enabled = false;
        }

        ChangeTransition(0);
    }
    /// <summary>
    /// �N���b�N
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSubmit(BaseEventData eventData)
    {
        if (_isPointer) return;

        //Debug.Log("�N���b�N");

        if (OnPointerClickAudio)
        {
            _audioSource.PlayOneShot(OnPointerClickAudio);
        }

        Press();
        ChangeTransition(2);

        StartCoroutine(OnFinishSubmit());
    }
    /// <summary>
    /// �I��
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
    /// �I������
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
        if (!IsInteractable()) return;

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
                foreach (var t in Animations)
                    _anim.ResetTrigger(t);
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
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        
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

[Serializable]
public class ClickCallback : UnityEvent { }