using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _other;

    private bool _canStart;

    private void Awake()
    {
        var t = transform as RectTransform;
        t.anchoredPosition = Vector3.zero;

        _image.DOFade(0f, 0.25f).OnComplete(ShowResult).SetId(this);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }

    private void ShowResult()
    {
        _canStart = true;
    }

    private void Update()
    {
        if(_canStart && Input.anyKeyDown)
        {
            _canStart = false;
            DOTween.Sequence()
                .Append(_image.DOFade(1f, 0.25f))
                .AppendCallback(HideOther)
                .Append(_image.DOFade(0f, 0.25f))
                .OnComplete(StartGame).SetId(this);
        }
    }

    private void HideOther()
    {
        _other.SetActive(false);
    }

    private void StartGame()
    {
        MessageDispatcher.NotifyGameStarted();
        gameObject.SetActive(false);
    }
}
