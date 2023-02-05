using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private int _nextScene;
    [SerializeField] private Image _image;
    [SerializeField] private Button _next;
    [SerializeField] private Button _credits;

    private void Awake()
    {
        var t = transform as RectTransform;

        t.anchoredPosition = Vector3.zero;

        _next.interactable = false;

        _image.DOFade(0f, 0.25f).OnComplete(EnableButtons);
    }

    private void OnDestroy()
    {
        _next.onClick.RemoveAllListeners();
        _credits.onClick.RemoveAllListeners();
    }

    private void ShowResult()
    {
        
        gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(_image.DOFade(1f, 0.25f))
            .AppendCallback(EnableButtons)
            .Append(_image.DOFade(0f, 0.2f));
    }

    private void GoToMain()
    {
        _next.interactable = false;
        _credits.interactable = false;
        _image.raycastTarget = true;

        DOTween.Kill(_image);
        _image.DOFade(1f, 0.25f).OnComplete(LoadMain);
    }

    private void LoadMain()
    {
        SceneManager.LoadScene(_nextScene);
    }

    private void ShowCredits()
    {
        //DOTween.Kill(_image);
        //_image.DOFade(1f, 0f);
    }

    private void EnableButtons()
    {
        _next.interactable = true;
        _credits.interactable = true;
        _image.raycastTarget = false;

        _next.onClick.AddListener(GoToMain);
        _credits.onClick.AddListener(ShowCredits);
    }
}
