using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AudioSource))]
/// <summary>
/// カスタムボタンクラス
/// </summary>
public class CustomButton : MonoBehaviour, 
    IPointerClickHandler,
    IPointerDownHandler, 
    IPointerUpHandler, 
    IPointerEnterHandler, 
    IPointerExitHandler
{
    [Header("Sprite & Image")]
    [SerializeField, Tooltip("ポインタがオブジェクトを押下した時に差し替えるスプライト")] 
    Sprite _onPointerDownSprite;

    [SerializeField, Tooltip("ポインタがオブジェクトに乗った時に表示するイメージ")] 
    Image _onPointerEnterImage;

    Image _buttonImage;
    Sprite _mainSprite;

    [Header("Audio")]
    [SerializeField, Tooltip("ポインタがオブジェクトに乗った時に出す音源")] 
    AudioClip _onPointerEnterAudio;

    [SerializeField, Tooltip("オブジェクト上でポインタを押下し、同一のオブジェクト上で離した時に出す音源")] 
    AudioClip _onPointerClickAudio;

    AudioSource _audioSource;

    Animator _anim;

    [HideInInspector] public TransitionType Transition;

    [HideInInspector] public Color[] Colors = new Color[5];

    [HideInInspector] public Sprite[] Sprites = new Sprite[5];

    [HideInInspector] public string[] Animations = new string[5];

    /// <summary>
    /// クリックした時にさせたい処理
    /// </summary>
    public event Action OnClickCallback;

    private void Start()
    {
        _buttonImage = GetComponent<Image>();
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _mainSprite = _buttonImage.sprite;

        if(_onPointerEnterImage)
        _onPointerEnterImage.enabled = false;
    }
    /// <summary>
    /// クリック
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) 
    {
        OnClickCallback?.Invoke();
        Debug.Log("クリック");

        if (_onPointerClickAudio)
        {
            _audioSource.PlayOneShot(_onPointerClickAudio);
        }

        ChangeTransition(1);
    }
    /// <summary>
    /// タップダウン
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData) 
    {
        ChangeTransition(2);
    }
    /// <summary>
    /// タップアップ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData) 
    {
        ChangeTransition(0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_onPointerEnterImage)
        {
            _onPointerEnterImage.enabled = true;
        }

        if (_onPointerEnterAudio)
        {
            _audioSource.PlayOneShot(_onPointerEnterAudio);
        }

        ChangeTransition(1);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_onPointerEnterImage)
        {
            _onPointerEnterImage.enabled = false;
        }

        ChangeTransition(0);
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
}
public enum TransitionType
{
    None,
    Color,
    Sprite,
    Animation,
}
