using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private int _nextScene;
    [SerializeField] private Image _image;
    [SerializeField] private Button _next;

    private void Awake()
    {
        var t = transform as RectTransform;

        t.anchoredPosition = Vector3.zero;

        _next.interactable = false;
        _image.DOFade(0f, 0);
    }

    void Start()
    {
        MessageDispatcher.OnGameOver += ShowResult;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MessageDispatcher.OnGameOver -= ShowResult;
        _next.onClick.RemoveAllListeners();
    }

    public void LoadRetry()
    {
        _next.onClick.RemoveAllListeners();
        SceneManager.LoadScene(_nextScene);
    }

    private void ShowResult()
    {
        _next.onClick.AddListener(LoadRetry);
        gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(_image.DOFade(1f, 0.25f))
            .AppendCallback(EnableButtons);
    }

    private void EnableButtons()
    {
        _next.interactable = true;
    }
}
